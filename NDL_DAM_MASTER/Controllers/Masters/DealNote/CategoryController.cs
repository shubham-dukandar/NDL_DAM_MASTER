using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NDL_DAM_MASTER.Controllers.Masters.DealNote
{
    public class CategoryController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage getCategoryInfo(Global_Master_Helper.Masters.DealNote.Category obj)
        {
            try
            {
                var Category = Global_Master_Helper.Masters.DealNote.Category_Master.getCategory();
                // var Department = Global_Master_Helper.Masters.DealNote.;
                if (obj.FK_CLIENTID != 0)
                {
                    Category = Category.Where(e => e.FK_CLIENTID == obj.FK_CLIENTID).ToList();
                }
                if (Category.Count > 0)
                {
                    Logger.Activity("Category data return from controller");
                    return Request.CreateResponse(HttpStatusCode.OK, Category);
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
