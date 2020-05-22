using Dapper;
using ebsrest.Models;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using EBSBusinessObjects.Models;

namespace ebsrest.Controllers
{
    [Authorize]
    public class IMController : ApiController
    {
        SqlHandler sqlHandler = new SqlHandler();

        /// <summary>
        /// Retrieves Warehouse Bins
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(List<GetWhseBinsResponse>))]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [Route("IM/GetWhseBins")]
        public IHttpActionResult GetWhseBins(GetWhseBinsRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@CompID", request.CompID);
            parameters.Add("@WhseKey", request.WhseKey);
            parameters.Add("@WhseBinID", request.WhseBinID.Replace("'", "''"));

            List<GetWhseBinsResponse> response = new List<GetWhseBinsResponse>();

            try
            {
                response = sqlHandler.SQLWithRetrieveList<GetWhseBinsResponse>("spimGetWhseBin_RKL", System.Data.CommandType.StoredProcedure, parameters);
            }
            catch (Exception exception)
            {
                Common.LogError(request.LoginName, exception.Message, exception.StackTrace, "IMController.GetWhseBins", "E");
                return BadRequest(exception.Message);
            }
            return Ok(response);
        }

        /// <summary>
        /// Gets Item Classes
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("IM/GetItemClasses")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(List<GetItemClassesResponse>))]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        public IHttpActionResult GetItemClasses(SimpleRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@CompID", request.CompID);
            parameters.Add("@ItemClassID", string.Empty);

            var response = new List<GetItemClassesResponse>();

            try
            {
                response = sqlHandler.SQLWithRetrieveList<GetItemClassesResponse>("spimGetItemClasses_RKL", System.Data.CommandType.StoredProcedure, parameters);
            }
            catch (Exception exception)
            {
                Common.LogError(request.LoginName, exception.Message, exception.StackTrace, "IMController.GetItemClasses", "E");
                return BadRequest(exception.Message);
            }
            return Ok(response);
        }

        /// <summary>
        /// Gets Item Sales Product Lines
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("IM/GetSlsProdLines")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(List<GetItemSlsProdLineResponse>))]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        public IHttpActionResult GetItemSlsProdLines(SimpleRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@CompID", request.CompID);

            var response = new List<GetItemSlsProdLineResponse>();

            try
            {
                response = sqlHandler.SQLWithRetrieveList<GetItemSlsProdLineResponse>("spimGetItemSalesProdLines_RKL", System.Data.CommandType.StoredProcedure, parameters);
            }
            catch (Exception exception)
            {
                Common.LogError(request.LoginName, exception.Message, exception.StackTrace, "IMController.GetItemSlsProdLines", "E");
                return BadRequest(exception.Message);
            }
            return Ok(response);
        }

        /// <summary>
        /// Get Item Purchase Prod Lines
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("IM/GetItemPurchProdLines")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(List<GetItemPurchProdLineResponse>))]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        public IHttpActionResult GetItemPurchProdLines(GetItemPurchProdLinesRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@CompID", request.CompID);
            parameters.Add("@PurchProdLineID", request.PurchProdLineID.Replace("'", "''"));

            var response = new List<GetItemPurchProdLineResponse>();

            try
            {
                response = sqlHandler.SQLWithRetrieveList<GetItemPurchProdLineResponse>("spimGetPurchProdLineDetails_rkl", System.Data.CommandType.StoredProcedure, parameters);
            }
            catch (Exception exception)
            {
                Common.LogError(request.LoginName, exception.Message, exception.StackTrace, "IMController.GetItemPurchProdLines", "E");
                return BadRequest(exception.Message);
            }
            return Ok(response);
        }

        /// <summary>
        /// Gets units of measure by ItemID
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("IM/GetItemUOMs")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(List<GetItemUOMsResponse>))]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        public IHttpActionResult GetItemUOMs(GetItemUOMsRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@CompID", request.CompID);
            parameters.Add("@ItemID", request.ItemID.Replace("'", "''"));

            var response = new List<GetItemUOMsResponse>();

            try
            {
                response = sqlHandler.SQLWithRetrieveList<GetItemUOMsResponse>("spimGetItemUOMs_RKL", System.Data.CommandType.StoredProcedure, parameters);
            }
            catch (Exception exception)
            {
                Common.LogError(request.LoginName, exception.Message, exception.StackTrace, "IMController.GetItemUOMs", "E");
                return BadRequest(exception.Message);
            }
            return Ok(response);
        }

        /// <summary>
        /// Gets UOMKey by UOM ID
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("IM/GetUOMKey")]
        [SwaggerResponse(HttpStatusCode.OK, Type = (typeof(int)))]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        public IHttpActionResult GetUOMKey(GetUOMKeyRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@CompID", request.CompID);
            parameters.Add("@UOMId", request.UOMID.Replace("'", "''"));

            int UOMKey = 0;
            try
            {
                UOMKey = sqlHandler.SQLWithRetrieveSingle<int>("spimGetUOMKey_RKL", System.Data.CommandType.StoredProcedure, parameters);
            }
            catch (Exception exception)
            {
                Common.LogError(request.LoginName, exception.Message, exception.StackTrace, "IMController.GetUOMKey", "E");
                return BadRequest(exception.Message);
            }
            return Ok(UOMKey);
        }
    }
}
