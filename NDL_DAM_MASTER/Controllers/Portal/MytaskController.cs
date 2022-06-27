using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Global_Master_API.Controllers.PortalGetPersonalqitem
{
    public class MytaskController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage GetPersonalqitem(Global_Master_Helper.WorkQueueItems obj)
        {
            try
            {
                var Result = Global_Master_Helper.Common.GetPersonalqitem(obj.AD_ID, obj.MasterApi, obj.WfeApi);
                return Request.CreateResponse(HttpStatusCode.OK, Result);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error occured while executing");
                throw ex;
            }
        }
    }
}
