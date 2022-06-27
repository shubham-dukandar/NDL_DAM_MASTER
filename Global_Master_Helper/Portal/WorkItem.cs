using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global_Master_Helper.Portal
{
    public class WorkItem
    {



        public static List<clsMenu> getUserMenus(string mtype, string ad_id)
        {
            List<clsMenu> lstWorkItem = new List<clsMenu>();
            GlobleMasterDataContext datacontext = new GlobleMasterDataContext();
            try
            {
                var rpt = (from v in datacontext.M_ACCESSes
                           join p in datacontext.M_ACCESS_PANELs on v.PANEL_ID equals p.PANE_ID
                           join am in datacontext.M_ACCESS_OBJ_ROLE_MAPs on v.OBJ_ID equals am.OBJECT_ID
                           join r in datacontext.M_ACCESS_ROLEs on am.ACCESS_ROLE_ID equals r.PK_ROLE_ID
                           join rm in datacontext.M_ACCESS_USR_ROLE_MAPs on r.PK_ROLE_ID equals rm.ACCESS_ROLE_ID
                           where v.DELETED_YN == "N" && p.PANE_DESC == mtype && rm.USER_ADID == ad_id
                           && am.IS_ACTIVE == 1 && rm.IS_ACTIVE == 1
                           select v).Distinct().OrderBy(x => x.OBJ_SEQ).ToList();
                foreach (var t in rpt)
                {
                    clsMenu menu = new clsMenu();
                    menu.OBJ_ID = t.OBJ_ID;
                    menu.OBJ_NAME = t.OBJ_NAME;
                    menu.OBJ_PARENT_ID = Convert.ToInt32(t.OBJ_PARENT_ID);
                    menu.OBJ_TYPE = t.OBJ_TYPE;
                    menu.OBJ_URL = t.OBJ_URL;
                    menu.PANEL_ID = Convert.ToInt32(t.PANEL_ID);
                    menu.PROCESS_NAME = t.PROCESS_NAME;
                    lstWorkItem.Add(menu);
                }
                return lstWorkItem;
            }
            catch (Exception ex)
            {
                Logger1.Error(ex);
                return lstWorkItem;
            }
        }
    }

    public class WorkItemMaster
    {
        public int OBJ_ID { get; set; }
        public string OBJ_PARENT_ID { get; set; }
        public string OBJ_LEVEL { get; set; }
        public int OBJ_SEQ { get; set; }
        public string OBJ_NAME { get; set; }
        public string OBJ_URL { get; set; }
        public string OBJ_TYPE { get; set; }
        public string PROCESS_RELATED_DATA { get; set; }
        public string TBALENAME { get; set; }
        public string tblcnt { get; set; }
    }

    //-----------Added by sagar------------

    public class clsMenu
    {
        public int OBJ_ID { get; set; }
        public string OBJ_NAME { get; set; }
        public int OBJ_PARENT_ID { get; set; }
        public string OBJ_TYPE { get; set; }
        public string OBJ_URL { get; set; }
        public int PANEL_ID { get; set; }
        public string PROCESS_NAME { get; set; }
        public string AD_ID { get; set; }
    }
}
