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

namespace NDL_DAM_MASTER.Controllers.Report
{
    public class DealNoteReportController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage getDealNoteHdrData()
        {
            try
            {
                return Request.CreateResponse<List<Global_Master_Helper.Report.DealNoteHdrData>>(HttpStatusCode.OK, Global_Master_Helper.Report.DealNoteReport.getDealNoteHdrData());
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error occured while executing");
            }
        }

    }
    
}
