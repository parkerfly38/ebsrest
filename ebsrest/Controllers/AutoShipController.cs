using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Dapper;
using ebsrest.Models;
using Swashbuckle.Swagger.Annotations;
using System.Data;
using System.Data.Common;
using System.Web.Http.Description;
using EBSBusinessObjects.Models;

namespace ebsrest.Controllers
{
    public class AutoShipController : ApiController
    {
        SqlHandler sqlHandler = new SqlHandler();

        /// <summary>
        /// Creates AutoShip Batch
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("InsertShipTransBatch")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(InsertShipTransBatchResponse))]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        public IHttpActionResult InsertShipTransBatch(InsertShipTransBatchRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@BeginPoint", request.BeginPoint);
            parameters.Add("@EndPoint", request.EndPoint);
            parameters.Add("@PostDate", request.PostDate);
            parameters.Add("@OvrdSegValue", request.OvrdSegValue);
            parameters.Add("@RefBatchID", request.RefBatchID);
            parameters.Add("@WhseID", request.WhseID);
            parameters.Add("@SessionKey", request.SessionKey);
            parameters.Add("@ARBatchCmnt", request.ARBatchCmnt);
            parameters.Add("@ARBatchKeyToUse", request.ARBatchKey);

            var response = new InsertShipTransBatchResponse();
            response.RowKey = 0;

            try
            {
                response.RowKey = sqlHandler.SQLWithRetrieveSingle<int>("spsoInsertShipTransBatch_rkl", CommandType.StoredProcedure, parameters);
                if (response.RowKey > 0)
                {
                    response.Status = "Success";
                }
                else
                {
                    response.Status = "Failure";
                }
            }
            catch (Exception exception)
            {
                Common.LogError(request.LoginName, exception.Message, exception.StackTrace, "AutoShipController.InsertShipTransBatch", "E");
                return BadRequest(exception.Message);
            }
            return Ok(response);
        }

        /// <summary>
        /// Creates AutoShip Shipment
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("InsertShip")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(InsertShipResponse))]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        public IHttpActionResult InsertShip(InsertShipRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@RefBatchID", request.RefBatchID);
            parameters.Add("@RefShipmentID", request.RefShipmentID);
            parameters.Add("@ShipMethID", request.ShipMethID);
            parameters.Add("@SONumber", request.SONumber);
            parameters.Add("@TranDate", request.TranDate);
            parameters.Add("@SessionKey", request.SessionKey);

            var response = new InsertShipResponse();
            response.RowKey = 0;

            try
            {
                response.RowKey = sqlHandler.SQLWithRetrieveSingle<int>("spsoInsertShip_RKL", CommandType.StoredProcedure, parameters);
                if (response.RowKey > 0)
                {
                    response.Status = "Success";
                }
                else
                {
                    response.Status = "Failure";
                }
            }
            catch (Exception exception)
            {
                Common.LogError(request.LoginName, exception.Message, exception.StackTrace, "AutoShipController.InsertShip", "E");
                return BadRequest(exception.Message);
            }
            return Ok(response);
        }

        /// <summary>
        /// Creates AutoShip Shipment Line
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("InsertShipLine")]
        [SwaggerResponse(HttpStatusCode.Created)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        public IHttpActionResult InsertShipLine(InsertShipLineRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@CompID", request.CompID);
            parameters.Add("@QtyShip", request.QtyShip);
            parameters.Add("@RefShipmentID", request.RefShipmentID);
            parameters.Add("@SONumber", request.SONumber);
            parameters.Add("@SOLineNo", request.SOLineNo);
            parameters.Add("@SessionKey", request.SessionKey);

            try
            {
                sqlHandler.SQLExecuteWithoutReturn("spsoInsertShipLine_rkl", CommandType.StoredProcedure, parameters);
            }
            catch (Exception exception)
            {
                Common.LogError(request.LoginName, exception.Message, exception.StackTrace, "AutoShipController.InsertShipLine", "E");
                return BadRequest(exception.Message);
            }
            return Created("Success", request);
        }

        /// <summary>
        /// Creates AutoShip distribution record.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("InsertShipDist")]
        [SwaggerResponse(HttpStatusCode.Created)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        public IHttpActionResult InsertShipDist(InsertShipDistRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@CompID", request.CompID);
            parameters.Add("@BinID", request.BinID);
            parameters.Add("@DistQty", request.DistQty);
            parameters.Add("@RefShipmentID", request.RefShipmentID);
            parameters.Add("@LotNo", request.LotNo);
            parameters.Add("@SONumber", request.SONumber);
            parameters.Add("@SOLineNo", request.SOLineNo);
            parameters.Add("@SessionKey", request.SessionKey);

            try
            {
                sqlHandler.SQLExecuteWithoutReturn("spsoInsertShipDist_RKL", CommandType.StoredProcedure, parameters);
            }
            catch (Exception exception)
            {
                Common.LogError(request.LoginName, exception.Message, exception.StackTrace, "AutoShipController.InsertShipDist", "E");
                return BadRequest(exception.Message);
            }

            return Created("Success", request);
        }

        /// <summary>
        /// Creates AutoShip Pack Record
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("InsertShipPack")]
        [SwaggerResponse(HttpStatusCode.Created)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        public IHttpActionResult InsertShipPack(InsertShipPackRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@RefBatchID", request.RefBatchID);
            parameters.Add("@RefShipmentID", request.RefShipmentID);
            parameters.Add("@CartonID", request.CartonID);
            parameters.Add("@Comment", request.Comment);
            parameters.Add("@FrtAmt", request.FrtAmt);
            parameters.Add("@FreightClassID", request.FreightClassID);
            parameters.Add("@ShipTrackNo", request.ShipTrackNo);
            parameters.Add("@PackageNo", request.PackageNo);
            parameters.Add("@SONumber", request.SONumber);
            parameters.Add("@SessionKey", request.SessionKey);

            int rowKey = 0;

            try
            {
                rowKey = sqlHandler.SQLWithRetrieveSingle<int>("spsoInsertShipPack_RKL", CommandType.StoredProcedure, parameters);
            }
            catch (Exception exception)
            {
                Common.LogError(request.LoginName, exception.Message, exception.StackTrace, "AutoShipController.InsertShipPack", "E");
                return BadRequest(exception.Message);
            }
            return Created("Success", rowKey);
        }

        /// <summary>
        /// InsertShipPackLine creates Shipment Packing Line
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("InsertShipPackLine")]
        [SwaggerResponse(HttpStatusCode.Created)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        public IHttpActionResult InsertShipPackLine(InsertShipPackLineRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@RefShipmentID", request.RefShipmentID);
            parameters.Add("@ItemID", request.ItemID);
            parameters.Add("@QtyShip", request.QtyShip);
            parameters.Add("@PackageNo", request.PackageNo);
            parameters.Add("@SONumber", request.SONumber);
            parameters.Add("@SOLineNo", request.SOLineNo);
            parameters.Add("@SessionKey", request.SessionKey);

            int rowKey = 0;

            try
            {
                rowKey = sqlHandler.SQLWithRetrieveSingle<int>("spsoInsertShipPackLine_rkl", CommandType.StoredProcedure, parameters);
            }
            catch (Exception exception)
            {
                Common.LogError(request.LoginName, exception.Message, exception.StackTrace, "AutoShipController.InsertShipPackLine", "E");
                return BadRequest(exception.Message);
            }
            return Created("Success", rowKey);
        }

        /// <summary>
        /// AutoShip creates shipment batch
        /// </summary>
        [HttpPost]
        [Route("AutoShip")]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.OK)]
        public IHttpActionResult AutoShip(AutoShipRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@iSessionKey", request.SessionKey);
            parameters.Add("@iCompanyID", request.CompanyID);
            parameters.Add("@iRptOption", 0);
            parameters.Add("@iUseStageTable", 1);
            parameters.Add("@oTotalRecs", 0, System.Data.DbType.Int32, ParameterDirection.Output);
            parameters.Add("@_oRetVal", 0, System.Data.DbType.Int32, ParameterDirection.Output);


            var response = new AutoShipResponse();
            int retval = 0;
            try
            {
                using (DbConnection connection = ConnectionFactory.GetOpenConnection("DefaultConnection"))
                {
                    connection.Execute("spsoAutoShip_rkl", parameters, commandType: CommandType.StoredProcedure);
                    retval = parameters.Get<int>("@_oRetVal");
                    response.TotalRecs = parameters.Get<int>("@oTotalRecs");
                }
            }
            catch (Exception exception)
            {
                Common.LogError(request.LoginName, exception.Message, exception.StackTrace, "AutoShipController.AutoShip", "E");
                return BadRequest(exception.Message);
            }
            switch (retval)
            {
                case 0:
                case -1:
                    response.StatusDetail = "SP Failure Unknown Error";
                    response.Status = "Failure";
                    break;
                case 1:
                    response.StatusDetail = "SP Successful";
                    response.Status = "Success";
                    break;
                case 2:
                    response.StatusDetail = "Warnings Only";
                    response.Status = "Suiccess";
                    break;
                case 3:
                    response.StatusDetail = "Fatal Error";
                    response.Status = "Failure";
                    break;
            }
            return Ok(response);
        }

        [HttpPost]
        [Route("InsertAutoShipQ")]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.Created)]
        public IHttpActionResult InsertAutoShipQ(InsertAutoShipQRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@SOKey", request.SOKey);
            parameters.Add("@SOLineKey", request.SOLineKey);
            parameters.Add("@BeginPoint", request.BeginPoint);
            parameters.Add("@EndPoint", request.EndPoint);
            parameters.Add("@LoginName", request.LoginName);
            parameters.Add("@CompanyID", request.CompanyID);
            try
            {
                sqlHandler.SQLExecuteWithoutReturn("spsoInsertAutoShipQ_rkl", CommandType.StoredProcedure, parameters);
            }
            catch (Exception exception)
            {
                Common.LogError(request.LoginName, exception.Message, exception.StackTrace, "AutoShipController.InsertAutoShipQ", "E");
                return BadRequest(exception.Message);
            }
            return Created("Success", 1);
        }

        [HttpPost]
        [Route("GetAutoShipQDetails")]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.OK)]
        [ResponseType(typeof(List<GetAutoShipQDetailsResponse>))]
        public IHttpActionResult GetAutoShipQDetails(GetAutoShipQDetailsRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@AutoShipQID", request.AutoShipQID);
            parameters.Add("@SOKey", request.SOKey);
            parameters.Add("@SOTranID", request.SOTranID);
            parameters.Add("@SOLineKey", request.SOLineKey);
            parameters.Add("@LoginName", request.SearchLoginName);
            parameters.Add("@Status", request.SearchStatus);
            parameters.Add("@SessionKey", request.SessionKey);
            parameters.Add("@EnQDtFr", request.EnQDtFr);
            parameters.Add("@EnQDtTo", request.EnQDtTo);
            parameters.Add("@ProcessDtTo", request.ProcessDtTo);
            parameters.Add("@ProcessDtFr", request.ProcessDtFr);
            parameters.Add("@PageIndex", request.PageIndex);
            parameters.Add("@PageSize", request.PageSize);

            parameters.Add("@_oResultSize", 0, DbType.Int32, ParameterDirection.Output);

            List<GetAutoShipQDetailsResponse> response = new List<GetAutoShipQDetailsResponse>();

            try
            {
                response = sqlHandler.SQLWithRetrieveList<GetAutoShipQDetailsResponse>("spsoGetAutoShipQDetails_RKL", CommandType.StoredProcedure, parameters);
            }
            catch (Exception exception)
            {
                Common.LogError(request.LoginName, exception.Message, exception.StackTrace, "AutoShipController.GetAutoShipDetailsQ", "E");
                return BadRequest(exception.Message);
            }
            return Ok(response);
        }

        [HttpPost]
        [Route("ProcessAutoShipFromQ")]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.Accepted)]
        public IHttpActionResult ProcessAutoShipFromQ(ProcessAutoshipFromQRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@AutoShipQID", request.AutoShipQID);
            parameters.Add("@LoadStg", request.LoadStg);

            try
            {
                sqlHandler.SQLExecuteWithoutReturn("spsoProcessAutoShipFromQ_rkl", CommandType.StoredProcedure, parameters);
            }
            catch (Exception exception)
            {
                Common.LogError(request.LoginName, exception.Message, exception.StackTrace, "AutoShipController.ProcessAutoShipFromQ", "E");
                return BadRequest(exception.Message);
            }
            return Content(HttpStatusCode.Accepted, "Success");
        }

        [HttpPost]
        [Route("GetAutoShipResults")]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.OK)]
        public IHttpActionResult GetAutoShipResults(GetAutoShipResultsRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@iSessionKey", request.SessionKey);
            parameters.Add("@AutoShipQID", request.AutoShipQID);

            GetAutoShipResultsResponse response = new GetAutoShipResultsResponse();

            try
            {
                using (DbConnection connection = ConnectionFactory.GetOpenConnection("DefaultConnection"))
                {
                    Dapper.SqlMapper.GridReader reader = connection.QueryMultiple("spsoGetAutoShipResults_RKL", parameters, commandType: CommandType.StoredProcedure);
                    response.Batch = reader.Read<AutoShipBatch>().ToList();
                    response.Shipments = reader.Read<AutoShipShipment>().ToList();
                    response.ShipmentLines = reader.Read<AutoShipShipmentLine>().ToList();
                    response.ShipmentDist = reader.Read<AutoShipShipmentDist>().ToList();
                    response.ASErrors = reader.Read<AutoShipmentQueueError>().ToList();
                    response.QLog = reader.Read<AutoShipQueueLog>().ToList();
                    response.Transactions = reader.Read<AutoShipTransactions>().ToList();
                }
            }
            catch (Exception exception)
            {
                Common.LogError(request.LoginName, exception.Message, exception.StackTrace, "AutoshipController.GetAutoShipResults", "E");
                return BadRequest(exception.Message);
            }
            return Ok(response);
        }
       
    }
}
