using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global_Master_Helper.Masters
{
    public class PageAccess_to_AccessRole
    {
        public static List<PageAccessRoleMaster> GetPageAccessData()
        {
            List<PageAccessRoleMaster> lstpageAccessroleMaster = new List<PageAccessRoleMaster>();
            try
            {
                using (GlobleMasterDataContext datacontext = new GlobleMasterDataContext())
                {
                    var objM_LOCATION = (from map in datacontext.M_ACCESS_OBJ_ROLE_MAPs
                                         join obj in datacontext.M_ACCESSes on map.OBJECT_ID equals obj.OBJ_ID
                                         join r in datacontext.M_ACCESS_ROLEs on map.ACCESS_ROLE_ID equals r.PK_ROLE_ID
                                         orderby r.ROLE_NAME
                                         where r.IS_ACTIVE == 1 && obj.DELETED_YN == "N"
                                         select new
                                         {
                                             map.ACCESS_ROLE_ID,
                                             r.ROLE_NAME,
                                             obj.OBJ_NAME,
                                             obj.OBJ_ID,
                                             obj.FK_CLIENTID,
                                             map.AMD_BY
                                         }).ToList();
                    foreach (var item in objM_LOCATION)
                    {
                        PageAccessRoleMaster objAccessroleMaster = new PageAccessRoleMaster();
                        objAccessroleMaster.ACCESS_ROLE_ID = item.ACCESS_ROLE_ID;
                        objAccessroleMaster.ROLLNAME = new CultureInfo("en").TextInfo.ToTitleCase(item.ROLE_NAME.ToLower());
                        objAccessroleMaster.PAGENAME = new CultureInfo("en").TextInfo.ToTitleCase(item.OBJ_NAME.ToLower());
                        objAccessroleMaster.FK_CLIENTID = Convert.ToInt32(item.FK_CLIENTID);
                        objAccessroleMaster.AMD_BY = item.AMD_BY;
                        lstpageAccessroleMaster.Add(objAccessroleMaster);
                    }
                }
                Logger1.Activity("Page Access List Return From Helper");
            }
            catch (Exception ex)
            {
                Logger1.Error(ex);
            }
            return lstpageAccessroleMaster;
        }

        public static List<PanelRoleMaster> GetPanelData()
        {
            List<PanelRoleMaster> lstPanelMaster = new List<PanelRoleMaster>();
            try
            {
                using (GlobleMasterDataContext datacontext = new GlobleMasterDataContext())
                {
                    var objM_LOCATION = datacontext.M_ACCESS_PANELs.ToList().OrderBy(x => x.PANE_ID);
                    foreach (var item in objM_LOCATION)
                    {
                        PanelRoleMaster objPanelMaster = new PanelRoleMaster();
                        objPanelMaster.PANE_ID = Convert.ToInt32(item.PANE_ID);
                        objPanelMaster.PANE_DESC = new CultureInfo("en").TextInfo.ToTitleCase(item.PANE_DESC.ToLower());
                        objPanelMaster.PANE_TYPE = item.PANE_TYPE;
                        lstPanelMaster.Add(objPanelMaster);
                    }
                }
                Logger1.Activity("Panel List Return From Helper");
            }
            catch (Exception ex)
            {
                Logger1.Error(ex);
            }
            return lstPanelMaster;
        }

        public static List<PageAccessRoleMaster> GetPageData(string panelid, string roleid)
        {
            List<PageAccessRoleMaster> lstpageAccessroleMaster = new List<PageAccessRoleMaster>();
            try
            {
                using (GlobleMasterDataContext datacontext = new GlobleMasterDataContext())
                {
                    var objM_LOCATION = datacontext.P_GET_PANELWISE_PAGEACCESS(roleid, panelid);
                    foreach (var item in objM_LOCATION)
                    {
                        PageAccessRoleMaster objAccessroleMaster = new PageAccessRoleMaster();
                        objAccessroleMaster.OBJECT_ID = item.OBJ_ID;
                        objAccessroleMaster.LABEL = item.label;
                        objAccessroleMaster.HASACCESS = item.hasaccess;
                        objAccessroleMaster.ACCESS_ROLE_ID = Convert.ToInt32(item.ACCESS_ROLE_ID);
                        lstpageAccessroleMaster.Add(objAccessroleMaster);
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

        public static string DataTransaction(List<PageAccessRoleMaster> objxml, int roleid2, int panelid2,string amd_by)
        {
            string Result = "";
            using (GlobleMasterDataContext db = new GlobleMasterDataContext())
            {
                try
                {
                    db.Connection.Open();
                    db.Transaction = db.Connection.BeginTransaction();

                    //   M_ACCESS_OBJ_ROLE_MAP ma = new M_ACCESS_OBJ_ROLE_MAP();
                    var acsusr = (from x in db.M_ACCESS_OBJ_ROLE_MAPs
                                  where x.ACCESS_ROLE_ID == roleid2 && x.PANEL_ID == panelid2
                                  select x).ToList();

                    //var db = new MeatRequestDataContext();
                    //if (input.UserID > 0)
                    //{
                    //    entity = new User()
                    //    {
                    //        UserID = input.UserID
                    //    };
                    //    db.Users.Attach(entity);
                    //    db.Users.DeleteOnSubmit(entity);
                    //}
                    foreach (var itm in acsusr)
                    {
                        db.M_ACCESS_OBJ_ROLE_MAPs.DeleteOnSubmit(itm);
                        db.SubmitChanges();
                    }

                    for (int i = 0; i < objxml.Count; i++)
                    {
                        M_ACCESS_OBJ_ROLE_MAP maurm = new M_ACCESS_OBJ_ROLE_MAP();

                        maurm.AMD_BY = amd_by;
                        maurm.AMD_DATE = DateTime.Now;
                        maurm.IS_ACTIVE = 1;
                        maurm.OBJECT_ID = objxml[i].OBJECT_ID;
                        maurm.ACCESS_ROLE_ID = objxml[i].ACCESS_ROLE_ID;
                        maurm.PANEL_ID = objxml[i].PANEL_ID;
                        maurm.FK_CLIENTID = 1;
                        db.M_ACCESS_OBJ_ROLE_MAPs.InsertOnSubmit(maurm);
                        db.SubmitChanges();
                    }


                    //var v_roletopage = db.P_INS_ROLE_OBJECT_ACCESS(PageRole, roleid2, panelid2);
                    var Cnt = db.GetChangeSet();
                    var Update_Count = Cnt.Inserts.Count();
                    db.SubmitChanges();
                    db.Transaction.Commit();
                    Result = "Save";
                    Logger1.Activity("Panel List Return From Helper");
                }
                catch (Exception ex)
                {
                    Logger1.Error(ex);
                    db.Transaction.Rollback();
                    Result = AppConstants.ErrorMessage;
                }
            }
            return Result;
        }
    }

    public class PageAccessRoleMaster
    {
        public int PK_ACCESS_OBJ_ROLE_ID { get; set; }
        public int ACCESS_ROLE_ID { get; set; }
        public int OBJECT_ID { get; set; }
        public int PANEL_ID { get; set; }
        public string AMD_BY { get; set; }
        public DateTime? AMD_DATE { get; set; }
        public int? ISACTIVE { get; set; }
        public string ROLLNAME { get; set; }
        public string PAGENAME { get; set; }
        public string LABEL { get; set; }
        public int HASACCESS { get; set; }
        public int FK_CLIENTID { get; set; }
        public string PAGEXML { get; set; }
        public string CREATED_BY { get; set; }
    }

    public class PanelRoleMaster
    {
        public int PANE_ID { get; set; }
        public string PANE_DESC { get; set; }
        public string PANE_TYPE { get; set; }

    }
}
