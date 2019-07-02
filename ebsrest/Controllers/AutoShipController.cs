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
        [SwaggerResponse(HttpStatusCode.OK, Type= typeof(InsertShipTransBatchResponse))]
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

        [HttpPost]
        [
    }
}
