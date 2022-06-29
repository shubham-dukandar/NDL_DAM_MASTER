using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Global_Master_Helper.Masters.DealNote
{
    public class SubCategory_Master
    {
        public static List<SubCategory> getSubCategory(int FK_CAT_ID)
        {
            List<SubCategory> Requestfor = new List<SubCategory>();
            try
            {
                using (GlobleMasterDataContext db = new GlobleMasterDataContext())
                {
                    var req = (from a in db.M_SUB_CATEGORies
                               orderby a.SUB_CATEGORY_NAME
                               where a.IS_ACTIVE == 1 && a.FK_CAT_ID == FK_CAT_ID
                               select new
                               {
                                   a.PK_SUB_CAT_ID,
                                   a.SUB_CATEGORY_NAME,
                                   a.SUB_CATEGORY_DESC,
                                   a.FK_CAT_ID,
                                   a.FK_CLIENTID,
                                   a.CREATED_BY,
                                   a.CREATED_DATE
                               }).ToList();

                    foreach (var data in req)
                    {
                        SubCategory a = new SubCategory();
                        a.id = Convert.ToInt32(data.PK_SUB_CAT_ID);
                        a.label = new CultureInfo("en").TextInfo.ToTitleCase(data.SUB_CATEGORY_NAME);
                        a.SUB_CATEGORY_DESC = new CultureInfo("en").TextInfo.ToTitleCase(data.SUB_CATEGORY_DESC.ToLower());
                        a.FK_CLIENTID = Convert.ToInt32(data.FK_CLIENTID);
                        a.EMP_NAME = data.CREATED_BY;
                        a.CREATION_DATE = Convert.ToDateTime(data.CREATED_DATE);
                        Requestfor.Add(a);
                    }
                }
                // Logger1.Activity("Department List Return From Helper");
            }
            catch (Exception ex)
            {
                //Logger1.Error(ex);
            }
            return Requestfor;
        }
    }
    public class SubCategory
    {
        public int id { get; set; }
        public string label { get; set; }
        public string SUB_CATEGORY_DESC { get; set; }
        public int FK_CAT_ID { get; set; }
        public string EMP_NAME { get; set; }
        public DateTime CREATION_DATE { get; set; }
        public int FK_CLIENTID { get; set; }

    }
}
