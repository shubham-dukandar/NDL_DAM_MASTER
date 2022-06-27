
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace Global_Master_Helper.Portal
{
    public class Authenticate
    {
        public static AuthenticateMaster GetAuthenticationDetail(string userid)
        {
            AuthenticateMaster objMaster = new AuthenticateMaster();
            try
            {
                using (GlobleMasterDataContext datacontext = new GlobleMasterDataContext())
                {
                    var @v_user = datacontext.M_EMPLOYEEs.Where(a => a.AD_ID == userid && a.IS_ACTIVE == 1).SingleOrDefault();

                    if (v_user != null)
                    {
                        //objMaster.PK_USER_ID = item.PK_USER_ID;
                        objMaster.AD_ID = @v_user.AD_ID;
                        objMaster.EMAIL_ID = @v_user.EMAILID;
                        objMaster.MOBILE_NO = @v_user.MOBILE_NO;
                        objMaster.EMPLOYEE_NAME = @v_user.EMP_NAME;
                        objMaster.PASSWORD = @v_user.PASSWORD;
                        objMaster.IS_ACTIVE = Convert.ToInt16(@v_user.IS_ACTIVE);
                        //objMaster.FK_USER_TYPE_ID = Convert.ToInt16(item.FK_USER_TYPE_ID);
                        objMaster.EMP_ID = @v_user.EMP_CODE;
                        objMaster.AMD_BY = @v_user.CREATED_BY;
                        objMaster.AMD_DATE = @v_user.CREATION_DATE;
                        objMaster.FK_BRANCHID = Convert.ToInt32(@v_user.FK_BRANCH_ID);
                        objMaster.FK_DESG_ID = Convert.ToInt32(@v_user.FK_DESIGNATION_ID);

                    }

                }
            }
            catch (Exception ex)
            {
                Logger1.Error(ex);
            }
            return objMaster;
        }

        public static string DataTransaction(string SaveType, string UserName, string newSessionID, string Tokenid)
        {
            try
            {
                if (SaveType.ToUpper() == "SAVE")
                {

                    using (GlobleMasterDataContext datacontext = new GlobleMasterDataContext())
                    {
                        T_LOGIN objProjectType = new T_LOGIN();
                        objProjectType.LOGINID = UserName;
                        objProjectType.LOGINTIME = DateTime.Now;
                        objProjectType.TOKENID = Tokenid;
                        objProjectType.DOMAIN = "BTS";
                        objProjectType.APP_SESSID = newSessionID;
                        objProjectType.DOMAINHOSTNAME = Dns.GetHostName().ToString();
                        datacontext.T_LOGINs.InsertOnSubmit(objProjectType);
                        datacontext.SubmitChanges();
                        return AppConstants.SaveMessage.ToString();
                    }

                }
                else if (SaveType.ToUpper() == "UPDATE")
                {
                    using (GlobleMasterDataContext datacontext = new GlobleMasterDataContext())
                    {
                        T_LOGIN objProjectType = new T_LOGIN();

                        objProjectType = datacontext.T_LOGINs.Where(x => x.LOGINID == UserName && x.APP_SESSID == newSessionID).FirstOrDefault(); //&& x.LOGINTIME == maxupdate
                        objProjectType.LOGOUTTIME = DateTime.Now;
                        datacontext.SubmitChanges();
                        return AppConstants.EditMessage;
                    }
                }
                else if (SaveType.ToUpper() == "GET")
                {
                    using (GlobleMasterDataContext datacontext = new GlobleMasterDataContext())
                    {

                        var objProjectType = datacontext.T_LOGINs.Where(x => x.LOGINID == UserName && x.APP_SESSID == newSessionID).FirstOrDefault(); //&& x.LOGINTIME == maxupdate

                        if (objProjectType != null)
                        {
                            return "true";
                        }
                        else
                        {
                            return "false";
                        }
                    }
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                Logger1.Error(ex);
                throw ex;
                return ex.Message;
            }

        }
    }

    public class AuthenticateMaster
    {
        public string AD_ID { get; set; }
        public string EMP_ID { get; set; }
        public string EMPLOYEE_NAME { get; set; }
        public string EMAIL_ID { get; set; }
        public int IS_ACTIVE { get; set; }
        public string PASSWORD { get; set; }
        public string AMD_BY { get; set; }
        public DateTime? AMD_DATE { get; set; }
        public string MOBILE_NO { get; set; }
        public int FK_BRANCHID { get; set; }
        public int FK_DESG_ID { get; set; }
    }

    public class DataTransMst
    {
        public string SaveType { get; set; }
        public string UserName { get; set; }
        public string newSessionID { get; set; }
        public string Tokenid { get; set; }

    }
}
