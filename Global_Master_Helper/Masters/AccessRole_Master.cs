using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global_Master_Helper.Masters
{
    public class AccessRole_Master
    {
        public static List<AccessRole> getAccessRole()
        {
            List<AccessRole> accessrole = new List<AccessRole>();
            try
            {
                using (GlobleMasterDataContext db = new GlobleMasterDataContext())
                {
                    var Role = (from a in db.M_ACCESS_ROLEs
                                orderby a.ROLE_NAME
                                where a.IS_ACTIVE == 1
                                select new
                                {
                                   a.PK_ROLE_ID,
                                    a.ROLE_NAME,
                                    a.FK_CLIENTID,
                                    a.AMD_BY,
                                    a.AMD_DATE
                                }).ToList();

                    foreach (var data in Role)
                    {
                        AccessRole a = new AccessRole();
                        a.id = Convert.ToInt32(data.PK_ROLE_ID);
                        a.label = new CultureInfo("en").TextInfo.ToTitleCase(data.ROLE_NAME.ToLower());
                        a.FK_CLIENTID = Convert.ToInt32(data.FK_CLIENTID);
                        a.AMD_BY = data.AMD_BY;
                        a.AMD_DATE = Convert.ToDateTime(data.AMD_DATE);
                        accessrole.Add(a);
                    }
                }
                Logger1.Activity("Access Role List return from Helper");
            }
            catch (Exception ex)
            {
                Logger1.Error(ex);
            }
            return accessrole;
        }

        public static string SaveAccessRoleData(string RoleName, string UserID)
        {
            string Result = "";

            using (GlobleMasterDataContext db = new GlobleMasterDataContext())
            {
                try
                {
                    db.Connection.Open();
                    db.Transaction = db.Connection.BeginTransaction();
                    M_ACCESS_ROLE check = (from c in db.M_ACCESS_ROLEs where c.IS_ACTIVE == 1 && c.ROLE_NAME == RoleName select c).FirstOrDefault();
                    if (check == null)
                    {

                        M_ACCESS_ROLE accessrole = new M_ACCESS_ROLE();
                        accessrole.ROLE_NAME = RoleName;
                        accessrole.IS_ACTIVE = 1;
                        accessrole.FK_CLIENTID = 1;
                        accessrole.AMD_BY = UserID;
                        accessrole.AMD_DATE = DateTime.Now;
                        db.M_ACCESS_ROLEs.InsertOnSubmit(accessrole);
                        var Cnt = db.GetChangeSet();
                        var Update_Count = Cnt.Inserts.Count();
                        db.SubmitChanges();
                        db.Transaction.Commit();
                        Result = "Save";
                    }
                    else
                    {
                        Result = "Duplicate";
                    }
                    Logger1.Activity("Access Roles Data " + Result + " in Helper");
                }
                catch (Exception ex)
                {
                    Logger1.Error(ex);
                    db.Transaction.Rollback();
                    Result = Global_Master_Helper.AppConstants.ErrorMessage;
                }
            }
            return Result;
        }

        public static string UpdateAccessRoleData(string PK_ROLEID, string RoleName, string UserID)
        {
            string Result = "";
            using (GlobleMasterDataContext db = new GlobleMasterDataContext())
            {
                try
                {
                    var check = (from c in db.M_ACCESS_ROLEs where c.IS_ACTIVE == 1 && c.ROLE_NAME == RoleName select c).FirstOrDefault();
                    if (check == null)
                    {
                        db.Connection.Open();
                        db.Transaction = db.Connection.BeginTransaction();
                        M_ACCESS_ROLE mar = new M_ACCESS_ROLE();
                        var product = db.M_ACCESS_ROLEs.Single(x => x.PK_ROLE_ID == Convert.ToInt32(PK_ROLEID));
                           product.PK_ROLE_ID = Convert.ToInt32(PK_ROLEID);
                        product.ROLE_NAME = RoleName;
                        product.AMD_BY = UserID;
                        product.AMD_DATE = DateTime.Now;
                        db.SubmitChanges();
                        db.Transaction.Commit();
                        Result = "Update";
                    }
                    else
                    {
                        Result = "Duplicate";
                    }
                    Logger1.Activity("Access Roles Data " + Result + "in Helper");
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

        public static string DeleteAccessRoleData(string PK_ROLEID, string UserID)
        {
            string Result = "";
            using (GlobleMasterDataContext db = new GlobleMasterDataContext())
            {
                try
                {
                    db.Connection.Open();
                    db.Transaction = db.Connection.BeginTransaction();
                    M_ACCESS_ROLE mz = new M_ACCESS_ROLE();
                    var students = db.M_ACCESS_ROLEs.SingleOrDefault(s => s.PK_ROLE_ID == Convert.ToInt32(PK_ROLEID));
                    if (students != null)
                    {
                        students.IS_ACTIVE = 0;
                        students.AMD_BY = UserID;
                        students.AMD_DATE = DateTime.Now;
                        db.SubmitChanges();
                        db.Transaction.Commit();
                        Result = "Delete";
                    }
                    Logger1.Activity("Access Roles Data " + Result + " in Helper");
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


    public class AccessRole
    {
        public int id { get; set; }
        public string label { get; set; }
        public string AMD_BY { get; set; }
        public DateTime AMD_DATE { get; set; }
        public int FK_CLIENTID { get; set; }
    }
}
