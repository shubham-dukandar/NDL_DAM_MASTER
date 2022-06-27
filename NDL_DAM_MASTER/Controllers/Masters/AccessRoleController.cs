using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using log4net;



namespace Global_Master_API.Controllers.Masters
{
    public class AccessRoleController : ApiController
    {
        [HttpPost]
        [Route("api/AccessRole/getAccessRoleInfo")]

        public HttpResponseMessage getAccessRoleInfo(Global_Master_Helper.Masters.AccessRole obj)
        {
            try
            {
                var AccessRole = Global_Master_Helper.Masters.AccessRole_Master.getAccessRole();
                if (obj.FK_CLIENTID != 0)
                {
                    AccessRole = AccessRole.Where(e => e.FK_CLIENTID == obj.FK_CLIENTID).ToList();
                }
                if (AccessRole.Count > 0)
                {
                    Logger.Activity("Access Roles data return from controller");
                    return Request.CreateResponse(HttpStatusCode.OK, AccessRole);
                }
                else
                {
                    Logger.Activity("Get Access Roles data");
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
        public HttpResponseMessage InsertAccessRoleInfo(Global_Master_Helper.Masters.AccessRole obj)
        {
            try
            {
                var AccessRole = Global_Master_Helper.Masters.AccessRole_Master.SaveAccessRoleData(obj.label, obj.AMD_BY);
                if (AccessRole == "Save")
                {
                    Logger.Activity("Access Roles data" + AccessRole + " in controller");
                    return Request.CreateResponse(HttpStatusCode.OK, AccessRole);
                    
                }
                else
                {
                    Logger.Activity("Access Roles data" + AccessRole + " in controller");
                    return Request.CreateResponse(HttpStatusCode.OK, AccessRole);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "service error..!");
                throw;
            }
        }

        [HttpPost]
        public HttpResponseMessage UpdateAccessRoleInfo(Global_Master_Helper.Masters.AccessRole obj)
        {
            try
            {
                var AccessRole = Global_Master_Helper.Masters.AccessRole_Master.UpdateAccessRoleData(Convert.ToString(obj.id), obj.label, obj.AMD_BY);
                if (AccessRole == "Update")
                {
                    Logger.Activity("Access Roles data" + AccessRole + " in controller");
                    return Request.CreateResponse(HttpStatusCode.OK, AccessRole);
                }
                else
                {
                    Logger.Activity("Access Roles data" + AccessRole + " in controller");
                    return Request.CreateResponse(HttpStatusCode.OK, AccessRole);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "service error..!");
                throw;
            }
        }

        [HttpPost]
        public HttpResponseMessage DeleteAccessRoleInfo(Global_Master_Helper.Masters.AccessRole obj)
        {
            try
            {
                var Emp = Global_Master_Helper.Masters.AccessRole_Master.DeleteAccessRoleData(Convert.ToString(obj.id), obj.AMD_BY);
                if (Emp == "Delete")
                {
                    Logger.Activity("Access Roles data" + Emp + " in controller");
                    return Request.CreateResponse(HttpStatusCode.OK, Emp);
                }
                else
                {
                    Logger.Activity("Access Roles data" + Emp + " in controller");
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Error in data deletion..!");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "service error..!");
                throw;
            }
        }

    }
}
