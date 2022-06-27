using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Global_Master_Helper.Masters.DealNote
{
    public class Request_Master
    {
        public static List<Request> getRequest()
        {
            List<Request> Requestfor = new List<Request>();
            try
            {
                using (GlobleMasterDataContext db = new GlobleMasterDataContext())
                {
                    var req = (from a in db.M_REQUESTs
                               orderby a.REQUEST_NAME
                               where a.IS_ACTIVE == 1
                               select new
                               {
                                   a.PK_REQUEST_ID,
                                   a.REQUEST_NAME,
                                   a.REQUEST_DESC,
                                   a.FK_CLIENTID,
                                   a.CREATED_BY,
                                   a.CREATED_DATE
                               }).ToList();

                    foreach (var data in req)
                    {
                        Request a = new Request();
                        a.id = Convert.ToInt32(data.PK_REQUEST_ID);
                        a.label = new CultureInfo("en").TextInfo.ToTitleCase(data.REQUEST_NAME.ToLower());
                        a.REQUEST_DESC = new CultureInfo("en").TextInfo.ToTitleCase(data.REQUEST_DESC.ToLower());
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
    public class Request
    {
        public int id { get; set; }
        public string label { get; set; }
        public string REQUEST_DESC { get; set; }
        public string EMP_NAME { get; set; }
        public DateTime CREATION_DATE { get; set; }
        public int FK_CLIENTID { get; set; }

    }
}
