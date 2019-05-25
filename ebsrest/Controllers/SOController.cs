using Dapper;
using ebsrest.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ebsrest.Controllers
{
    public class SOController : ApiController
    {
        SqlHandler sqlHandler = new SqlHandler();

        [Authorize]
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
            foreach (var line in orderRequest.lines)
            {
                totalPrice += line.Price;
            }

            if (totalPrice == 0)
            {
                return BadRequest("No price on items, failure to submit");
            }

            return Ok();
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

            if (freightAmt != null)
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
                
    }
}
