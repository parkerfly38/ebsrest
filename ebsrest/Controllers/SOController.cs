using Dapper;
using ebsrest.Models;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Description;

namespace ebsrest.Controllers
{
    public class SOController : ApiController
    {
        SqlHandler sqlHandler = new SqlHandler();

        /// <summary>
        /// Submits an Order or Quote to Sage 500.
        /// </summary>
        /// <param name="orderRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SubmitOrder")]
        [ResponseType(typeof(OrderResponse))]
        public IHttpActionResult SubmitOrder(OrderRequest orderRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            orderRequest.CompID = orderRequest.CompID.Replace("'", "''");
            orderRequest.CustID = orderRequest.CustID.Replace("'", "''");
            orderRequest.CustPONo = orderRequest.CustPONo.Replace("'", "''");
            orderRequest.ShipToName = orderRequest.ShipToName.Replace("'", "''");
            orderRequest.TranCmnt = orderRequest.TranCmnt.Replace("'", "''");
            orderRequest.UserFld1 = orderRequest.UserFld1.Replace("'", "''");
            orderRequest.UserFld2 = orderRequest.UserFld2.Replace("'", "''");
            orderRequest.UserFld3 = orderRequest.UserFld3.Replace("'", "''");
            orderRequest.UserFld4 = orderRequest.UserFld4.Replace("'", "''");
            orderRequest.PmtTermsID = orderRequest.PmtTermsID.Replace("'", "''");

            if (string.IsNullOrEmpty(orderRequest.CustID))
            {
                return BadRequest("Must provide CustID");
            }
            if (Common.IsValidCustID(orderRequest.CustID, orderRequest.CompID))
            {
                return BadRequest("Invalid Customer ID");
            }

            orderRequest.sessKey = Common.GetSessionKey();

            switch (orderRequest.TranTypeString)
            {
                case "SO":
                    orderRequest.tranType = 801;
                    break;
                case "QT": case "QO":
                    orderRequest.tranType = 840;
                    break;
                default:
                    orderRequest.tranType = 801;
                    break;
            }

            if (orderRequest.ShipMethID == "0")
                orderRequest.ShipMethID = string.Empty;

            orderRequest.PmtTermsKey = Common.GetPmtTermsKey(orderRequest.PmtTermsID, orderRequest.CompID);

            //if no request date in header, add it now
            orderRequest.RequestDate = DateTime.Now;

            if (orderRequest.ShipToCountryID == "US")
                orderRequest.ShipToCountryID = "USA";

            int POLineCount = 0;
            int CmdTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["SQLCommandTimeout"]);

            orderRequest.lines = AllocFreighAndTax(orderRequest.lines, orderRequest.FreightAmt, orderRequest.FreightAllocMethQorA, orderRequest.TradeDiscAmt, orderRequest.TradeDiscPct, orderRequest.CompID);

            decimal totalPrice = 0;
            decimal lineTax = 0;
            foreach (var line in orderRequest.lines)
            {
                totalPrice += line.Price;
            }

            if (totalPrice == 0)
            {
                return BadRequest("No price on items, failure to submit");
            }

            foreach (var line in orderRequest.lines)
            {
                POLineCount++;

                if (string.IsNullOrEmpty(line.Description))
                {
                    line.Description = Common.GetItemDesc(orderRequest.CompID, line.ItemID);
                    if (string.IsNullOrEmpty(line.Description))
                        line.Description = line.ItemID;
                }

                if (line.POLineNo == 0)
                    line.POLineNo = POLineCount;

                //validate delivery method
                if (line.DelMethod != 1 && line.DelMethod != 2 && line.DelMethod != 3 && line.DelMethod != 4)
                {
                    //invalid order
                    return BadRequest("Line " + POLineCount.ToString() + ", Delivery Method " + line.DelMethod.ToString() + " is not valid.  Must be 1 (Ship), 2 (Drop Ship), 3 (Counter Sale) or 4 (Will Call)");
                }

                if (!string.IsNullOrEmpty(line.VendID))
                {
                    //vendor can only be provided if the line is drop ship
                    if (line.DelMethod != 2)
                    {
                        return BadRequest("Line " + POLineCount.ToString() + ", Vendor ID " + line.VendID + " cannot be provided unless line is drop ship.");
                    }
                    //validate vendor
                    if (Common.GetVendorKey(orderRequest.CompID, line.VendID) == 0)
                    {
                        return BadRequest("Line " + POLineCount.ToString() + ", Vendor ID " + line.VendID + " is not a valid MAS vendor.");
                    }
                }

                if (!string.IsNullOrEmpty(line.VendAddrID))
                {
                    if (string.IsNullOrEmpty(line.VendID))
                    {
                        return BadRequest("Line " + POLineCount.ToString() + ", if Vendor Address ID is provided, Vendor ID must also be provided.");
                    }
                    if (Common.GetVendorAddressKey(orderRequest.CompID, line.VendID, line.VendAddrID) == 0)
                    {
                        return BadRequest("Line " + POLineCount.ToString() + ", Vendor Address ID " + line.VendAddrID + " is not a valid MAS address.");
                    }
                }

                //validate item
                if (Common.GetItemKey(line.ItemID, orderRequest.CompID) == 0)
                {
                    return BadRequest("Line " + POLineCount.ToString() + ", Item " + line.ItemID + " is not a valid MAS item.");
                }

                //check line quantity
                if (string.IsNullOrEmpty(line.ItemID) || line.Quantity == 0)
                {
                    return BadRequest("All lines must include at minimum ItemID and Quantity.");
                }

                //PO Line number
                if (line.POLineNo == 0)
                    line.POLineNo = POLineCount;

                SqlHandler sqlHandler = new SqlHandler();
                DynamicParameters lineParameters = new DynamicParameters();
                lineParameters.Add("@ItemID", line.ItemID);
                lineParameters.Add("@Price", line.Price);
                lineParameters.Add("@Quantity", line.Quantity);
                lineParameters.Add("@Description", line.Description);
                lineParameters.Add("@RequestDate", line.RequestDate);
                lineParameters.Add("@ShipDate", line.ShipDate);
                lineParameters.Add("@PromiseDate", line.PromiseDate);
                lineParameters.Add("@POLineNo", line.POLineNo);
                lineParameters.Add("@OrderID", line.OrderID);
                lineParameters.Add("@UnitMeasID", line.UnitMeasID);
                lineParameters.Add("@TaxClassID", line.TaxClassID);
                if (string.IsNullOrEmpty(line.UserFld1))
                {
                    lineParameters.Add("@UserFld1", DBNull.Value);
                }
                else
                {
                    lineParameters.Add("@UserFld1", line.UserFld1);
                }
                if (string.IsNullOrEmpty(line.UserFld2))
                {
                    lineParameters.Add("@UserFld2", DBNull.Value);
                }
                else
                {
                    lineParameters.Add("@UserFld2", line.UserFld2);
                }
                lineParameters.Add("@Backorder", line.Backorder);
                if (line.AcctRefKey == 0)
                {
                    lineParameters.Add("@AcctRefKey", DBNull.Value);
                }
                else
                {
                    lineParameters.Add("@AcctRefKey", line.AcctRefKey);
                }
                if (line.WhseKey == 0)
                {
                    lineParameters.Add("@WhseKey", DBNull.Value);
                }
                else
                {
                    lineParameters.Add("@WhseKey", line.WhseKey);
                }
                lineParameters.Add("@ExtCmnt", line.ExtCmnt);
                lineParameters.Add("@FreightAmt", line.FreightAmt);
                lineParameters.Add("@TradeDiscAmt", line.TradeDiscAmt);
                lineParameters.Add("@SessKey", line.SessKey);

                if (line.STaxSchdKey == 0)
                {
                    lineParameters.Add("@STaxSchdKey", DBNull.Value);
                }
                else
                {
                    lineParameters.Add("@STaxSchdKey", line.STaxSchdKey);
                }
                if (line.ShipMethKey == 0)
                {
                    lineParameters.Add("@ShipMethKey", DBNull.Value);
                }
                else
                {
                    lineParameters.Add("@ShipMethKey", line.ShipMethKey);
                }
                if (string.IsNullOrEmpty(line.ShipMethID))
                {
                    lineParameters.Add("@ShipMethID", DBNull.Value);
                }
                else
                {
                    lineParameters.Add("@ShipMethID", line.ShipMethID);
                }
                if (line.DelMethod == 0)
                {
                    lineParameters.Add("@DelMethod", DBNull.Value);
                }
                else
                {
                    lineParameters.Add("@DelMethod", line.DelMethod);
                }
                if (string.IsNullOrEmpty(line.VendID))
                {
                    lineParameters.Add("@VendID", DBNull.Value);
                }
                else
                {
                    lineParameters.Add("@VendID", line.VendID);
                }
                if (string.IsNullOrEmpty(line.VendAddrID))
                {
                    lineParameters.Add("@VendAddrID", DBNull.Value);
                }
                else
                {
                    lineParameters.Add("@VendAddrID", line.VendAddrID);
                }

                sqlHandler.SQLExecuteWithoutReturn("spsoInsertStgOrderBasicLine_RKL", System.Data.CommandType.StoredProcedure, lineParameters);

                lineTax += line.TaxAmt;
            }

            if (POLineCount == 0)
            {
                return BadRequest("Must provide at least 1 line item.");
            }

            if (string.IsNullOrEmpty(orderRequest.ShipMethID))
            {
                orderRequest.ShipMethID = Common.GetShipMethID(orderRequest.CompID, orderRequest.CustID, orderRequest.ShipToAddrLine1, orderRequest.ShipToAddrLine2, orderRequest.ShipToAddrLine3,
                    orderRequest.ShipToAddrLine4, orderRequest.ShipToAddrLine5, orderRequest.ShipToName, orderRequest.ShipToCity, orderRequest.ShipToCountryID, orderRequest.ShipToStateID, orderRequest.ShipToPostalCode);
                //get customer's ship method for default ship to address
                if (string.IsNullOrEmpty(orderRequest.ShipMethID))
                    orderRequest.ShipMethID = Common.GetShipMethID(orderRequest.CompID, orderRequest.CustID);
            }

            //save order
            if (lineTax != 0)
                orderRequest.TaxAmt = lineTax;

