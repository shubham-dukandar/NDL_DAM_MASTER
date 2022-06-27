using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Global_Master_Helper.Masters
{
    public class UseraccessToRole
    {
        public static List<UserAccessRoleMaster> GetAccessData()
        {
            List<UserAccessRoleMaster> lstpageAccessroleMaster = new List<UserAccessRoleMaster>();
            try
            {
                using (GlobleMasterDataContext datacontext = new GlobleMasterDataContext())
                {
                    var objM_LOCATION = (from r in datacontext.M_ACCESS_ROLEs
                                         join m in datacontext.M_ACCESS_USR_ROLE_MAPs on r.PK_ROLE_ID equals m.ACCESS_ROLE_ID
                                        // join v in datacontext.M_EMPLOYEEs on m.USER_ADID equals v.AD_ID
                                         orderby m.EMP_NAME
                                         where r.IS_ACTIVE == 1 && m.IS_ACTIVE == 1
                                         select new
                                         {
                                             r.PK_ROLE_ID,
                                             r.ROLE_NAME,
                                             m.USER_ADID,
                                             m.PK_USER_ROLE_ACCESS_ID,
                                             m.AMD_BY,
                                             m.ACCESS_ROLE_ID,
                                             m.EMP_NAME,                                           
                                             m.FK_CLIENTID
                                         }).Distinct().ToList();
                    foreach (var item in objM_LOCATION)
                    {
                        UserAccessRoleMaster objAccessroleMaster = new UserAccessRoleMaster();
                        objAccessroleMaster.ROLLNAME = new CultureInfo("en").TextInfo.ToTitleCase(item.ROLE_NAME.ToLower());
                        objAccessroleMaster.USERNAME = new CultureInfo("en").TextInfo.ToTitleCase(item.EMP_NAME.ToLower() + '_' + item.USER_ADID.ToLower());
                        objAccessroleMaster.AMDBY = item.AMD_BY;
                        objAccessroleMaster.PK_USER_ROLE_ACCESS_ID = Convert.ToInt32(item.PK_USER_ROLE_ACCESS_ID);
                        objAccessroleMaster.ACCESS_ROLE_ID = Convert.ToInt32(item.ACCESS_ROLE_ID);
                        objAccessroleMaster.USER_ADID = item.USER_ADID;
                        objAccessroleMaster.FK_CLIENTID = Convert.ToInt32(item.FK_CLIENTID);
                        objAccessroleMaster.CREATED_BY = item.AMD_BY;
                        lstpageAccessroleMaster.Add(objAccessroleMaster);
                    }
                }
                Logger1.Activity("UserAccessRole Return From Helper");
            }
            catch (Exception ex)
            {
                Logger1.Error(ex);
            }
            return lstpageAccessroleMaster;
        }

        public static string DataTransaction(List<UserAccessRoleMaster> objxml, int roleid2, string amd_by)
        {
            string Result = "";
            using (GlobleMasterDataContext db = new GlobleMasterDataContext())
            {
                try
                {
                    db.Connection.Open();
                    db.Transaction = db.Connection.BeginTransaction();
                    var acsusr = (from x in db.M_ACCESS_USR_ROLE_MAPs
                                  where x.ACCESS_ROLE_ID == roleid2
                                  select x).ToList();

                    foreach (var itm in acsusr)
                    {
                        db.M_ACCESS_USR_ROLE_MAPs.DeleteOnSubmit(itm);
                        db.SubmitChanges();
                    }
                    for (int i = 0; i < objxml.Count; i++)
                    {
                        M_ACCESS_USR_ROLE_MAP maurm = new M_ACCESS_USR_ROLE_MAP();
                        maurm.AMD_BY = amd_by;
                        maurm.AMD_DATE = DateTime.Now;
                        maurm.IS_ACTIVE = 1;
                        maurm.USER_ADID = Convert.ToString(objxml[i].USER_ADID);
                        maurm.ACCESS_ROLE_ID = objxml[i].ACCESS_ROLE_ID;
                        maurm.FK_CLIENTID = 1;
                        maurm.EMP_NAME = Convert.ToString(objxml[i].EMP_NAME);
                        maurm.EMAILID = Convert.ToString(objxml[i].EMAILID);
                        db.M_ACCESS_USR_ROLE_MAPs.InsertOnSubmit(maurm);
                        db.SubmitChanges();
                    }

                    //  var v_roletopage = db.P_INS_USER_ACESS_ROLEMAP(PageRole, roleid2);
                    var Cnt = db.GetChangeSet();
                    var Update_Count = Cnt.Inserts.Count();
                    db.SubmitChanges();
                    db.Transaction.Commit();
                    Result = "Save";
                    Logger1.Activity("UserAccessData" + Result + "In Helper");
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

        public static List<UserAccessRoleMaster> GetUsrWiseAccessData(string usrid, int clientid)
        {
            List<UserAccessRoleMaster> lstpageAccessroleMaster = new List<UserAccessRoleMaster>();
            try
            {
                using (GlobleMasterDataContext datacontext = new GlobleMasterDataContext())
                {
                    var objM_LOCATION = (from r in datacontext.M_ACCESS_ROLEs
                                         join m in datacontext.M_ACCESS_USR_ROLE_MAPs on r.PK_ROLE_ID equals m.ACCESS_ROLE_ID                                        
                                         join v in datacontext.M_EMPLOYEEs on m.USER_ADID equals v.AD_ID
                                         orderby v.EMP_NAME
                                         where r.IS_ACTIVE == 1 && m.IS_ACTIVE == 1 && m.USER_ADID == usrid && v.FK_CLIENTID == clientid
                                         && m.FK_CLIENTID == clientid && r.FK_CLIENTID == clientid
                                       //  && c.IS_ACTIVE == 1
                                         && v.IS_ACTIVE == 1
                                         select new
                                         {
                                             r.PK_ROLE_ID,
                                             r.ROLE_NAME,
                                             m.USER_ADID,
                                             v.EMP_NAME,
                                             v.EMP_CODE,
                                            // c.ITMCATNAME,
                                           //  c.PK_ITMCATID
                                         }).Distinct().ToList();
                    foreach (var item in objM_LOCATION)
                    {
                        UserAccessRoleMaster objAccessroleMaster = new UserAccessRoleMaster();
                        objAccessroleMaster.ROLLNAME = new CultureInfo("en").TextInfo.ToTitleCase(item.ROLE_NAME.ToLower());
                        objAccessroleMaster.USERNAME = new CultureInfo("en").TextInfo.ToTitleCase(item.EMP_NAME.ToLower() + '_' + item.EMP_CODE.ToLower());
                        objAccessroleMaster.USER_ADID = item.USER_ADID;
                       // objAccessroleMaster.CATNAME = item.ITMCATNAME;
                      //  objAccessroleMaster.PK_CAT_ID = item.PK_ITMCATID;
                        lstpageAccessroleMaster.Add(objAccessroleMaster);
                    }
                }
                Logger1.Activity("UserAccessRole Return From Helper");
            }
            catch (Exception ex)
            {
                Logger1.Error(ex);
            }
            return lstpageAccessroleMaster;
        }

        public static List<UserAccessRoleMaster> GetUsrWiseAccessRoleData(string usrid, int clientid)
        {
            List<UserAccessRoleMaster> lstpageAccessroleMaster = new List<UserAccessRoleMaster>();
            try
            {
                using (GlobleMasterDataContext datacontext = new GlobleMasterDataContext())
                {
                    var objM_LOCATION = (from r in datacontext.M_ACCESS_ROLEs
                                         join m in datacontext.M_ACCESS_USR_ROLE_MAPs on r.PK_ROLE_ID equals m.ACCESS_ROLE_ID
                                         join v in datacontext.M_EMPLOYEEs on m.USER_ADID equals v.AD_ID
                                         orderby v.EMP_NAME
                                         where r.IS_ACTIVE == 1 && m.IS_ACTIVE == 1 && m.USER_ADID == usrid && v.FK_CLIENTID == clientid
                                         && m.FK_CLIENTID == clientid && r.FK_CLIENTID == clientid
                                          && v.IS_ACTIVE == 1
                                         select new
                                         {
                                             r.PK_ROLE_ID,
                                             r.ROLE_NAME,
                                             m.USER_ADID,
                                             v.EMP_NAME,
                                             v.EMP_CODE
                                         }).Distinct().ToList();
                    foreach (var item in objM_LOCATION)
                    {
                        UserAccessRoleMaster objAccessroleMaster = new UserAccessRoleMaster();
                        objAccessroleMaster.ROLLNAME = new CultureInfo("en").TextInfo.ToTitleCase(item.ROLE_NAME.ToLower());
                        objAccessroleMaster.USERNAME = new CultureInfo("en").TextInfo.ToTitleCase(item.EMP_NAME.ToLower() + '_' + item.EMP_CODE.ToLower());
                        objAccessroleMaster.USER_ADID = item.USER_ADID;
                        lstpageAccessroleMaster.Add(objAccessroleMaster);
                    }
                }
                Logger1.Activity("User Access Role Return From Helper");
            }
            catch (Exception ex)
            {
                Logger1.Error(ex);
            }
            return lstpageAccessroleMaster;
        }
    }

    public class UserAccessRoleMaster
    {
        public string ROLLNAME { get; set; }
        public string USERNAME { get; set; }
        public int FK_CLIENTID { get; set; }
        public string PAGEXML { get; set; }
        public string USER_ADID { get; set; }
        public int ACCESS_ROLE_ID { get; set; }
        public string AMDBY { get; set; }
        public int PK_USER_ROLE_ACCESS_ID { get; set; }
        public string CREATED_BY { get; set; }
        public string CATNAME { get; set; }
        public int PK_CAT_ID { get; set; }
        public string EMP_NAME { get; set; }
        public string AD_ID { get; set; }
        public string EMAILID { get; set; }
    }
}
