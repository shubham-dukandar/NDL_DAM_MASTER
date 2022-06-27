using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NDL_DAM_MASTER.Controllers.Masters.DealNote
{
    public class SubCategoryController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage getSubCategoryInfo(Global_Master_Helper.Masters.DealNote.SubCategory obj)
        {
            try
            {
                var SubCategory = Global_Master_Helper.Masters.DealNote.SubCategory_Master.getSubCategory(obj.FK_CAT_ID);
                // var Department = Global_Master_Helper.Masters.DealNote.;
                if (obj.FK_CLIENTID != 0)
                {
                    SubCategory = SubCategory.Where(e => e.FK_CLIENTID == obj.FK_CLIENTID).ToList();
                }
                if (SubCategory.Count > 0)
                {
                    Logger.Activity("SubCategory data return from controller");
                    return Request.CreateResponse(HttpStatusCode.OK, SubCategory);
                }
                else
                {
                    Logger.Activity("Get SubCategory data");
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