            DynamicParameters orderParameters = new DynamicParameters();
            orderParameters.Add("@OrderID", orderRequest.OrderID);
            orderParameters.Add("@AckDate", DBNull.Value); // we're setting this always null because 500 client cannot delete if value populated
            orderParameters.Add("@CustID", orderRequest.CustID);
            orderParameters.Add("@CustPONo", orderRequest.CustPONo);
            orderParameters.Add("@RequestDate", orderRequest.RequestDate);
            orderParameters.Add("@ShipMethID", orderRequest.ShipMethID);
            orderParameters.Add("@ShipToAddrLine1", orderRequest.ShipToAddrLine1);
            orderParameters.Add("@ShipToAddrLine2", orderRequest.ShipToAddrLine2);
            orderParameters.Add("@ShipToAddrLine3", orderRequest.ShipToAddrLine3);
            orderParameters.Add("@ShipToAddrLine4", orderRequest.ShipToAddrLine4);
            orderParameters.Add("@ShipToAddrLine5", orderRequest.ShipToAddrLine5);
            orderParameters.Add("@ShipToAddrName", orderRequest.ShipToName);
            orderParameters.Add("@ShipToCity", orderRequest.ShipToCity);
            orderParameters.Add("@ShipToCountryID", orderRequest.ShipToCountryID);
            orderParameters.Add("@ShipToPostalCode", orderRequest.ShipToPostalCode);
            orderParameters.Add("@ShipToStateID", orderRequest.ShipToStateID);
            orderParameters.Add("@TranCmnt", orderRequest.TranCmnt);
            if (string.IsNullOrEmpty(orderRequest.UserFld1))
            {
                orderParameters.Add("@UserFld1", DBNull.Value);
            }
            else
            {
                orderParameters.Add("@UserFld1", orderRequest.UserFld1);
            }
            if (string.IsNullOrEmpty(orderRequest.UserFld2))
            {
                orderParameters.Add("@UserFld2", DBNull.Value);
            }
            else
            {
                orderParameters.Add("@UserFld2", orderRequest.UserFld1);
            }
            if (string.IsNullOrEmpty(orderRequest.UserFld3))
            {
                orderParameters.Add("@UserFld3", DBNull.Value);
            }
            else
            {
                orderParameters.Add("@UserFld3", orderRequest.UserFld1);
            }
            if (string.IsNullOrEmpty(orderRequest.UserFld4))
            {
                orderParameters.Add("@UserFld4", DBNull.Value);
            }
            else
            {
                orderParameters.Add("@UserFld4", orderRequest.UserFld1);
            }
            orderParameters.Add("@CompID", orderRequest.CompID);
            orderParameters.Add("@sessKey", orderRequest.sessKey);
            orderParameters.Add("@tranType", orderRequest.tranType);
            orderParameters.Add("@PmtTermsKey", orderRequest.PmtTermsKey);
            if (orderRequest.ExpirationDate == null)
            {
                orderParameters.Add("@ExpirationDate", DBNull.Value);
            }
            else
            {
                orderParameters.Add("@ExpirationDate", orderRequest.ExpirationDate);
            }
            orderParameters.Add("@STaxAmt", orderRequest.TaxAmt);
            if (orderRequest.CalcSalesTax == "Y" || orderRequest.CalcSalesTax == "A")
            {
                orderParameters.Add("@CalcSalesTax", "N");
            }
            else
            {
                orderParameters.Add("@CalcSalesTax", "Y");
            }
            orderParameters.Add("@Hold", orderRequest.Hold);
            orderParameters.Add("@DfltSperKey", orderRequest.DfltSperKey);
            orderParameters.Add("@SalesSourceID", orderRequest.SalesSourceID);
            orderParameters.Add("@HoldReason", orderRequest.HoldReason);
            orderParameters.Add("@Status", orderRequest.Status);
            orderParameters.Add("@BillToAddrLine1", orderRequest.BillToAddrLine1);
            orderParameters.Add("@BillToAddrLine2", orderRequest.BillToAddrLine2);
            orderParameters.Add("@BillToAddrLine3", orderRequest.BillToAddrLine3);
            orderParameters.Add("@BillToAddrLine4", orderRequest.BillToAddrLine4);
            orderParameters.Add("@BillToAddrLine5", orderRequest.BillToAddrLine5);
            orderParameters.Add("@BillToAddrName", orderRequest.BillToName);
            orderParameters.Add("@BillToCity", orderRequest.BillToCity);
            orderParameters.Add("@BillToCountryID", orderRequest.BillToCountryID);
            orderParameters.Add("@BillToPostalCode", orderRequest.BillToPostalCode);
            orderParameters.Add("@BillToStateID", orderRequest.BillToStateID);

            if (orderRequest.DfltWhseKey == 0)
            {
                orderParameters.Add("@DfltWhseKey", DBNull.Value);
            }
            else
            {
                orderParameters.Add("@DfltWhseKey", orderRequest.DfltWhseKey);
            }
            orderParameters.Add("@CreateUserID", orderRequest.LoginName);
            if (orderRequest.STaxSchdKey == 0)
            {
                orderParameters.Add("@STaxSchdKey", DBNull.Value);
            }
            else
            {
                orderParameters.Add("@STaxSchdKey", orderRequest.STaxSchdKey);
            }
            switch (orderRequest.CrHold)
            {
                case 1:
                    orderParameters.Add("@CrHold", 1);
                    break;
                case 0:
                    orderParameters.Add("@CrHold", 0);
                    break;
                default:
                    orderParameters.Add("@CrHold", DBNull.Value);
                    break;
            }
            orderParameters.Add("@CRMOpportunityID", orderRequest.CRMOpportunityID);
            if (orderRequest.DfltDelMethod == null)
            {
                orderParameters.Add("@isDropShip", 1);
            }
            else
            {
                orderParameters.Add("@isDropShip", orderRequest.DfltDelMethod);
            }

            orderParameters.Add("@FOB", orderRequest.FOB);
            orderParameters.Add("@FOBKey", orderRequest.FOBKey);
            if (string.IsNullOrEmpty(orderRequest.ShipToAddrID))
            {
                orderParameters.Add("@ShipToAddrID", DBNull.Value);
            }
            else
            {
                orderParameters.Add("@ShipToAddrID", orderRequest.ShipToAddrID);
            }

            orderParameters.Add("@_oSOKey", 0, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            orderParameters.Add("@_oTranID", "", System.Data.DbType.String, System.Data.ParameterDirection.Output);

            int SOKey = 0;
            string TranID = string.Empty;
            string Status = string.Empty;

            using (DbConnection connection = ConnectionFactory.GetOpenConnection("DefaultConnection"))
            {
                connection.Execute("spsoInsertStgOrderBasic_RKL", orderParameters, commandType: System.Data.CommandType.StoredProcedure);
                SOKey = orderParameters.Get<int>("@_oSOKey");
                TranID = orderParameters.Get<string>("@_oTranID");
            }

            OrderResponse response = new OrderResponse();
            
            if (string.IsNullOrEmpty(TranID))
            {
                Status = "Failure";
                response.Errors = GetOrderErrors(orderRequest.sessKey, orderRequest.LoginName);               
            }
            else
            {
                Status = "Success";
            }
            response.Status = Status;
            response.SOKey = SOKey;
            response.TranID = TranID;

            return Ok();
        }

        /// <summary>
        /// Searches Sales Orders based on a variety of criteria.
        /// </summary>
        /// <param name="searchOrder"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SearchOrders")]
        [ResponseType(typeof(List<SearchOrderResponse>))]
        public IHttpActionResult SearchOrders(SearchOrderRequest searchOrder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@compid", searchOrder.compid);
            parameters.Add("@customerName", searchOrder.customerName);
            parameters.Add("@custID", searchOrder.custID);
            parameters.Add("@CustPONo", searchOrder.CustPONo);
            parameters.Add("@CustPONoBCE", searchOrder.CustPONoBCE);
            parameters.Add("@CustPONo1", searchOrder.CustPONo1);
            parameters.Add("@CustPONo2", searchOrder.CustPONo2);
            parameters.Add("@salespersonID", searchOrder.salespersonID);
            parameters.Add("@salespersonname", searchOrder.salespersonname);
            parameters.Add("@orderDateFr", searchOrder.orderDateFr);
            parameters.Add("@orderDateTo", searchOrder.orderDateTo);
            parameters.Add("@tranNo", searchOrder.tranNo);
            parameters.Add("@tranId", searchOrder.tranID);
            parameters.Add("@includeDetails", searchOrder.includeDetails);
            parameters.Add("@headerStatus", searchOrder.headerStatus);
            parameters.Add("@lineStatus", searchOrder.lineStatus);
            parameters.Add("@backOrderOnly", searchOrder.backOrderOnly);
            parameters.Add("@SperKeyIn", searchOrder.SperKeyIn);
            parameters.Add("@CustKeyIn", searchOrder.CustKeyIn);
            parameters.Add("@FromAmt", searchOrder.FromAmt);
            parameters.Add("@ToAmt", searchOrder.ToAmt);
            parameters.Add("@CurrID", searchOrder.CurrID);
            parameters.Add("@ConfirmNo", searchOrder.ConfirmNo);
            parameters.Add("@UnshippedOnly", searchOrder.UnshippedOnly);
            parameters.Add("@TranType", searchOrder.TranType);

            if (searchOrder.PageIndex == 0)
                searchOrder.PageIndex = 1;
            if (searchOrder.PageSize == 0)
                searchOrder.PageSize = 999;
            parameters.Add("@PageIndex", searchOrder.PageIndex);
            parameters.Add("@PageSize", searchOrder.PageSize);
            parameters.Add("@_oResultSize", null, DbType.Int32, ParameterDirection.Output);

            SearchOrderResponse response = new SearchOrderResponse();
            response.orders = new List<SearchOrderLine>();
            response.TotalRecords = 0;

            try
            {
                using (DbConnection connection = ConnectionFactory.GetOpenConnection("DefaultConnection"))
                {
                    response.orders = connection.Query<SearchOrderLine>("spsoSearchOrder_RKL", parameters, commandType: CommandType.StoredProcedure).ToList();
                    response.TotalRecords = parameters.Get<int>("@_oResultSize");
                }
            }
            catch (Exception exception)
            {
                Common.LogError(searchOrder.LoginName, exception.Message, exception.StackTrace, "SOController.SearchOrders", "E");
                return BadRequest(exception.Message);
            }
            return Ok(response);
        }

        /// <summary>
        /// Returns Sales Order Details.
        /// </summary>
        /// <param name="detailsRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("OrderDetails")]
        [ResponseType(typeof(OrderDetailsResponse))]
        public IHttpActionResult OrderDetails(OrderDetailsRequest detailsRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            OrderDetailsResponse response = new OrderDetailsResponse();
            response.header = new OrderDetailsHeader();
            response.lines = new List<OrderDetailsLine>();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@compid", detailsRequest.compid);
            parameters.Add("@SOKey", detailsRequest.SOKey);

            try
            {
                response.header = sqlHandler.SQLWithRetrieveSingle<OrderDetailsHeader>("spsoGetOrderHeader_RKL", CommandType.StoredProcedure, parameters);
                response.lines = sqlHandler.SQLWithRetrieveList<OrderDetailsLine>("spsoGetOrderLines_RKL", CommandType.StoredProcedure, parameters);
            }
            catch (Exception exception)
            {
                Common.LogError(detailsRequest.LoginName, exception.Message, exception.StackTrace, "SOController.OrderDetails", "E");
                return BadRequest(exception.Message);
            }
            return Ok(response);
        }

        /// <summary>
        /// Looks up serial numbers by ItemID.
        /// </summary>
        /// <param name="lookupRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SerialNumberLookup")]
        [ResponseType(typeof(List<SerialNumberLookupResponse>))]
        public IHttpActionResult SerialNumberLookup(SerialNumberLookupRequest lookupRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            List<SerialNumberLookupResponse> response = new List<SerialNumberLookupResponse>();
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@CompID", lookupRequest.CompID);
            parameters.Add("@ItemID", lookupRequest.ItemID);
            parameters.Add("@SerialNo", lookupRequest.SerialNo);

            try
            {
                response = sqlHandler.SQLWithRetrieveList<SerialNumberLookupResponse>("spsoSerialNbrLookup_rkl", CommandType.StoredProcedure, parameters);
            }
            catch (Exception exception)
            {
                Common.LogError(lookupRequest.LoginName, exception.Message, exception.StackTrace, "SOController.SerialNumberLookup", "E");
                return BadRequest(exception.Message);
            }

            return Ok(response);
        }

