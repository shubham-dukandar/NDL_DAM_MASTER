using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NDL_DAM_MASTER.Controllers
{
    public class CommonController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage getaudittraildata(Global_Master_Helper.audittrail obj)
        {
            try
            {
                return Request.CreateResponse<List<Global_Master_Helper.audittrail>>(HttpStatusCode.OK, Global_Master_Helper.Common.getAudit(obj.PROCESSID, obj.INSTANCEID));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error occured while executing");
            }
        }
        [HttpPost]
        public HttpResponseMessage getDocs(Global_Master_Helper.DocMaster obj)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, Global_Master_Helper.Common.getDocumentData(obj.OBJECT_VALUE));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Request.CreateResponse(HttpStatusCode.OK, "Error");

            }
        }
        [HttpPost]
        public HttpResponseMessage getFlowodDealNote(Global_Master_Helper.FlowApproval obj)
        {
            try
            {
                return Request.CreateResponse<List<Global_Master_Helper.FlowApproval>>(HttpStatusCode.OK, Global_Master_Helper.Common.getFlowodDealNote(obj.FK_DEPT_ID,obj.FK_REQUEST_FOR_ID));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error occured while executing");
            }
        }
        [HttpPost]
        public HttpResponseMessage InsertAuditTrail(Global_Master_Helper.AuditMaster obj)
        {
            try
            {
                var Area = Global_Master_Helper.Common.InsertAuditTrail(obj.FK_PROCESSID, obj.INSTANCEID, obj.STEPNAME, obj.PERFORMERTYPE, obj.ACTIONBYUSR, obj.ACTION, obj.REMARK, obj.ACTIONDATE);
                if (Area == true)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Data Insert succesfully");
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Error");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "service error");
                throw ex;
            }
        }


        [HttpPost]
        public HttpResponseMessage SendMail(Global_Master_Helper.SendMailMaster obj)
        {
            try
            {
                var Area = Global_Master_Helper.Common.SendMail(obj.FK_PROCESSID, obj.INSTANCEID, obj.FK_WIID, obj.STEPNAME, obj.ACTION, obj.HEADERINFO, obj.ExitActionMailSubject, obj.ExitActionMailId, obj.ExitActionMailCC, obj.ExitActionMailBody);
                if (Area == true)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Data Insert succesfully");
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Error");
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
