using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NDL_DAM_MASTER.Controllers.Masters.DealNote
{
    public class CurrencyController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage getCurrencyInfo(Global_Master_Helper.Masters.DealNote.Currency obj)
        {
            try
            {
                var Currency = Global_Master_Helper.Masters.DealNote.Currency_Master.getCurrency();
               // var Department = Global_Master_Helper.Masters.DealNote.;
                if (obj.FK_CLIENTID != 0)
                {
                    Currency = Currency.Where(e => e.FK_CLIENTID == obj.FK_CLIENTID).ToList();
                }
                if (Currency.Count > 0)
                {
                    Logger.Activity("Currency data return from controller");
                    return Request.CreateResponse(HttpStatusCode.OK, Currency);
                }
                else
                {
                    Logger.Activity("Get Currency data");
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
