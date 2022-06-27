using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NDL_DAM_MASTER.Controllers.Masters.DealNote
{
    public class DepartmentController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage getDepartmentInfo(Global_Master_Helper.Masters.DealNote.Department obj)
        {
            try
            {
                var Department = Global_Master_Helper.Masters.DealNote.Department_Master.getDepartment();
                if (obj.FK_CLIENTID != 0)
                {
                    Department = Department.Where(e => e.FK_CLIENTID == obj.FK_CLIENTID).ToList();
                }
                if (Department.Count > 0)
                {
                    Logger.Activity("Category data return from controller");
                    return Request.CreateResponse(HttpStatusCode.OK, Department);
                }
                else
                {
                    Logger.Activity("Get Category data");
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