        /// <summary>
        /// Searches Shipments with option for Paged Results
        /// </summary>
        /// <param name="shipRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SearchShipmentsWithPaging")]
        [ResponseType(typeof(SearchShipmentsResponse))]
        public IHttpActionResult SearchShipmentsWithPaging(SearchShipmentsRequest shipRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@CompID", shipRequest.CompID);
            parameters.Add("@CustID", shipRequest.CustID);
            parameters.Add("@TranID", shipRequest.TranID);
            parameters.Add("@SalesOrderID", shipRequest.SalesOrderID);
            parameters.Add("@CustPONo", shipRequest.CustPONo);
            parameters.Add("@CustPOBCE", shipRequest.CustPOBCE);
            parameters.Add("@CustPONoFr", shipRequest.CustPONoFr);
            parameters.Add("@CustPONoTo", shipRequest.CustPONoTo);
            parameters.Add("@FromTranDate", shipRequest.FromTranDate);
            parameters.Add("@ToTranDate", shipRequest.ToTranDate);
            parameters.Add("@SperKeyIn", shipRequest.SperKeyIn);
            parameters.Add("@CustKeyIn", shipRequest.CustKeyIn);
            parameters.Add("@AddrKey", shipRequest.AddrKey);
            parameters.Add("@ShipMethKey", shipRequest.ShipMethKey);
            parameters.Add("@ShipTrackNo", shipRequest.ShipTrackNo);
            parameters.Add("@InvcTranID", shipRequest.InvcTranID);
            parameters.Add("@PageIndex", shipRequest.PageIndex);
            parameters.Add("@PageSize", shipRequest.PageSize);
            parameters.Add("@_oResultSize", null, DbType.Int32, ParameterDirection.Output);

            SearchShipmentsResponse response = new SearchShipmentsResponse();
            response.shipments = new List<SearchShipment>();
            response.ResultSize = 0;

            try
            {
                using (DbConnection connection = ConnectionFactory.GetOpenConnection("DefaultConnection"))
                {
                    response.shipments = connection.Query<SearchShipment>("spsoGetShipmentsWithPaging_RKL", parameters, commandType: CommandType.StoredProcedure).ToList();
                    response.ResultSize = parameters.Get<int>("@_oResultSize");
                }
            }
            catch (Exception exception)
            {
                Common.LogError(shipRequest.LoginName, exception.Message, exception.StackTrace, "SOController.SearchShipmentsWithPaging", "E");
                return BadRequest(exception.Message);
            }

            return Ok(response);
        }

        /// <summary>
        /// Shows details about shipments
        /// </summary>
        /// <param name="detailsRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ShipmentDetails")]
        [ResponseType(typeof(ShipmentDetailsResponse))]
        public IHttpActionResult ShipmentDetails(ShipmentDetailsRequest detailsRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@CompID", detailsRequest.CompID);
            parameters.Add("@ShipKey", detailsRequest.ShipKey);

            ShipmentDetailsResponse response = new ShipmentDetailsResponse();
            response.header = new ShipmentDetailsHeader();
            response.lines = new List<ShipmentDetailsLine>();

            try
            {
                response.header = sqlHandler.SQLWithRetrieveSingle<ShipmentDetailsHeader>("spsoGetShipmentHeader_RKL", CommandType.StoredProcedure, parameters);
                response.lines = sqlHandler.SQLWithRetrieveList<ShipmentDetailsLine>("spsoGetShipmentDetails_RKL", CommandType.StoredProcedure, parameters);
            }
            catch (Exception exception)
            {
                Common.LogError(detailsRequest.LoginName, exception.Message, exception.StackTrace, "SOController.ShipmentDetails", "E");
                return BadRequest(exception.Message);
            }

            return Ok(response);
        }

        /// <summary>
        /// Get SO Options
        /// </summary>
        /// <param name="optionsRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetSOOptions")]
        [ResponseType(typeof(GetSOOptionsResponse))]
        public IHttpActionResult GetSOOptions(GetSOOptionsRequest optionsRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@CompID", optionsRequest.CompID);

            GetSOOptionsResponse response = new GetSOOptionsResponse();

            try
            {
                response = sqlHandler.SQLWithRetrieveSingle<GetSOOptionsResponse>("spsoGetSOOptions_rkl", CommandType.StoredProcedure, parameters);
            }
            catch (Exception exception)
            {
                Common.LogError(optionsRequest.LoginName, exception.Message, exception.StackTrace, "SOController.GetSOOptions", "E");
                return BadRequest(exception.Message);
            }

            return Ok(response);
        }

        /// <summary>
        /// Search Sales Orders
        /// </summary>
        /// <param name="searchRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SOSearch")]
        [ResponseType(typeof(List<SOSearchResponse>))]
        public IHttpActionResult SOSearch(SOSearchRequest searchRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@CompID", searchRequest.CompID);
            parameters.Add("@CustPONo", searchRequest.CustPONo);
            parameters.Add("@CustID", searchRequest.CustID);
            parameters.Add("@SperKeyIn", searchRequest.SperKeyIn);
            parameters.Add("@FromTranDate", searchRequest.FromTranDate);
            parameters.Add("@ToTranDate", searchRequest.ToTranDate);
            parameters.Add("@SalesProdLineKeyIn", searchRequest.SalesProdLineKeyIn);
            parameters.Add("@ItemClassKeyIn", searchRequest.ItemClassKeyIn);
            parameters.Add("@CustKeyIn", searchRequest.CustKeyIn);
            parameters.Add("@VendID", searchRequest.VendID);
            parameters.Add("@ItemID", searchRequest.ItemID);
            parameters.Add("@TranType", searchRequest.TranType);
            parameters.Add("@PageIndex", searchRequest.PageIndex);
            parameters.Add("@PageSize", searchRequest.PageSize);
            parameters.Add("@LoginName", searchRequest.LoginName);

            List<SOSearchResponse> response = new List<SOSearchResponse>();

            try
            {
                response = sqlHandler.SQLWithRetrieveList<SOSearchResponse>("spsoGetSalesOrders_RKL", CommandType.StoredProcedure, parameters);
            }
            catch (Exception exception)
            {
                Common.LogError(searchRequest.LoginName, exception.Message, exception.StackTrace, "SOController.SOSearch", "E");
                return BadRequest(exception.Message);
            }

            return Ok(response);
        }
        
        /// <summary>
        /// Get Sales Order Lines
        /// </summary>
        /// <param name="lineRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetSOLines")]
        [ResponseType(typeof(List<GetSOLinesResponse>))]
        public IHttpActionResult GetSOLines(SORequest lineRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@SOKey", lineRequest.SOKey);

            List<GetSOLinesResponse> response = new List<GetSOLinesResponse>();

            try
            {
                response = sqlHandler.SQLWithRetrieveList<GetSOLinesResponse>("spsoGetSOLines_RKL", CommandType.StoredProcedure, parameters);
            }
            catch (Exception exception)
            {
                Common.LogError(lineRequest.LoginName, exception.Message, exception.StackTrace, "SOController.GetSOLines", "E");
                return BadRequest(exception.Message);
            }
            return Ok(response);
        }

        /// <summary>
        /// Get shipments for invoices
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetShipmentsInvoices")]
        [ResponseType(typeof(List<GetShipmentsInvoicesResponse>))]
        public IHttpActionResult GetShipmentsInvoices(GetShipmentsInvoicesRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@CompID", request.CompID);
            parameters.Add("@CustID", request.CustID);
            parameters.Add("@CustKey", request.CustKey);
            parameters.Add("@ItemID", request.ItemID);
            parameters.Add("@ItemKey", request.ItemKey);
            parameters.Add("@FiscYear", request.FiscYear);
            parameters.Add("@IorS", request.IorS);

            List<GetShipmentsInvoicesResponse> response = new List<GetShipmentsInvoicesResponse>();

            try
            {
                response = sqlHandler.SQLWithRetrieveList<GetShipmentsInvoicesResponse>("spsoGetShipmentsInvoices_RKL", CommandType.StoredProcedure, parameters);
            }
            catch (Exception exception)
            {
                Common.LogError(request.LoginName, exception.Message, exception.StackTrace, "SOController.GetShipmentsInvoices", "E");
                return BadRequest(exception.Message);
            }
            return Ok(response);
        }

        /// <summary>
        /// Get Invoices for Sales Orders
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetInvoicesForSO")]
        [ResponseType(typeof(List<GetInvoicesForSOResponse>))]
        public IHttpActionResult GetInvoicesForSO(SORequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@SOKey", request.SOKey);
            parameters.Add("@compid", request.CompID);

            List<GetInvoicesForSOResponse> response = new List<GetInvoicesForSOResponse>();

            try
            {
                response = sqlHandler.SQLWithRetrieveList<GetInvoicesForSOResponse>("spsoGetInvoicesForSO_RKL", CommandType.StoredProcedure, parameters);
            }
            catch (Exception exception)
            {
                Common.LogError(request.LoginName, exception.Message, exception.StackTrace, "SOController.GetInvoicesForSO", "E");
                return BadRequest(exception.Message);
            }

            return Ok(response);
        }

        /// <summary>
        /// Get detailed Sales Order lines
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetSOLinesWithDetails")]
        [ResponseType(typeof(List<GetSOLinesWithDetailResponse>))]
        public IHttpActionResult GetSOLinesWithDetails(SORequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@SOKey", request.SOKey);

            List<GetSOLinesWithDetailResponse> response = new List<GetSOLinesWithDetailResponse>();

            try
            {
                response = sqlHandler.SQLWithRetrieveList<GetSOLinesWithDetailResponse>("spsoGetSOLinesWithDetail_rkl", CommandType.StoredProcedure, parameters);
            }
            catch (Exception exception)
            {
                Common.LogError(request.LoginName, exception.Message, exception.StackTrace, "SOController.GetSOLinesWithDetails", "E");
                return BadRequest(exception.Message);
            }
            return Ok(response);
        }

        /// <summary>
        /// Get Order history
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetOrderHistory")]
        [ResponseType(typeof(List<GetOrderHistoryResponse>))]
        public IHttpActionResult GetOrderHistory(GetOrderHistoryRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@CompanyID", request.CompID);
            parameters.Add("@ItemKey", request.ItemKey);
            parameters.Add("@CustKey", request.CustKey);

            List<GetOrderHistoryResponse> response = new List<GetOrderHistoryResponse>();

            try
            {
                response = sqlHandler.SQLWithRetrieveList<GetOrderHistoryResponse>("spsoGetOrderHistory_RKL", CommandType.StoredProcedure, parameters);
            }
            catch (Exception exception)
            {
                Common.LogError(request.LoginName, exception.Message, exception.StackTrace, "SOController.GetOrderHistory", "E");
                return BadRequest(exception.Message);
            }
            return Ok(response);
        }

        /// <summary>
        /// Return a shopping cart header for use with an external ordering system.
        /// </summary>
        /// <param name="headerRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetShoppingCartHeader")]
        [ResponseType(typeof(List<SOHeader>))]
        public IHttpActionResult GetShoppingCartHeader(SOHeaderRequest headerRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@CompID", headerRequest.CompID);
            if (headerRequest.SOKeyTemp == null)
            {
                parameters.Add("@SOKeyTemp", DBNull.Value);
            }
            else
            {
                parameters.Add("@SOKeyTemp", headerRequest.SOKeyTemp);
            }
            if (headerRequest.Status == null)
            {
                parameters.Add("@Status", DBNull.Value);
            }
            else
            {
                parameters.Add("@Status", headerRequest.Status);
            }
            parameters.Add("@PortalSessionID", headerRequest.PortalSessionID);
            parameters.Add("@LoginName", headerRequest.SearchLoginName);
            parameters.Add("@CustID", headerRequest.CustID);
            parameters.Add("@CustPONo", headerRequest.CustPONo);
            parameters.Add("@ImportRef", headerRequest.ImportRef);

            List<SOHeader> response = new List<SOHeader>();

            try
            {
                response = sqlHandler.SQLWithRetrieveList<SOHeader>("spsoGetSOHeaderWrk_rkl", System.Data.CommandType.StoredProcedure, parameters);
            }
            catch (Exception exception)
            {
                Common.LogError(headerRequest.LoginName, exception.Message, exception.StackTrace, "SOController.GetShoppingCartHeader", "E");
                return BadRequest(exception.Message);
            }

            return Ok(response);
        }

