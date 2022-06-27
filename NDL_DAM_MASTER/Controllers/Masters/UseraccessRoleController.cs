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
    public class UseraccessRoleController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage getUserAccessInfo(Global_Master_Helper.Masters.UserAccessRoleMaster obj)
        {
            try
            {
                var Branch = Global_Master_Helper.Masters.UseraccessToRole.GetAccessData();
                if (obj.ToString() != "")
                {
                    Branch = Branch.Where(e => e.FK_CLIENTID == obj.FK_CLIENTID).ToList();
                }
                if (Branch.Count > 0)
                {
                    Logger.Activity("UserAccessRole return from controller");
                    return Request.CreateResponse(HttpStatusCode.OK, Branch);
                }
                else
                {
                    Logger.Activity("Get UserAccessRole data");
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
        public HttpResponseMessage getUserWiseAccessInfo(Global_Master_Helper.Masters.UserAccessRoleMaster obj)
        {
            try
            {
                var Branch = Global_Master_Helper.Masters.UseraccessToRole.GetUsrWiseAccessData(obj.USER_ADID,obj.FK_CLIENTID);
                
                if (Branch.Count > 0)
                {
                    Logger.Activity("UserAccessRole return from controller");
                    return Request.CreateResponse(HttpStatusCode.OK, Branch);
                }
                else
                {
                    Logger.Activity("Get UserAccessRole data");
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
        public HttpResponseMessage InsertPageAccessInfo(UserAccessRoleMaster obj)
        {
            try
            {
                List<UserAccessRoleMaster> lsthdrData = new List<UserAccessRoleMaster>();
                JObject xmlJObject = JObject.Parse(obj.PAGEXML);
                var hdrData = Convert.ToString(xmlJObject["hdrData"]);
                lsthdrData = JsonConvert.DeserializeObject<List<UserAccessRoleMaster>>(hdrData.ToString());
                Logger.Activity("UserAccessRole return from controller");
                return Request.CreateResponse(HttpStatusCode.OK, UseraccessToRole.DataTransaction(lsthdrData, obj.ACCESS_ROLE_ID ,obj.CREATED_BY));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error occured while executing");
            }
        }

        [HttpPost]
        public HttpResponseMessage getUserWiseAccessRoleInfo(Global_Master_Helper.Masters.UserAccessRoleMaster obj)
        {
            try
            {
                var Branch = Global_Master_Helper.Masters.UseraccessToRole.GetUsrWiseAccessRoleData(obj.USER_ADID, obj.FK_CLIENTID);

                if (Branch.Count > 0)
                {
                    Logger.Activity("UserAccessRole return from controller");
                    return Request.CreateResponse(HttpStatusCode.OK, Branch);
                }
                else
                {
                    Logger.Activity("Get UserAccessRole data");
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



       