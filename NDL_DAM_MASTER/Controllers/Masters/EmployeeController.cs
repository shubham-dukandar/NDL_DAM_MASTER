using Global_Master_Helper.Masters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GPLVENDOR_MASTER.Controllers.Masters
{
    public class EmployeeController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage getAllEmpInfo(Global_Master_Helper.Masters.Employee obj)
        {
            try
            {
                var Emp = Global_Master_Helper.Masters.EmployeeMaster.getAllEmployee();
                if (obj.label != "" && obj.label != null)
                {
                    Emp = Emp.Where(e => e.label.ToUpper().Contains(obj.label.ToUpper())).ToList();
                    //Emp.Where(e => e.label.Contains(obj.label)).ToList();
                }
                if (obj.AD_ID != "" && obj.AD_ID != null)
                {
                    Emp = Emp.Where(e => e.AD_ID == obj.AD_ID).ToList();
                }
                if (Emp.Count > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, Emp);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Data Not Found");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "service error");
                throw ex;
            }
        }

        [HttpPost]
        public HttpResponseMessage getEmpInfo(Global_Master_Helper.Masters.Employee obj)
        {
            try
            {
                var Emp = Global_Master_Helper.Masters.EmployeeMaster.getAllEmployee();
                //if (obj.label != "" && obj.label != null)
                //{
                //    Emp = Emp.Where(e => e.label.ToUpper().Contains(obj.label.ToUpper())).ToList();
                //    //Emp.Where(e => e.label.Contains(obj.label)).ToList();
                //}
                //if (obj.AD_ID != "" && obj.AD_ID != null)
                //{
                //    Emp = Emp.Where(e => e.AD_ID == obj.AD_ID).ToList();
                //}
                if (Emp.Count > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, Emp);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Data Not Found");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "service error");
                throw ex;
            }
        }


        [HttpPost]
        public HttpResponseMessage InsertEmpInfo(Global_Master_Helper.Masters.Employee obj)
        {
            try
            {
                var Emp = Global_Master_Helper.Masters.EmployeeMaster.SaveEmployeeData(obj.EMP_NAME, obj.EMP_CODE, obj.EMAILID, obj.AD_ID, obj.CREATED_BY, obj.FK_DEPARTMENT_ID, obj.FK_DESIGNATION_ID, obj.FK_GRADE_ID, obj.FK_BRANCH_ID, obj.MOBILE_NO, obj.ADDRESS);
                if (Emp == "true")
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "true");
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Duplicate");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Service Error..!");
                throw;
            }
        }


        [HttpPost]
        public HttpResponseMessage UpdateEmpInfo(Global_Master_Helper.Masters.Employee obj)
        {
            try
            {
                var Emp = Global_Master_Helper.Masters.EmployeeMaster.UpdateEmployeeData(obj.EMP_NAME, obj.EMP_CODE, obj.EMAILID, obj.AD_ID, obj.CREATED_BY, obj.PK_EMP_ID, obj.FK_DEPARTMENT_ID, obj.FK_DESIGNATION_ID, obj.FK_GRADE_ID, obj.FK_BRANCH_ID, obj.MOBILE_NO, obj.ADDRESS);
                if (Emp == "true")
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Data updated successfully..!");
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Error in data updation..!");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "service error..!");
                throw;
            }
        }

        [HttpPost]
        public HttpResponseMessage DeleteEmpInfo(Global_Master_Helper.Masters.Employee obj)
        {
            try
            {
                var Emp = Global_Master_Helper.Masters.EmployeeMaster.DeleteEmployeeData(obj.PK_EMP_ID);
                if (Emp == "true")
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Data deleted successfully..!");
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Error in data deletion..!");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "service error..!");
                throw;
            }
        }

        [HttpPost]
        public HttpResponseMessage getEmpInfoUseridWise(Global_Master_Helper.Masters.Employee obj)
        {
            try
            {
                var Emp = Global_Master_Helper.Masters.EmployeeMaster.getReportEmployee(obj.AD_ID);

                if (Emp.Count > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, Emp);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Data Not Found");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "service error");
                throw ex;
            }
        }


    }
}
