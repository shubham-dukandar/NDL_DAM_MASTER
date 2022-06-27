using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Global_Master_Helper.Masters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using log4net;

namespace Global_Master_API.Controllers.Masters
{
    public class PageAccessRoleController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage getPageAccessInfo(Global_Master_Helper.Masters.PageAccessRoleMaster obj)
        {
            try
            {
                var Branch = Global_Master_Helper.Masters.PageAccess_to_AccessRole.GetPageAccessData();
                if (obj.ToString() != "")
                {
                    Branch = Branch.Where(e => e.FK_CLIENTID == obj.FK_CLIENTID).ToList();
                }
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


        [HttpPost]
        public HttpResponseMessage getPageInfo(Global_Master_Helper.Masters.PageAccessRoleMaster obj)
        {
            try
            {
                var Branch = Global_Master_Helper.Masters.PageAccess_to_AccessRole.GetPageData(Convert.ToString(obj.PANEL_ID), Convert.ToString(obj.ACCESS_ROLE_ID));

                if (Branch.Count > 0)
                {
                    Logger.Activity("Page data return from controller");
                    return Request.CreateResponse(HttpStatusCode.OK, Branch);
                }
                else
                {
                    Logger.Activity("Get Page data");
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

        [HttpPost]
        public HttpResponseMessage getPanelInfo(Global_Master_Helper.Masters.PanelRoleMaster obj)
        {
            try
            {
                var Branch = Global_Master_Helper.Masters.PageAccess_to_AccessRole.GetPanelData();

                if (Branch.Count > 0)
                {
                    Logger.Activity("Panel data return from controller");
                    return Request.CreateResponse(HttpStatusCode.OK, Branch);
                }
                else
                {
                    Logger.Activity("Get Panel data");
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

        [HttpPost]
        public HttpResponseMessage InsertPageAccessInfo(Global_Master_Helper.Masters.PageAccessRoleMaster obj)
        {
            try
            {
                List<PageAccessRoleMaster> lsthdrData = new List<PageAccessRoleMaster>();
                JObject xmlJObject = JObject.Parse(obj.PAGEXML);
                var hdrData = Convert.ToString(xmlJObject["hdrdata"]);
                lsthdrData = JsonConvert.DeserializeObject<List<PageAccessRoleMaster>>(hdrData.ToString());
                Logger.Activity("PageAccess data in controller");
                return Request.CreateResponse(HttpStatusCode.OK, PageAccess_to_AccessRole.DataTransaction(lsthdrData, obj.ACCESS_ROLE_ID, obj.PANEL_ID, obj.CREATED_BY));

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error occured while executing");
            }
        }
    }
}