        /// <summary>
        /// Shipment Scheduler
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ShipmentScheduler")]
        [ResponseType(typeof(List<ShipmentScheduleResponse>))]
        public IHttpActionResult ShipmentScheduler(ShipmentScheduleRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Show_A_S_NS", request.Show_A_S_NS);
            parameters.Add("@WhseID", request.WhseID);
            parameters.Add("@Thru", request.Thru);
            parameters.Add("@From", request.From);

            List<ShipmentScheduleResponse> response = new List<ShipmentScheduleResponse>();

            try
            {
                response = sqlHandler.SQLWithRetrieveList<ShipmentScheduleResponse>("spsoShipmentSchedule_RKL", CommandType.StoredProcedure, parameters);
            }
            catch (Exception exception)
            {
                Common.LogError(request.LoginName, exception.Message, exception.StackTrace, "SOController.ShipmentScheduler", "E");
                return BadRequest(exception.Message);
            }
            return Ok(response);
        }

        /// <summary>
        /// Get Dates for Orders
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetOrderDates")]
        [ResponseType(typeof(GetOrderDatesResponse))]
        public IHttpActionResult GetOrderDates(GetOrderDatesRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@compid", request.CompID);
            parameters.Add("@custid", request.CustID);
            parameters.Add("@AddrKey", 0);
            parameters.Add("@AddrKeyIn", string.Empty);
            parameters.Add("@SperKeyIn", string.Empty);

            List<dynamic> dbReturn = new List<dynamic>();

            try
            {
                dbReturn = sqlHandler.SQLWithRetrieveList<dynamic>("sparGetCustOtherAddr_RKL", CommandType.StoredProcedure, parameters);
            }
            catch (Exception exception)
            {
                Common.LogError(request.LoginName, exception.Message, exception.StackTrace, "SOController.GetOrderDates", "E");
                return BadRequest(exception.Message);
            }
            int shipDays = 0;
            if (dbReturn.Where(x => x.CustAddrID == request.CustShipID).Count() > 0)
            {
                shipDays = (int)dbReturn[0].ShipDays;
            }

            //get portal default if ship days for cust addr is zero
            if (shipDays == 0)
            {
                var attributes = Common.GetPortalCustomAttributes(request.CompID, "ShipDays", request.LoginName);
                if (attributes.Where(x => x.AttribName == "ShipDays").Count() > 0)
                {
                    shipDays = Convert.ToInt32(attributes.Where(x => x.AttribName == "ShipDays").Select(x => x.AttribValue).FirstOrDefault());
                }
            }

            DateTime reqDate = AddDate(DateTime.Now, shipDays, true);
            if (request.LineReqDate != null)
            {
                int diffDays = ((TimeSpan)(request.LineReqDate - reqDate)).Days;
                if (diffDays != 0) reqDate = (DateTime)request.LineReqDate;
            }
            if (request.HdrReqDate != null)
            {
                int diffDays = ((TimeSpan)(request.HdrReqDate - reqDate)).Days;
                if (diffDays != 0) reqDate = (DateTime)request.HdrReqDate;
            }

            GetOrderDatesResponse response = new GetOrderDatesResponse()
            {
                RequestDate = reqDate,
                PromiseDate = reqDate,
                ShipDate = AddDate(reqDate, (shipDays * -1), true)
            };

            return Ok(response);
        }

        /// <summary>
        /// Get Sales Source IDs
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetSalesSourceIDs")]
        [ResponseType(typeof(List<GetSalesSourceIDsResponse>))]
        public IHttpActionResult GetSalesSourceIDs(GetSOOptionsRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@CompanyID", request.CompID);

            List<GetSalesSourceIDsResponse> response = new List<GetSalesSourceIDsResponse>();

            try
            {
                response = sqlHandler.SQLWithRetrieveList<GetSalesSourceIDsResponse>("spsoGetSalesSourceIDs_RKL", CommandType.StoredProcedure, parameters);
            }
            catch (Exception exception)
            {
                Common.LogError(request.LoginName, exception.Message, exception.StackTrace, "SOController.GetSalesSourceIDs", "E");
                return BadRequest(exception.Message);
            }
            return Ok(response);
        }

        /// <summary>
        /// Run Sales Analysis
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SalesAnalysis")]
        [ResponseType(typeof(List<SalesAnalysisResponse>))]
        public IHttpActionResult SalesAnalysis(SalesAnalysisRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@CompID", request.CompID);
            parameters.Add("@CustKeyIn", request.CustKeyIn);
            parameters.Add("@SperKeyIn", request.SperKeyIn);
            parameters.Add("@DateFr", request.DateFr);
            parameters.Add("@DateTo", request.DateTo);

            List<SalesAnalysisResponse> response = new List<SalesAnalysisResponse>();

            try
            {
                response = sqlHandler.SQLWithRetrieveList<SalesAnalysisResponse>("spsoSalesAnalysis_rkl", CommandType.StoredProcedure, parameters);
            }
            catch (Exception exception)
            {
                Common.LogError(request.LoginName, exception.Message, exception.StackTrace, "SOController.SalesAnalysis", "E");
                return BadRequest(exception.Message);
            }

            return Ok(response);
        }

        /// <summary>
        /// Get shipments for RMAs
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetShipmentForRMA")]
        [ResponseType(typeof(List<GetShipmentForRMAResponse>))]
        public IHttpActionResult GetShipmentForRMA(GetShipmentForRMARequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@CompID", request.CompID);
            parameters.Add("@CustID", request.CustID);
            parameters.Add("@ShipToAddrID", request.ShipToAddrID);
            parameters.Add("@BillToAddrID", request.BillToAddrID);
            parameters.Add("@SOTranNo", request.SOTranNo);
            parameters.Add("@SOLineNo", request.SOLineNo);
            parameters.Add("@ItemID", request.ItemID);
            parameters.Add("@TrackNo", request.TrackNo);
            parameters.Add("@SHTranNo", request.SHTranNo);

            List<GetShipmentForRMAResponse> response = new List<GetShipmentForRMAResponse>();

            try
            {
                response = sqlHandler.SQLWithRetrieveList<GetShipmentForRMAResponse>("spsoGetShipmentRMA_RKL", CommandType.StoredProcedure, parameters);
            }
            catch (Exception exception)
            {
                Common.LogError(request.LoginName, exception.Message, exception.StackTrace, "SOController.GetShipmentForRMA", "E");
                return BadRequest(exception.Message);
            }
            return Ok(response);
        }

        /// <summary>
        /// Get shipment serial numbers
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetShipmentSerialNbrs")]
        [ResponseType(typeof(List<GetShipmentSerialNbrsResponse>))]
        public IHttpActionResult GetShipmentSerialNbrs(GetShipmentSerialNbrsRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@CompID", request.CompID);
            parameters.Add("@ShipLineKey", request.ShipLineKey);
            parameters.Add("@ShipLineDistKey", request.ShipLineDistKey);
            parameters.Add("@ShipmentNbr", request.ShipmentNbr);

            List<GetShipmentSerialNbrsResponse> response = new List<GetShipmentSerialNbrsResponse>();

            try
            {
                response = sqlHandler.SQLWithRetrieveList<GetShipmentSerialNbrsResponse>("spsoGetShipmentSerialNbrs_RKL", CommandType.StoredProcedure, parameters);
            }
            catch (Exception exception)
            {
                Common.LogError(request.LoginName, exception.Message, exception.StackTrace, "SOController.GetShipmentSerialNbrs", "E");
                return BadRequest(exception.Message);
            }
            return Ok(response);
        }

        /// <summary>
        /// Save RMA to 500
        /// </summary>
        /// <remarks>Note that POLineNo in RMALineDetails must have same POLineNo in RMASerialNbrs to save proposed serial numbers for this line.</remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveRMA")]
        [SwaggerResponse(HttpStatusCode.OK, type: typeof(SaveRMAResponse))]
        [SwaggerResponse(HttpStatusCode.BadRequest, type: typeof(RMAErrors))]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        public IHttpActionResult SaveRMA(SaveRMARequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            List<RMAErrors> errors = ValidateRMA(ref request);
            if (errors.Count() > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var error in errors)
                {
                    sb.Append(error.POLineNo.ToString() + ": " + error.Error + "/n/r");
                }
                return BadRequest(sb.ToString());
            }

            DynamicParameters rmaParameters = new DynamicParameters();
            rmaParameters.Add("@CompID", request.CompID);
            rmaParameters.Add("@CustID", request.CustID.Replace("'", "''"));
            rmaParameters.Add("@BillToCustAddrID", request.BillToCustAddrID.Replace("'", "''"));
            rmaParameters.Add("@ShipToCustAddrID", request.ShipToCustAddrID.Replace("'", "''"));
            rmaParameters.Add("@CntctKey", request.CntctKey);
            rmaParameters.Add("@TranCmnt", request.TranCmnt.Replace("'", "''"));
            rmaParameters.Add("@CurrID", request.CurrID);
            rmaParameters.Add("@FreightAmt", request.FreightAmt);
            rmaParameters.Add("@RestockAmt", request.RestockAmount);
            rmaParameters.Add("@ReturnAmt", request.ReturnAmt);
            rmaParameters.Add("@STaxAmt", request.STaxAmt);
            rmaParameters.Add("@STaxTranKey", request.STaxTranKey);
            rmaParameters.Add("@TradeDiscAmt", request.TradeDiscAmt);
            rmaParameters.Add("@TranAmt", request.TranAmt);
            rmaParameters.Add("@UserFld1", request.UserFld1.Replace("'", "''"));
            rmaParameters.Add("@UserFld2", request.UserFld2.Replace("'", "''"));
            rmaParameters.Add("@UserFld3", request.UserFld3.Replace("'", "''"));
            rmaParameters.Add("@UserFld4", request.UserFld4.Replace("'", "''"));
            rmaParameters.Add("@CreateUserID", request.CreateUserID);
            rmaParameters.Add("@RcvgWhseKey", request.RcvgWhseKey);

            SaveRMAResponse response = new SaveRMAResponse();
            response.rmaDetails = new RMADetails();

            try
            {
                response.rmaDetails = sqlHandler.SQLWithRetrieveSingle<RMADetails>("spsoSubmitRMA_RKL", CommandType.StoredProcedure, rmaParameters);
            }
            catch (Exception exception)
            {
                Common.LogError(request.LoginName, exception.Message, exception.StackTrace, "SOController.SaveRMA", "E");
                return BadRequest(exception.Message);
            }

