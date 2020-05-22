using ebsrest.Models;
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
    public class CommonController : ApiController
    {
        [HttpPost]
        [Route("GetPortalCustomAttributes")]
        [ResponseType(typeof(List<PortalCustomAttributes>))]
        public IHttpActionResult GetPortalCustomAttributes(GetPortalCustomAttribRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var portalCustomAttributes = Common.GetPortalCustomAttributes(request.CompID, request.AttribName, request.LoginName);

            return Ok(portalCustomAttributes);
        }
    }
}
