using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Global_Master_API.Controllers.Portal
{
    public class WorkitemController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage GetUsersMenu(Global_Master_Helper.Portal.clsMenu obj)
        {
            try
            {
                return Request.CreateResponse<List<Global_Master_Helper.Portal.clsMenu>>(HttpStatusCode.OK, Global_Master_Helper.Portal.WorkItem.getUserMenus(obj.OBJ_TYPE.ToString(), obj.AD_ID.ToString()));
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