            response.rmaLineDetails = new List<RMALineDetails>();
            //create line items
            foreach (var line in request.lines)
            {
                DynamicParameters lineParams = new DynamicParameters();
                lineParams.Add("@RMAKey", response.rmaDetails.RMAKey);
                lineParams.Add("@Description", line.Description);
                lineParams.Add("@ExtAmt", line.ExtAmt);
                lineParams.Add("@ExtCmnt", line.ExtCmnt);
                lineParams.Add("@FreightAmt", line.FreightAmt);
                lineParams.Add("@ItemKey", line.ItemKey);
                lineParams.Add("@OrigShipLineKey", line.OrigShipLineKey);
                lineParams.Add("@QtyAuthForRtrn", line.QtyAuthForRtrn);
                lineParams.Add("@RcvgWhseKey", line.RcvgWhseKey);
                lineParams.Add("@ReasonCodeKey", line.ReasonCodeKey);
                lineParams.Add("@RestockAmt", line.RestockAmt);
                lineParams.Add("@RtrnType", line.RtrnType);
                lineParams.Add("@ShipMethKey", line.ShipMethKey);
                lineParams.Add("@SOLineDistKey", line.SOLineDistKey);
                lineParams.Add("@STaxClassKey", line.STaxClassKey);
                lineParams.Add("@STaxTranKey", line.STaxTranKey);
                lineParams.Add("@TradeDiscAmnt", line.TradeDiscAmt);
                lineParams.Add("@UnitMeasKey", line.UnitMeasKey);
                lineParams.Add("@UnitPrice", line.UnitPrice);
                lineParams.Add("@UnitPriceOvrd", line.UnitPriceOvrd);
                lineParams.Add("@UserFld1", line.UserFld1);
                lineParams.Add("@UserFld2", line.UserFld2);
                lineParams.Add("@UserFld3", line.UserFld3);
                lineParams.Add("@UserFld4", line.UserFld4);

                RMALineDetails lineDetail = new RMALineDetails();

                try
                {
                    lineDetail = sqlHandler.SQLWithRetrieveSingle<RMALineDetails>("spsoSubmitRMALine_RKL", CommandType.StoredProcedure, lineParams);
                }
                catch (Exception exception)
                {
                    Common.LogError(request.LoginName, exception.Message, exception.StackTrace, "SOController.SaveRMA", "E");
                    return BadRequest(exception.Message);
                }

                response.rmaLineDetails.Add(lineDetail);

                if (request.serialNbrs.Where(x => x.POLineNo == line.POLineNo).Any())
                {
                    string serialNumber = request.serialNbrs.Where(x => x.POLineNo == line.POLineNo).Select(x => x.SerialNbr).FirstOrDefault();
                    DynamicParameters serialParameters = new DynamicParameters();
                    serialParameters.Add("@RMALineKey", lineDetail.RMALineKey);
                    serialParameters.Add("@ProposedSerialNo", serialNumber);

                    sqlHandler.SQLExecuteWithoutReturn("spsoSubmitRMALineDist_RKL", CommandType.StoredProcedure, serialParameters);
                }
            }

            return Ok(response);
        }

        /// <summary>
        /// Search RMAs
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SearchRMA")]
        [ResponseType(typeof(List<SearchRMAResponse>))]
        public IHttpActionResult SearchRMA(SearchRMARequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@CompID", request.CompID);
            parameters.Add("@RMAKey", request.RMAKey);
            parameters.Add("@TranNo", request.TranNo);
            parameters.Add("@TranID", request.TranID);
            parameters.Add("@CustID", request.CustID);
            parameters.Add("@CustKey", request.CustKey);
            parameters.Add("@CreateDateFr", request.CreateDateFr);
            parameters.Add("@CreateDateTo", request.CreateDateTo);
            parameters.Add("@ExpDateFr", request.ExpDateFr);
            parameters.Add("@ExpDateTo", request.ExpDateTo);
            parameters.Add("@SperID", request.SperID);
            parameters.Add("@SperKeyIn", request.SperKeyIn);

            List<SearchRMAResponse> response = new List<SearchRMAResponse>();

            try
            {
                response = sqlHandler.SQLWithRetrieveList<SearchRMAResponse>("spsoSearchRMA_RKL", CommandType.StoredProcedure, parameters);
            }
            catch (Exception exception)
            {
                Common.LogError(request.LoginName, exception.Message, exception.StackTrace, "SOController.SearchRMA", "E");
                return BadRequest(exception.Message);
            }

            return Ok(response);
        }

        [HttpPost]
        [Route("RMADetails")]
        [ResponseType(typeof(RMADetailsResponse))]
        public IHttpActionResult RMADetails(RMADetailsRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@CompID", request.CompID);
            parameters.Add("@RMAKey", request.RMAKey);
            parameters.Add("@TranNo", string.Empty);
            parameters.Add("@CustID", string.Empty);
            parameters.Add("@CustKey", 0);
            parameters.Add("@CreateDateFr", string.Empty);
            parameters.Add("@CreateDateTo", string.Empty);
            parameters.Add("@ExpDateFr", string.Empty);
            parameters.Add("@ExpDateTo", string.Empty);
            parameters.Add("@SperID", string.Empty);
            parameters.Add("SperKeyIn", string.Empty);

            DynamicParameters lineParameters = new DynamicParameters();
            lineParameters.Add("@CompID", request.CompID);
            lineParameters.Add("@RMAKey", request.RMAKey);

            DynamicParameters distParameters = new DynamicParameters();
            distParameters.Add("@RMAKey", request.RMAKey);
            distParameters.Add("@RMALineKey", 0);

            RMADetailsResponse response = new RMADetailsResponse();
            response.header = new SearchRMAResponse();
            response.lines = new List<RMADetailsLine>();
            response.lineDist = new List<RMADetailsLineDist>();

            try
            {
                response.header = sqlHandler.SQLWithRetrieveSingle<SearchRMAResponse>("spsoSearchRMA_RKL", CommandType.StoredProcedure, parameters);
                response.lines = sqlHandler.SQLWithRetrieveList<RMADetailsLine>("spsoGetRMALines_RKL", CommandType.StoredProcedure, lineParameters);
                response.lineDist = sqlHandler.SQLWithRetrieveList<RMADetailsLineDist>("spsoGetRMALineDist_RKL", CommandType.StoredProcedure, distParameters);
            }
            catch (Exception exception)
            {
                Common.LogError(request.LoginName, exception.Message, exception.StackTrace, "SOController.RMADetails", "E");
                return BadRequest(exception.Message);
            }

            return Ok(response);
        }

        /// <summary>
        /// Save SO Web Header is the shopping cart header save function provided by EBS Services.
        /// </summary>
        /// <param name="cartRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveSOWebHdr")]
        [ResponseType(typeof(SOHeader))]
        public IHttpActionResult SaveSOWebHdr(SOHeader request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@CompID", request.CompID);
            parameters.Add("@SOKeyTemp", request.SOKeyTemp);
            parameters.Add("@PortalSessionID", request.PortalSessionID);
            parameters.Add("@OrderInitDate", request.OrderInitDate);
            parameters.Add("@OrderComplDate", request.OrderComplDate);
            parameters.Add("@Status", request.Status);
            parameters.Add("@OrderType", request.OrderType);
            parameters.Add("@CustID", request.CustID.Replace("'", "''"));
            parameters.Add("@CustPO", request.CustPO.Replace("'", "''"));
            parameters.Add("@RequestDate", request.RequestDate);
            parameters.Add("@ExpDate", request.ExpDate);
            parameters.Add("@ShipDate", request.ShipDate);
            parameters.Add("@PromiseDate", request.PromiseDate);
            parameters.Add("@ShipMethID", request.ShipMethID.Replace("'", "''"));
            parameters.Add("@OverrideShipAddr", request.OverrideShipAddr);
            parameters.Add("@isDropShip", request.isDropShip);
            parameters.Add("@AddrKey", request.AddrKey);
            parameters.Add("@ShipFromAddrName", request.ShipFromAddrName);
            parameters.Add("@ShipFromAddrLine1", request.ShipFromAddrLine1);
            parameters.Add("@ShipFromAddrLine2", request.ShipFromAddrLine2);
            parameters.Add("@ShipFromAddrLine3", request.ShipFromAddrLine3);
            parameters.Add("@ShipFromAddrLine4", request.ShipFromAddrLine4);
            parameters.Add("@ShipFromAddrLine5", request.ShipFromAddrLine5);
            parameters.Add("@ShipFromCity", request.ShipFromCity);
            parameters.Add("@ShipFromStateID", request.ShipFromStateID);
            parameters.Add("@ShipFromPostalCode", request.ShipFromPostalCode);
            parameters.Add("@ShipFromCountryID", request.ShipFromCountryID);
            parameters.Add("@ShipToAddrName", request.ShipToAddrName);
            parameters.Add("@ShipToAddrLine1", request.ShipToAddrLine1);
            parameters.Add("@ShipToAddrLine2", request.ShipToAddrLine2);
            parameters.Add("@ShipToAddrLine3", request.ShipToAddrLine3);
            parameters.Add("@ShipToAddrLine4", request.ShipToAddrLine4);
            parameters.Add("@ShipToAddrLine5", request.ShipToAddrLine5);
            parameters.Add("@ShipToCity", request.ShipToCity);
            parameters.Add("@ShipToStateID", request.ShipToStateID);
            parameters.Add("@ShipToPostalCode", request.ShipToPostalCode);
            parameters.Add("@ShipToCountryID", request.ShipToCountryID);
            parameters.Add("@ClosestWhse", request.ClosestWhse);
            parameters.Add("@ShipDays", request.ShipDays);
            parameters.Add("@SalespersonID", request.SalesPersonID.Replace("'", "''"));
            parameters.Add("@Currency", request.Currency);
            parameters.Add("@Comment", request.Comment.Replace("'", "''"));
            parameters.Add("@UserFld1", request.UserFld1.Replace("'", "''"));
            parameters.Add("@UserFld2", request.UserFld2.Replace("'", "''"));
            parameters.Add("@UserFld3", request.UserFld3.Replace("'", "''"));
            parameters.Add("@UserFld4", request.UserFld4.Replace("'", "''"));
            parameters.Add("@OrderID", request.OrderID.Replace("'", "''"));
            parameters.Add("@TotalLines", request.TotalLines);
            parameters.Add("@TotalExtdAmt", request.TotalExtdAmt);
            parameters.Add("@TotalTaxable", request.TotalTaxable);
            parameters.Add("@TotalExemption", request.TotalExemption);
            parameters.Add("@TotalTax", request.TotalTax);
            parameters.Add("@PmtTermsID", request.PmtTermsID.Replace("'", "''"));
            parameters.Add("@STaxSchdKey", request.STaxSchdKey);
            parameters.Add("@UseCC", request.UseCC);
            parameters.Add("@Hold", request.Hold);
            parameters.Add("@SorD", request.SorD);
            parameters.Add("@TradeDiscAmt", request.TradeDiscAmt);
            parameters.Add("@TradeDiscPct", request.TradeDiscPct);
            parameters.Add("@FreightAmt", request.FreightAmt);
            parameters.Add("@BillToAddrKey", request.BillToAddrKey);
            parameters.Add("@ImportRef", request.ImportRef);
            parameters.Add("@ShipStatus", request.ShipStatus);
            parameters.Add("@SalesSourceID", request.SalesSourceID);
            parameters.Add("@FOB", request.DfltFOBID);
            parameters.Add("@FOBKey", request.DfltFOBKey);

            try
            {
                var response = sqlHandler.SQLWithRetrieveSingle<int>("spsoSaveSOHeaderWrk_RKL", CommandType.StoredProcedure, parameters);
                request.SOKeyTemp = response;
            }
            catch (Exception exception)
            {
                Common.LogError(request.LoginName, exception.Message, exception.StackTrace, "SOController.SaveSOWebdr", "E");
                return BadRequest(exception.Message);
            }

            return Ok(request);
        }

