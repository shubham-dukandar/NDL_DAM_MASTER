using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global_Master_Helper.Masters.DealNote
{
    public class Region_Master
    {
        public static List<Region> getRegion()
        {
            List<Region> Region = new List<Region>();
            try
            {
                using (GlobleMasterDataContext db = new GlobleMasterDataContext())
                {
                    var req = (from a in db.M_REGIONs
                               orderby a.REGION_NAME
                               where a.IS_ACTIVE == 1
                               select new
                               {
                                   a.PK_REGION_ID,
                                   a.REGION_NAME,
                                   a.REGION_CODE,
                                   a.FK_CLIENTID,
                                   a.CREATED_BY,
                                   a.CREATED_DATE
                               }).ToList();

                    foreach (var data in req)
                    {
                        Region a = new Region();
                        a.id = Convert.ToInt32(data.PK_REGION_ID);
                        a.label = new CultureInfo("en").TextInfo.ToTitleCase(data.REGION_NAME.ToLower());
                        a.REGION_CODE = new CultureInfo("en").TextInfo.ToTitleCase(data.REGION_CODE.ToLower());
                        a.FK_CLIENTID = Convert.ToInt32(data.FK_CLIENTID);
                        a.EMP_NAME = data.CREATED_BY;
                        a.CREATION_DATE = Convert.ToDateTime(data.CREATED_DATE);
                        Region.Add(a);
                    }
                }
                // Logger1.Activity("Department List Return From Helper");
            }
            catch (Exception ex)
            {
                //Logger1.Error(ex);
            }
            return Region;
        }
    }
    public class Region
    {
        public int id { get; set; }
        public string label { get; set; }
        public string REGION_CODE { get; set; }
        public string EMP_NAME { get; set; }
        public DateTime CREATION_DATE { get; set; }
        public int FK_CLIENTID { get; set; }

    }
}