using Global_Master_Helper;
using Global_Master_Helper.Process.DealNote;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace NDL_DAM_MASTER.Controllers.Portal
{
    public class HomePageController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage getDashBoardData(Global_Master_Helper.Portal.HomePageData obj)
        {
            try
            {
                var Branch = Global_Master_Helper.Portal.HomePage.GetDashBoarddataData(obj.USER_ADID);
                //if (obj.ToString() != "")
                //{
                //    Branch = Branch.Where(e => e.FK_CLIENTID == obj.FK_CLIENTID).ToList();
                //}
                if (Branch.Count > 0)
                {
                    Logger.Activity("PageAccess data return from controller");
                    return Request.CreateResponse(HttpStatusCode.OK, Branch);
                }
                else
                {
                    Logger.Activity("Get PageAccess data");
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