        /// <summary>
        /// Update SO Header Work Status
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveSOWebHdrStatus")]
        [SwaggerResponse(HttpStatusCode.Created)]
        [SwaggerResponse(HttpStatusCode.NoContent)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        public IHttpActionResult SaveSOWebHdrStatus(SaveSOWebHdrStatusRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@SOKeyTemp", request.SOKeyTemp);
            parameters.Add("@LoginName", request.LoginName);
            parameters.Add("@OrderComplDate", request.OrderComplDate);
            parameters.Add("@Status", request.Status);
            parameters.Add("@SOKey", request.SOKey);
            parameters.Add("@CustID", request.CustID);

            int response = 0;

            try
            {
                using (DbConnection connection = ConnectionFactory.GetOpenConnection("DefaultConnection"))
                {
                    response = connection.Execute("spsoUpdateSOHeaderWrkStatus_RKL", parameters, commandType: CommandType.StoredProcedure);
                    if (response > 0)
                    {
                        return Created("Success", request);
                    }
                    else
                    {
                        return StatusCode(HttpStatusCode.NoContent);
                    }
                }
            }
            catch (Exception exception)
            {
                Common.LogError(request.LoginName, exception.Message, exception.StackTrace, "SOController.SaveSOWebHdrStatus", "E");
                return BadRequest(exception.Message);
            }
            
        }

        /// <summary>
        /// Save SO Web Blanket
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveSOWebBlanket")]
        [SwaggerResponse(HttpStatusCode.Created)]
        [SwaggerResponse(HttpStatusCode.NoContent)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        public IHttpActionResult SaveSOWebBlanket(SaveSOBlanketWrkRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@SOKeyTemp", request.SOKeyTemp);
            parameters.Add("@MaxAmountToGen", request.MaxAmountToGen);
            parameters.Add("@MaxSOToGen", request.MaxSOToGen);
            parameters.Add("@Contract", request.Contract);
            parameters.Add("@ContractRef", request.ContractRef);
            parameters.Add("@CurrExchRateMeth", request.CurrExchRateMeth);
            parameters.Add("@StartDate", request.StartDate);
            parameters.Add("@StopDate", request.StopDate);
            parameters.Add("@ProcCycleKey", request.ProcCycleKey);
            parameters.Add("@SorD", request.SorD);

            int response = 0;

            try
            {
                using (DbConnection connection = ConnectionFactory.GetOpenConnection("DefaultConnection"))
                {
                    response = connection.Execute("spsoSaveSOBlanketWrk_RKL", parameters, commandType: CommandType.StoredProcedure);
                    if (response > 0)
                    {
                        return Created("Success", request);
                    }
                    else
                    {
                        return StatusCode(HttpStatusCode.NoContent);
                    }
                }
            }
            catch (Exception exception)
            {
                Common.LogError(request.LoginName, exception.Message, exception.StackTrace, "SOController.SaveSOWebBlanket", "E");
                return BadRequest(exception.Message);
            }
        }

        /// <summary>
        /// Save individual SO Line as part of cart
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveSOWebLine")]
        [SwaggerResponse(HttpStatusCode.Created, Type= typeof(SOLine))]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        public IHttpActionResult SaveSOWebLine(SOLine request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@SOLineKeyTemp", request.SOLineKeyTemp);
            parameters.Add("@SOKeyTemp", request.SOKeyTemp);
            parameters.Add("@ItemID", request.ItemID.Replace("'", "''"));
            parameters.Add("@isDropShip", request.isDropShip);
            parameters.Add("@Price", request.Price);
            parameters.Add("@PriceOverride", request.PriceOverride);
            parameters.Add("@Quantity", request.Quantity);
            parameters.Add("@Description", request.Description.Replace("'", "''"));
            parameters.Add("@POLineNo", request.POLineNo);
            parameters.Add("@UnitMeasID", request.UnitMeasID);
            parameters.Add("@RequestDate", request.RequestDate);
            parameters.Add("@ExpDate", request.ExpDate);
            parameters.Add("@ShipDate", request.ShipDate);
            parameters.Add("@PromiseDate", request.PromiseDate);
            parameters.Add("@TaxClassID", request.TaxClassID);
            parameters.Add("@TaxClassKey", request.TaxClassKey);
            parameters.Add("@UserFld1", request.UserFld1);
            parameters.Add("@UserFld2", request.UserFld2);
            parameters.Add("@CustItem", request.CustItem);
            parameters.Add("@UMKey", request.UMKey);
            parameters.Add("@ExtdAmt", request.ExtdAmt);
            parameters.Add("@ItemKey", request.ItemKey);
            parameters.Add("@Notes", request.Notes.Replace("'", "''"));
            parameters.Add("@BackOrder", request.BackOrder);
            parameters.Add("@TaxAmt", request.TaxAmt);
            parameters.Add("@UnitCost", request.UnitCost);
            parameters.Add("@GMAmt", request.GMAmt);
            parameters.Add("@GMPct", request.GMPct);
            parameters.Add("@AllowDecimalQty", request.AllowDecimalQty);
            parameters.Add("@MinSaleQty", request.MinSaleQty);
            parameters.Add("@SaleMultiple", request.SaleMutiple);
            parameters.Add("@SalesPromoID", request.SalesPromoID);
            parameters.Add("@SalesPromoKey", request.SalesPromoKey);
            parameters.Add("@SorD", request.SorD);
            parameters.Add("@LineShipStatus", request.LineShipStatus);
            parameters.Add("@AcctRefKey", request.AcctRefKey);
            parameters.Add("@WhseKey", request.WhseKey);
            parameters.Add("@ShipMethKey", request.ShipMethKey);
            parameters.Add("@ShipMethID", request.ShipMethIDLn);
            parameters.Add("@FreightAmt", request.FreightAmt);
            parameters.Add("@TradeDiscAmt", request.TradeDiscAmt);
            parameters.Add("@ePortalPromoKey", request.ePortalPromoKey);

            int response = 0;

            try
            {
                response = sqlHandler.SQLWithRetrieveSingle<int>("spsoSaveSOLineWrk_rkl", CommandType.StoredProcedure, parameters);
            }
            catch (Exception exception)
            {
                Common.LogError(request.LoginName, exception.Message, exception.StackTrace, "SOController.SaveSOWebLine", "E");
                return BadRequest(exception.Message);
            }
            request.SOLineKeyTemp = response;

            return Created("Success", request);
        }

        /// <summary>
        /// Save web order ship to address.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveSOWebAddr")]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.Created, Type=typeof(SOAddr))]
        public IHttpActionResult SaveSOWebAddr(SOAddr request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@SOAddrKeyTemp", request.SOAddrKeyTemp);
            parameters.Add("@SOKeyTemp", request.SOKeyTemp);
            parameters.Add("@AddrKey", request.AddrKey);
            parameters.Add("@CustAddrID", request.CustAddrID.Replace("'", "''"));
            parameters.Add("@ShipToAddrName", request.ShipToAddrName.Replace("'", "''"));
            parameters.Add("@ShipToAddrLine1", request.ShipToAddrLine1.Replace("'", "''"));
            parameters.Add("@ShipToAddrLine2", request.ShipToAddrLine2.Replace("'", "''"));
            parameters.Add("@ShipToAddrLine3", request.ShipToAddrLine3.Replace("'", "''"));
            parameters.Add("@ShipToAddrLine4", request.ShipToAddrLine4.Replace("'", "''"));
            parameters.Add("@ShipToAddrLine5", request.ShipToAddrLine5.Replace("'", "''"));
            parameters.Add("@ShipToCity", request.ShipToCity);
            parameters.Add("@ShipToStateID", request.ShipToStateID);
            parameters.Add("@ShipToPostalCode", request.ShipToPostalCode);
            parameters.Add("@ShipToCountryID", request.ShipToCountryID);
            parameters.Add("@SorD", request.SorD);

            int response = 0;

            try
            {
                response = sqlHandler.SQLWithRetrieveSingle<int>("spsoSaveSOAddrWrk_rkl", CommandType.StoredProcedure, parameters);
            }
            catch (Exception exception)
            {
                Common.LogError(request.LoginName, exception.Message, exception.StackTrace, "SOController.SaveSOWebAddr", "E");
                return BadRequest(exception.Message);
            }
            request.SOAddrKeyTemp = response;
            return Created("Success", request);
        }

        /// <summary>
        /// Save web order attribute.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveSOWebAttr")]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.NoContent)]
        [SwaggerResponse(HttpStatusCode.Created)]
        public IHttpActionResult SaveSOWebAttr(SOAttr request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@AttribType", request.AttribType);
            parameters.Add("@AttribRefKey", request.AttribRefKey);
            parameters.Add("@AttribCharValue", request.AttribCharValue);
            parameters.Add("@AttribDecValue", request.AttribDecValue);
            parameters.Add("@AttribIntValue", request.AttribIntValue);
            parameters.Add("@AttribKey", request.AttribKey);
            parameters.Add("@SorD", request.SorD);

            try
            {
                using (DbConnection connection = ConnectionFactory.GetOpenConnection("DefaultConnection"))
                {
                    var response = connection.Execute("spsoSaveSOAttrWrk_RKL", parameters, commandType: CommandType.StoredProcedure);
                    if (response > 0)
                    {
                        return Created("Success", request);
                    }
                    else
                    {
                        return StatusCode(HttpStatusCode.NoContent);
                    }
                }
            }
            catch (Exception exception)
            {
                Common.LogError(request.LoginName, exception.Message, exception.StackTrace, "SOController.SaveSOWebAttr", "E");
                return BadRequest(exception.Message);
            }
        }

        /// <summary>
        /// Saves or updates SO web-based or work in progress blanket order to Sage 500.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.Created)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [Route("SaveSOBlanket")]
        public IHttpActionResult SaveSOBlanket(SaveSOBlanketRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@SOKey", request.SOKey);
            parameters.Add("@SOKeyTemp", request.SOKeyTemp);

            try
            {
                using (DbConnection connection = ConnectionFactory.GetOpenConnection("DefaultConnection"))
                {
                    var response = connection.Execute("spsoSaveSOBlanketInfo_RKL", parameters, commandType: CommandType.StoredProcedure);
                    if (response > 0)
                    {
                        return Created("Success", request);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }
            catch (Exception exception)
            {
                Common.LogError(request.LoginName, exception.Message, exception.StackTrace, "SOController.SaveSOBlanket", "E");
                return BadRequest(exception.Message);
            }

        }

        /// <summary>
        /// Check SO Logs.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CheckSOLog")]
        [ResponseType(typeof(Dictionary<string, int>))]
        public IHttpActionResult CheckSOLog(CheckSOLogRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@CompanyID", request.CompID);
            parameters.Add("@TranType", request.TranType);
            parameters.Add("@TranNo", request.TranNo);

            int count = 0;

            try
            {
                using (DbConnection connection = ConnectionFactory.GetOpenConnection("DefaultConnection"))
                {
                    count = sqlHandler.SQLWithRetrieveSingle<int>("spsoCheckSOLog_RKL", CommandType.StoredProcedure, parameters);
                }
            }
            catch (Exception exception)
            {
                Common.LogError(request.LoginName, exception.Message, exception.StackTrace, "SOController.CheckSOLog", "E");
                return BadRequest(exception.Message);
            }

            Dictionary<string, int> response = new Dictionary<string, int>();
            response.Add("Count", count);

            return Ok(response);
        }

        /// <summary>
        /// Creates a Sales Order from an existing quote.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateSOFromQuote")]
        [ResponseType(typeof(CreateSOFromQuoteOrBlanketResponse))]
        public IHttpActionResult CreateSOFromQuote(CreateSOFromQuoteRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (request.TranDate == null)
            {
                request.TranDate = DateTime.Now;
            }

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@CompanyID", request.CompanyID);
            parameters.Add("@SOKeyIn", request.SOKeyIn);
            parameters.Add("@TranDate", request.TranDate);
            parameters.Add("@TranNo", request.TranNo);
            parameters.Add("@User", request.User);
            parameters.Add("@SessionID", request.SessionID);

            CreateSOFromQuoteOrBlanketResponse response = new CreateSOFromQuoteOrBlanketResponse();

            try
            {
                response = sqlHandler.SQLWithRetrieveSingle<CreateSOFromQuoteOrBlanketResponse>("spsoCreateSOFromQuote_RKL", CommandType.StoredProcedure, parameters);
            }
            catch (Exception exception)
            {
                Common.LogError(request.LoginName, exception.Message, exception.StackTrace, "SOController.CreateSOFromQuote", "E");
                return BadRequest(exception.Message);
            }
            return Ok(response);
        }

        /// <summary>
        /// Creates a Sales Order from a blanket order.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateSOFromBlanket")]
        [ResponseType(typeof(CreateSOFromQuoteOrBlanketResponse))]
        public IHttpActionResult CreateSOFromBlanket(CreateSOFromBlanketRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (request.TranDate == null)
                request.TranDate = DateTime.Now;

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@CompanyID", request.CompanyID);
            parameters.Add("@SOKeyIn", request.SOKeyIn);
            parameters.Add("@TranDate", request.TranDate);
            parameters.Add("@TranNo", request.TranNo);
            parameters.Add("@User", request.User);
            parameters.Add("@SessionID", request.SessionID);
            parameters.Add("@SOKeyTempIn", request.SOKeyTempIn);

            try
            {
                using (DbConnection connection = ConnectionFactory.GetOpenConnection("DefaultConnection"))
                {
                    connection.Execute("spsoCreateSOFromBlnkt_RKL", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception exception)
            {
                Common.LogError(request.LoginName, exception.Message, exception.StackTrace, "SOController.CreateSOFromBlanket", "E");
                return BadRequest(exception.Message);
            }

            CreateSOFromQuoteOrBlanketResponse response = new CreateSOFromQuoteOrBlanketResponse();

            parameters = new DynamicParameters();
            parameters.Add("@CompanyID", request.CompanyID);
            parameters.Add("@OrigSOKey", request.SOKeyIn);
            parameters.Add("@BlnktSOKey", 0);

            try
            {
                var res = sqlHandler.SQLWithRetrieveSingle<dynamic>("spsoGetRecurOrderHistory_RKL", CommandType.StoredProcedure, parameters);
                response.RetVal = 0;
                response.SOKey = (int)res.BlnktSOKey;
                response.TranID = (string)res.BlnktTranID;
            }
            catch (Exception exception)
            {
                Common.LogError(request.LoginName, exception.Message, exception.StackTrace, "SOController.CreateSOFromBlanket", "E");
                return BadRequest(exception.Message);
            }
            return Ok(response);
        }

        /// <summary>
        /// Get Recurring Blanket Orders
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetRecurBlnktOrders")]
        [ResponseType(typeof(GetRecurBlnktOrdersResponse))]
        public IHttpActionResult GetRecurBlnktOrders(GetRecurBlnktOrdersRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@CompanyID", request.CompID);
            parameters.Add("@WhseKey", request.WhseKey);
            parameters.Add("@TranID", request.TranID);
            parameters.Add("@Priority", request.Priority);
            parameters.Add("@CustPONo", request.CustPONo);
            parameters.Add("@ItemKey", request.ItemKey);
            parameters.Add("@CustKey", request.CustKey);
            parameters.Add("@ItemType", request.ItemType);
            parameters.Add("@IncludeDetails", 0);
            parameters.Add("@FromDate", request.FromDate);
            parameters.Add("@ToDate", request.ToDate);
            parameters.Add("@BlnktType", request.BlnktType);
            parameters.Add("@CustKeys", request.CustKeys);

            DynamicParameters lineParameters = new DynamicParameters();
            lineParameters.Add("@CompanyID", request.CompID);
            lineParameters.Add("@WhseKey", request.WhseKey);
            lineParameters.Add("@TranID", request.TranID);
            lineParameters.Add("@Priority", request.Priority);
            lineParameters.Add("@CustPONo", request.CustPONo);
            lineParameters.Add("@ItemKey", request.ItemKey);
            lineParameters.Add("@CustKey", request.CustKey);
            lineParameters.Add("@ItemType", request.ItemType);
            lineParameters.Add("@IncludeDetails", 1);
            lineParameters.Add("@FromDate", request.FromDate);
            lineParameters.Add("@ToDate", request.ToDate);
            lineParameters.Add("@BlnktType", request.BlnktType);
            lineParameters.Add("@CustKeys", request.CustKeys);

            GetRecurBlnktOrdersResponse response = new GetRecurBlnktOrdersResponse();
            response.orders = new List<BlnktOrder>();
            response.lines = new List<BlnktOrder>();

            try
            {
                response.orders = sqlHandler.SQLWithRetrieveList<BlnktOrder>("spsoGetRecurOrders_RKL", CommandType.StoredProcedure, parameters);
                response.lines = sqlHandler.SQLWithRetrieveList<BlnktOrder>("spsoGetRecurOrders_RKL", CommandType.StoredProcedure, lineParameters);
            }
            catch (Exception exception)
            {
                Common.LogError(request.LoginName, exception.Message, exception.StackTrace, "SOController.GetRecurBlnktOrders", "E");
                return BadRequest(exception.Message);
            }
            return Ok(response);
        }

        /// <summary>
        /// Save or add SO Web Header User Defined Field Values
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveSOWebHdrUDF")]
        [SwaggerResponse(HttpStatusCode.OK)]
        public IHttpActionResult SaveSOWebHdrUDF(SaveSalesOrderUDFRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@SOKeyTemp", request.SOKeyTemp);
            parameters.Add("@FieldKey", request.FieldKey);
            parameters.Add("@FieldValue", request.FieldValue);
            parameters.Add("@Delete", request.Delete);

            try
            {
                using (DbConnection connection = ConnectionFactory.GetOpenConnection("DefaultConnection"))
                {
                    connection.Execute("spsoSaveSalesOrderUDFWrk_RKL", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception exception)
            {
                Common.LogError(request.LoginName, exception.Message, exception.StackTrace, "SOController.SaveSOWebHdrUDF", "E");
                return BadRequest(exception.Message);
            }
            return Ok();
        }

        [HttpPost]
        [Route("SaveSOLineUDF")]
        [SwaggerResponse(HttpStatusCode.OK)]
        public IHttpActionResult SaveSOLineUDF(SaveSOLineUDFRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@SOLineKeyTemp", request.SOLineKeyTemp);
            parameters.Add("@FieldKey", request.FieldKey);
            parameters.Add("@FieldValue", request.FieldValue);
            parameters.Add("@Delete", request.Delete);

            try
            {
                using (DbConnection connection = ConnectionFactory.GetOpenConnection("DefaultConnection"))
                {
                    connection.Execute("spsoSaveSOLineUDFWrk_RKL", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception exception)
            {
                Common.LogError(request.LoginName, exception.Message, exception.StackTrace, "SOController.SaveSOLineUDF", "E");
                return BadRequest(exception.Message);
            }
            return Ok();
        }

        [HttpPost]
        [Route("GetShoppingCart")]
        [ResponseType(typeof(ShoppingCartResponse))]
        public IHttpActionResult GetShoppingCart(SOHeaderRequest cartRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            DynamicParameters parameters = new DynamicParameters();
            DynamicParameters blanketParams = new DynamicParameters();
            parameters.Add("@CompID", cartRequest.CompID);
            if (cartRequest.SOKeyTemp == null)
            {
                parameters.Add("@SOKeyTemp", DBNull.Value);
                blanketParams.Add("@SOKeyTemp", DBNull.Value);
            }
            else
            {
                parameters.Add("@SOKeyTemp", cartRequest.SOKeyTemp);
                parameters.Add("@SOKeyTemp", cartRequest.SOKeyTemp);
            }
            if (cartRequest.Status == null)
            {
                parameters.Add("@Status", DBNull.Value);
            }
            else
            {
                parameters.Add("@Status", cartRequest.Status);
            }
            parameters.Add("@PortalSessionID", cartRequest.PortalSessionID);
            parameters.Add("@LoginName", cartRequest.SearchLoginName);
            parameters.Add("@CustID", cartRequest.CustID);
            parameters.Add("@CustPONo", cartRequest.CustPONo);


            SOHeader header = new SOHeader();
            List<SOOrders> orders = new List<SOOrders>();
            List<SOBlanket> blankets = new List<SOBlanket>();
            List<SOLine> lines = new List<SOLine>();
            List<SOAddr> address = new List<SOAddr>();
            try
            {
                orders = sqlHandler.SQLWithRetrieveList<SOOrders>("spsoGetSOOrdersWrk_rkl", System.Data.CommandType.StoredProcedure, parameters);
                blankets = sqlHandler.SQLWithRetrieveList<SOBlanket>("spsoGetSOBlanketWrk_rkl", System.Data.CommandType.StoredProcedure, blanketParams);
                lines = sqlHandler.SQLWithRetrieveList<SOLine>("spsoGetSOLineWrk_rkl", System.Data.CommandType.StoredProcedure, parameters);
                address = sqlHandler.SQLWithRetrieveList<SOAddr>("spsoGetSOAddrWrk_rkl", System.Data.CommandType.StoredProcedure, parameters);
                //add importref
                parameters.Add("@ImportRef", cartRequest.ImportRef);
                header = sqlHandler.SQLWithRetrieveSingle<SOHeader>("spsoGetSOHeaderWrk_rkl", System.Data.CommandType.StoredProcedure, parameters);
            }
            catch (Exception exception)
            {
                Common.LogError(cartRequest.LoginName, exception.Message, exception.StackTrace, "SOController.GetShoppingCart", "E");
                return BadRequest(exception.Message);
            }
            ShoppingCartResponse shoppingCartResponse = new ShoppingCartResponse();
            shoppingCartResponse.header = header;
            shoppingCartResponse.lines = lines;
            shoppingCartResponse.blanket = blankets;
            shoppingCartResponse.addr = address;
            shoppingCartResponse.orders = orders;

            return Ok(shoppingCartResponse);
        }

        #region Private Functions

        private List<RMAErrors> ValidateRMA(ref SaveRMARequest request)
        {
            List<RMAErrors> errors = new List<RMAErrors>();
            try
            {

                //validate companyid
                using (DbConnection connection = ConnectionFactory.GetOpenConnection("DefaultConnection"))
                {
                    var compIdVer = connection.Query("spciGetCompanyDetails_RKL", new { CompID = request.CompID }, commandType: CommandType.StoredProcedure).ToList();
                    if (!compIdVer.Any())
                    {
                        errors.Add(new RMAErrors() { Error = "Invalid Company ID", POLineNo = 0 });
                        return errors;
                    }
                    else
                    {
                        if (request.BillToCustAddrID.Length == 0)
                        {
                            request.BillToCustAddrID = (string)compIdVer[0].BillToCustAddrID;
                        }
                        if (request.ShipToCustAddrID.Length == 0)
                        {
                            request.ShipToCustAddrID = (string)compIdVer[0].ShipToCustAddrID;
                        }
                        if (request.CntctKey == 0)
                        {
                            request.CntctKey = (int)compIdVer[0].DfltCntctKey;
                        }
                        if (request.CurrID.Length == 0)
                        {
                            request.CurrID = (string)compIdVer[0].PrimaryAddrCurrID;
                        }
                    }
                }

                //validate custid
                DynamicParameters custIdParams = new DynamicParameters();
                custIdParams.Add("@CompID", request.CompID);
                custIdParams.Add("@CustID", request.CustID);
                custIdParams.Add("@CustKey", null);
                using (DbConnection connection = ConnectionFactory.GetOpenConnection("DefaultConnection"))
                {
                    var custIdVer = connection.Query("sparGetCustInfo_RKL", custIdParams, commandType: CommandType.StoredProcedure).Any();
                    if (!custIdVer)
                    {
                        errors.Add(new RMAErrors() { Error = "Invalid Customer ID", POLineNo = 0 });
                        return errors;
                    }
                }

                //validated BillToAddrID and ShipToAddrID
                if (request.BillToCustAddrID.Trim().Length != 0 || request.ShipToCustAddrID.Trim().Length != 0)
                {
                    DynamicParameters custAddrParams = new DynamicParameters();
                    custAddrParams.Add("@CompID", request.CompID);
                    custAddrParams.Add("@CustID", request.CustID);
                    custAddrParams.Add("@AddrKey", null);
                    custAddrParams.Add("@AddrKeyIn", null);
                    custAddrParams.Add("@SperKeyIn", null);

                    List<dynamic> custAddr = new List<dynamic>();

                    using (DbConnection connection = ConnectionFactory.GetOpenConnection("DefaultConnection"))
                    {
                        custAddr = connection.Query<dynamic>("sparGetCustOtherAddr_rkl", custAddrParams, commandType: CommandType.StoredProcedure).ToList();
                        if (custAddr.Count() > 0)
                        {
                            if (request.BillToCustAddrID.Trim().Length > 0)
                            {
                                string billToCustAddrID = request.BillToCustAddrID;
                                if (!custAddr.Where(x => x.CustAddrID == billToCustAddrID).Any())
                                {
                                    errors.Add(new RMAErrors() { Error = "Invalid Bill To Addr ID", POLineNo = 0 });
                                    return errors;
                                }
                            }
                            if (request.ShipToCustAddrID.Trim().Length > 0)
                            {
                                string shipToCustAddrID = request.ShipToCustAddrID;
                                if (!custAddr.Where(x => x.CustAddrID == shipToCustAddrID).Any())
                                {
                                    errors.Add(new RMAErrors() { Error = "Invalid Ship to Addr ID", POLineNo = 0 });
                                    return errors;
                                }
                            }
                        }
                    }
                }

                //validate contact key, if not zero
                if (request.CntctKey > 0)
                {
                    using (DbConnection connection = ConnectionFactory.GetOpenConnection("DefaultConnection"))
                    {
                        var ckeyCheck = connection.Query("spciGetContacts_RKL", new { CntctKey = request.CntctKey }, commandType: CommandType.StoredProcedure).Any();
                        if (!ckeyCheck)
                        {
                            errors.Add(new RMAErrors() { Error = "Invalid Contact Key", POLineNo = 0 });
                            return errors;
                        }
                    }
                }

                //validate currencyid
                if (request.CurrID.Length > 0)
                {
                    using (DbConnection connection = ConnectionFactory.GetOpenConnection("DefaultConnection"))
                    {
                        var checkCurrID = connection.Query("spciGetCurrencies_RKL", null, commandType: CommandType.StoredProcedure).Any();
                        if (!checkCurrID)
                        {
                            errors.Add(new RMAErrors() { Error = "Invalid Currency ID", POLineNo = 0 });
                            return errors;
                        }
                    }
                }

                foreach (var line in request.lines)
                {
                    //validate item
                    if (line.ItemKey == 0)
                    {
                        if (line.ItemID.Length > 0)
                        {
                            DynamicParameters itemParameters = new DynamicParameters();
                            itemParameters.Add("@iCompanyID", request.CompID);
                            itemParameters.Add("@iItemID", line.ItemID);
                            itemParameters.Add("@iStatus", null);

                            using (DbConnection connection = ConnectionFactory.GetOpenConnection("DefaultConnection"))
                            {
                                var itemData = connection.Query<dynamic>("spimItemLookup_rkl", itemParameters, commandType: CommandType.StoredProcedure).ToList();
                                if (itemData.Any())
                                {
                                    line.ItemKey = (int)itemData[0].ItemKey;
                                }
                            }
                        }
                    }
                    if (line.ItemKey == 0)
                    {
                        errors.Add(new RMAErrors() { Error = "Line " + line.POLineNo.ToString() + " Must provide valid ItemKey or ItemID.", POLineNo = line.POLineNo });
                    }

                    //validate unitmeas
                    if (line.UnitMeasKey == 0)
                    {
                        if (line.UnitMeasID.Length > 0)
                        {
                            using (DbConnection connection = ConnectionFactory.GetOpenConnection("DefaultConnection"))
                            {
                                var uomData = connection.Query<dynamic>("spimGetUOMKey_rkl", new { CompID = request.CompID, UOMId = line.UnitMeasID }, commandType: CommandType.StoredProcedure).ToList();
                                if (uomData.Any())
                                {
                                    line.UnitMeasKey = (int)uomData[0].UnitMeasKey;
                                }
                            }
                        }
                    }
                    if (line.UnitMeasKey == 0)
                    {
                        errors.Add(new RMAErrors() { Error = "Line " + line.POLineNo.ToString() + " Must provide valid UnitMeasKey or UnitMeasID.", POLineNo = line.POLineNo });
                    }

                    //solinedistkey and origshiplinekey
                    if (line.SOLineDistKey == 0 || line.OrigShipLineKey == 0)
                    {
                        if (line.SOTranNo.Length > 0 && line.SOLineNo > 0)
                        {
                            DynamicParameters lineDistParameters = new DynamicParameters();
                            lineDistParameters.Add("@CompID", request.CompID);
                            lineDistParameters.Add("@CustID", request.CustID);
                            lineDistParameters.Add("@ShipToAddrID", null);
                            lineDistParameters.Add("@BillToAddrID", null);
                            lineDistParameters.Add("@SOTranNo", line.SOTranNo);
                            lineDistParameters.Add("@SOLineNo", line.SOLineNo);
                            lineDistParameters.Add("@ItemID", null);
                            lineDistParameters.Add("@TrackNo", null);
                            lineDistParameters.Add("@SHTranNo", null);

                            using (DbConnection connection = ConnectionFactory.GetOpenConnection("DefaultConnection"))
                            {
                                var shipmentDetail = connection.Query<dynamic>("spsoGetShipmentRMA_rkl", lineDistParameters, commandType: CommandType.StoredProcedure).ToList();
                                if (shipmentDetail.Any())
                                {
                                    if (line.SOLineNo == 0 && line.ItemKey > 0)
                                    {
                                        shipmentDetail = shipmentDetail.Where(x => x.ItemKey == line.ItemKey).ToList();
                                    }
                                    if (line.SOLineDistKey == 0)
                                    {
                                        line.SOLineDistKey = (int)shipmentDetail[0].SOLineDistKey;
                                    }
                                    if (line.OrigShipLineKey == 0)
                                    {
                                        line.OrigShipLineKey = (int)shipmentDetail[0].ShipLineKey;
                                    }
                                }
                            }
                        }
                        if (line.SOLineDistKey == 0)
                        {
                            errors.Add(new RMAErrors() { Error = "Line " + line.POLineNo.ToString() + ": Must provide valid SOLineDistKey or valid SOTranNo/SOLineNo combination.", POLineNo = line.POLineNo });
                        }
                        if (line.OrigShipLineKey == 0)
                        {
                            errors.Add(new RMAErrors() { Error = "Line " + line.POLineNo.ToString() + ": Must provide valid OrigShipLineKey or valid SOTranNo/SOLineNo combination.", POLineNo = line.POLineNo });
                        }

                    }
                }
            }
            catch (Exception exception)
            {
                Common.LogError(request.LoginName, exception.Message, exception.StackTrace, "SOController.ValdiateRMA", "E");
            }
            return errors;
        }

        private DateTime AddDate(DateTime date, int daysInterval, bool weekdaysOnly)
        {
            DateTime newDate = date;
            if (daysInterval != 0)
            {
                int i = daysInterval;
                while (i != 0)
                {
                    if (daysInterval > 0)
                    {
                        newDate = newDate.AddDays(1);
                        var dayOfWeek = newDate.DayOfWeek;
                        if (weekdaysOnly && (dayOfWeek == DayOfWeek.Monday || dayOfWeek == DayOfWeek.Tuesday || dayOfWeek == DayOfWeek.Wednesday || dayOfWeek == DayOfWeek.Thursday || dayOfWeek == DayOfWeek.Friday))
                        {
                            i = i - 1;
                        }
                    } else {
                        newDate = newDate.AddDays(-1);
                        var dayOfWeek = newDate.DayOfWeek;
                        if (weekdaysOnly && (dayOfWeek == DayOfWeek.Monday || dayOfWeek == DayOfWeek.Tuesday || dayOfWeek == DayOfWeek.Wednesday || dayOfWeek == DayOfWeek.Thursday || dayOfWeek == DayOfWeek.Friday))
                        {
                            i = i + 1;
                        }
                    }
                }
            }
            return newDate;
        }

        private List<OrderError> GetOrderErrors(int sessKey, string loginName)
        {
            List<OrderError> orderErrors = new List<OrderError>();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Status", "Failure");
            parameters.Add("@SessionKey", sessKey);

            SqlHandler sqlHandler = new SqlHandler();

            orderErrors = sqlHandler.SQLWithRetrieveList<OrderError>("spciGetAPIErrors_rkl", System.Data.CommandType.StoredProcedure, parameters);

            return orderErrors;
        }

        private List<OrderLineRequest> AllocFreighAndTax(List<OrderLineRequest> dtItems, decimal freightAmt, string freightAllocMethQorA,
            decimal tradeDiscAmtH, decimal tradeDiscPctH, string CompID)
        {
            decimal dAmtH = 0;
            decimal qtyH = 0;
            decimal freightTotalAllocation = 0;
            decimal tradeDiscountTotalAllocation = 0;
            int lineCount = 0;
            bool allocFreight = false;

            foreach (var request in dtItems)
            {
                qtyH += request.Quantity;
                dAmtH += Math.Round((request.Quantity * request.Price), 2, MidpointRounding.AwayFromZero);
                lineCount++;
            }

            if (freightAmt != 0)
            {
                allocFreight = true;
            }

            int POLineCount = 0;

            foreach (var request in dtItems)
            {
                POLineCount++;

                if (freightAmt != 0 && request.FreightAmt == 0)
                {
                    if (freightAllocMethQorA == "Q")
                        request.FreightAmt = Math.Round(freightAmt * (request.Quantity / qtyH), 2, MidpointRounding.AwayFromZero);
                    else
                        request.FreightAmt = Math.Round(freightAmt * ((request.Quantity * request.Price) / dAmtH), 2, MidpointRounding.AwayFromZero);

                    //adjust freight for rounding
                    freightTotalAllocation += request.FreightAmt;
                    if (POLineCount == lineCount)
                    {
                        if (freightTotalAllocation != freightAmt)
                        {
                            request.FreightAmt = request.FreightAmt - (freightTotalAllocation - freightAmt);
                        }
                    }
                }

                //apply trade discount if item is eligible
                bool discountEligible = false;
                if (!string.IsNullOrEmpty(request.ItemID))
                {
                    DynamicParameters itemParameters = new DynamicParameters();
                    itemParameters.Add("@CompID", CompID);
                    itemParameters.Add("@ItemID", request.ItemID);
                    var itemData = sqlHandler.SQLWithRetrieveSingle<dynamic>("spimGetItemInfo_RKL", System.Data.CommandType.StoredProcedure, itemParameters);
                    int subjectToTradeDiscount = 0;
                    try
                    {
                        subjectToTradeDiscount = itemData.SubjToTradeDisc;
                    }
                    catch (Exception exception)
                    {
                        //logerror
                    }
                    if (subjectToTradeDiscount == 1)
                    {
                        discountEligible = true;
                    }
                }

                if (discountEligible)
                {
                    if (tradeDiscAmtH != 0 && request.TradeDiscAmt == 0)
                    {
                        request.TradeDiscAmt = Math.Round(tradeDiscAmtH * ((request.Quantity * request.Price) / dAmtH), 2, MidpointRounding.AwayFromZero);
                    }
                    else if (tradeDiscPctH != 0 && request.TradeDiscAmt == 0)
                    {
                        request.TradeDiscAmt = Math.Round(tradeDiscPctH * (request.Quantity * request.Price), 2, MidpointRounding.AwayFromZero);
                    }
                }

                tradeDiscountTotalAllocation += request.TradeDiscAmt;
            }

            //adjust trade discount amount for rounding, add difference to first non-zero line
            if (tradeDiscountTotalAllocation != tradeDiscAmtH && tradeDiscAmtH != 0)
            {
                foreach (var line in dtItems)
                {
                    if (line.Price != 0)
                    {
                        line.TradeDiscAmt = line.TradeDiscAmt - (tradeDiscountTotalAllocation - tradeDiscAmtH);
                        break;
                    }
                }
            }

            return dtItems;
        }

        #endregion
    }
}
