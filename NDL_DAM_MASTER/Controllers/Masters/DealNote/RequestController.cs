using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NDL_DAM_MASTER.Controllers.Masters.DealNote
{
    public class RequestController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage getRequsetInfo(Global_Master_Helper.Masters.DealNote.Request obj)
        {
            try
            {
                var requestfor = Global_Master_Helper.Masters.DealNote.Request_Master.getRequest();
                // var Department = Global_Master_Helper.Masters.DealNote.;
                if (obj.FK_CLIENTID != 0)
                {
                    requestfor = requestfor.Where(e => e.FK_CLIENTID == obj.FK_CLIENTID).ToList();
                }
                if (requestfor.Count > 0)
                {
                    Logger.Activity("Requestfor data return from controller");
                    return Request.CreateResponse(HttpStatusCode.OK, requestfor);
                }
                else
                {
                    Logger.Activity("Get Requestfor data");
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Data Not Found");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "service error");
                throw;
            }
        }
    }
}
