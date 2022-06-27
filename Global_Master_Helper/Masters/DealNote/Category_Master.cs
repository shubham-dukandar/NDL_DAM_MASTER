using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global_Master_Helper.Masters.DealNote
{
    public class Category_Master
    {
        public static List<Category> getCategory()
        {
            List<Category> Requestfor = new List<Category>();
            try
            {
                using (GlobleMasterDataContext db = new GlobleMasterDataContext())
                {
                    var req = (from a in db.M_CATEGORies
                               orderby a.CATEGORY_NAME
                               where a.IS_ACTIVE == 1
                               select new
                               {
                                   a.PK_CAT_ID,
                                   a.CATEGORY_NAME,
                                   a.CATEGORY_DESC,
                                   a.FK_CLIENTID,
                                   a.CREATED_BY,
                                   a.CREATED_DATE
                               }).ToList();

                    foreach (var data in req)
                    {
                        Category a = new Category();
                        a.id = Convert.ToInt32(data.PK_CAT_ID);
                        a.label = new CultureInfo("en").TextInfo.ToTitleCase(data.CATEGORY_NAME.ToLower());
                        a.CATEGORY_DESC = new CultureInfo("en").TextInfo.ToTitleCase(data.CATEGORY_DESC.ToLower());
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
    public class Category
    {
        public int id { get; set; }
        public string label { get; set; }
        public string CATEGORY_DESC { get; set; }

       
        public string EMP_NAME { get; set; }
        public DateTime CREATION_DATE { get; set; }
        public int FK_CLIENTID { get; set; }

    }
}
