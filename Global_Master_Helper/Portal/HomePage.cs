using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global_Master_Helper.Portal
{
    public class HomePage
    {
        public static List<HomePageData> GetDashBoarddataData(string user_adid)
        {
            List<HomePageData> lstpageAccessroleMaster = new List<HomePageData>();
            try
            {
                using (GlobleMasterDataContext datacontext = new GlobleMasterDataContext())
                {
                    var objM_LOCATION = datacontext.P_GET_DASHBOARD_DATA(user_adid);
                    foreach (var item in objM_LOCATION)
                    {
                        HomePageData objHomedata = new HomePageData();
                        objHomedata.dealnote_complete = Convert.ToInt32(item.req_approved);
                        objHomedata.dealnote_pending = Convert.ToInt32(item.req_pending);
                        objHomedata.dealnote_count = Convert.ToInt32(item.dealnote_count);

                        objHomedata.dn_submit = Convert.ToInt32(item.dn_submit);
                        objHomedata.dn_approved = Convert.ToInt32(item.dn_approved);
                        objHomedata.dn_sendback = Convert.ToInt32(item.dn_sendback);
                        objHomedata.dn_modification = Convert.ToInt32(item.dn_modification);
                        lstpageAccessroleMaster.Add(objHomedata);
                    }
                }
                Logger1.Activity("Panel List Return From Helper");
            }
            catch (Exception ex)
            {
                Logger1.Error(ex);
            }
            return lstpageAccessroleMaster;
        }
    }
    public class HomePageData
    {
        public int dealnote_pending { get; set; }
        public int dealnote_complete { get; set; }
        public int dealnote_count { get; set; }
        public string USER_ADID { get; set; }

        public int dn_submit { get; set; }
        public int dn_approved { get; set; }
        public int dn_sendback { get; set; }
        public int dn_modification { get; set; }
    }

}
