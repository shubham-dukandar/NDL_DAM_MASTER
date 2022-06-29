using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Global_Master_Helper.Masters.DealNote
{
    public class Division_Master
    {
        public static List<Division> getDivision()
        {
            List<Division> Division = new List<Division>();
            try
            {
                using (GlobleMasterDataContext db = new GlobleMasterDataContext())
                {
                    var req = (from a in db.M_DIVISIONs
                               orderby a.DIVISION_NAME
                               where a.IS_ACTIVE == 1
                               select new
                               {
                                   a.PK_DIVISION_ID,
                                   a.DIVISION_NAME,
                                   a.DIVISION_CODE,
                                   a.FK_CLIENTID,
                                   a.CREATED_BY,
                                   a.CREATED_DATE
                               }).ToList();

                    foreach (var data in req)
                    {
                        Division a = new Division();
                        a.id = Convert.ToInt32(data.PK_DIVISION_ID);
                        a.label = new CultureInfo("en").TextInfo.ToTitleCase(data.DIVISION_NAME.ToUpper());
                        a.DIVISON_CODE = new CultureInfo("en").TextInfo.ToTitleCase(data.DIVISION_CODE.ToLower());
                        a.FK_CLIENTID = Convert.ToInt32(data.FK_CLIENTID);
                        a.EMP_NAME = data.CREATED_BY;
                        a.CREATION_DATE = Convert.ToDateTime(data.CREATED_DATE);
                        Division.Add(a);
                    }
                }
                // Logger1.Activity("Department List Return From Helper");
            }
            catch (Exception ex)
            {
                //Logger1.Error(ex);
            }
            return Division;
        }
    }
    public class Division
    {
        public int id { get; set; }
        public string label { get; set; }
        public string DIVISON_CODE { get; set; }
        public string EMP_NAME { get; set; }
        public DateTime CREATION_DATE { get; set; }
        public int FK_CLIENTID { get; set; }

    }
}