using Global_Master_Helper.Portal;
using log4net.Repository.Hierarchy;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using log4net;

namespace Global_Master_API.Controllers.Portal
{
    public class AuthenticateController : ApiController
    {

        [HttpPost]
        public HttpResponseMessage AuthenticateUser(Global_Master_Helper.Portal.AuthenticateMaster obj)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, Global_Master_Helper.Portal.Authenticate.GetAuthenticationDetail(obj.AD_ID.ToString()));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error occured while executing");
                throw ex;
            }

        }

        [HttpPost]
        public HttpResponseMessage DataTransaction(Global_Master_Helper.Portal.DataTransMst obj)
        {
            try
            {
                string Result = Global_Master_Helper.Portal.Authenticate.DataTransaction(obj.SaveType.ToString(), obj.UserName.ToString(), obj.newSessionID.ToString(), obj.Tokenid.ToString());
                return Request.CreateResponse(HttpStatusCode.OK, Result);
            }
            catch (Exception ex)
            {
                
                Logger.Error(ex);
                throw ex;
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error occured while executing");
            }
        }

        [HttpPost]
        public HttpResponseMessage ChangePassword(Global_Master_Helper.Portal.ChangePwdData obj)
        {
            try
            {
                List<EmployeeMaster> lsthdrData = new List<EmployeeMaster>();
                JObject xmlJObject = JObject.Parse(obj.transactiondata);
                var hdrData = Convert.ToString(xmlJObject["hdrData"]);
                lsthdrData = JsonConvert.DeserializeObject<List<EmployeeMaster>>(hdrData.ToString());
                string Result = Global_Master_Helper.Portal.ChangePwd.DataTransaction(lsthdrData, obj.savetype);
                return Request.CreateResponse(HttpStatusCode.OK, Result);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw ex;
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error occured while executing");
            }
        }
    }
}
