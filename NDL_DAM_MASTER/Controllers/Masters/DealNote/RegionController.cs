using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NDL_DAM_MASTER.Controllers.Masters.DealNote
{
    public class RegionController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage getRegionData(Global_Master_Helper.Masters.DealNote.Region obj)
        {
            try
            {
                var regiondata = Global_Master_Helper.Masters.DealNote.Region_Master.getRegion();
                // var Department = Global_Master_Helper.Masters.DealNote.;
                if (obj.FK_CLIENTID != 0)
                {
                    regiondata = regiondata.Where(e => e.FK_CLIENTID == obj.FK_CLIENTID).ToList();
                }
                if (regiondata.Count > 0)
                {
                    Logger.Activity("Requestfor data return from controller");
                    return Request.CreateResponse(HttpStatusCode.OK, regiondata);
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
