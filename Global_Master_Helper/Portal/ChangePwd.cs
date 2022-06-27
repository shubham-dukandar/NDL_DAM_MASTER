using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global_Master_Helper.Portal
{
    public class ChangePwd
    {
        public static string DataTransaction(List<EmployeeMaster> objeMPLOYEEMaster, string SaveType)
        {
            if (SaveType.ToUpper() == "UPDATE")
            {
                using (GlobleMasterDataContext datacontext = new GlobleMasterDataContext())
                {
                    try
                    {
                        //FSL.Cryptography.BlowFish bw = new FSL.Cryptography.BlowFish();
                        //bw.Initialize("FLOLOGIC");
                        //string pwd = bw.Decrypt(newpwd.ToString());

                        FSL.Cryptography.BlowFish bw = new FSL.Cryptography.BlowFish();
                        bw.Initialize("FLOLOGIC");
                        string newpwd = objeMPLOYEEMaster[0].PASSWORD;
                        string encryptpwd = bw.Encrypt(newpwd.ToString());

                        M_EMPLOYEE objEmployee = new M_EMPLOYEE();
                        objEmployee = datacontext.M_EMPLOYEEs.Where(x => x.AD_ID == objeMPLOYEEMaster[0].LOGIN_NAME).FirstOrDefault();
                        objEmployee.PASSWORD = encryptpwd;
                        datacontext.SubmitChanges();
                        return "Password Change Successfully";

                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }
                }
            }
            else if (SaveType.ToUpper() == "GET")
            {
                using (GlobleMasterDataContext datacontext = new GlobleMasterDataContext())
                {
                    try
                    {
                        string result = string.Empty;
                        M_EMPLOYEE checkobjusr = datacontext.M_EMPLOYEEs.Where(x => x.AD_ID == objeMPLOYEEMaster[0].LOGIN_NAME).FirstOrDefault();

                        if (checkobjusr != null)
                        {
                            FSL.Cryptography.BlowFish bw = new FSL.Cryptography.BlowFish();
                            bw.Initialize("FLOLOGIC");
                            string oldpwd = bw.Decrypt(checkobjusr.PASSWORD);

                            if (oldpwd != objeMPLOYEEMaster[0].CURRENTPWD)
                            {
                                result = "Current Password Not Match!!";
                            }
                            else
                            {
                                result = "";
                            }
                        }
                        else
                        {
                            result = "";
                        }
                        return result;
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }
                }
            }
            else if (SaveType.ToUpper() == "GETMAILID")
            {
                using (GlobleMasterDataContext datacontext = new GlobleMasterDataContext())
                {
                    try
                    {
                        string result = string.Empty;
                        M_EMPLOYEE checkobjusr = datacontext.M_EMPLOYEEs.Where(x => x.AD_ID == objeMPLOYEEMaster[0].LOGIN_NAME).FirstOrDefault();

                        if (checkobjusr != null)
                        {
                            string oldmailid = checkobjusr.EMAILID;

                            if (oldmailid != objeMPLOYEEMaster[0].EMAILID)
                            {
                                result = "Email ID is not correct...!";
                            }
                            else
                            {
                                result = "";
                            }
                        }
                        else
                        {
                            result = "Login Id is not Available";
                        }
                        return result;
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }
                }
            }
            else if (SaveType.ToUpper() == "UPDATEPWD")
            {
                using (GlobleMasterDataContext datacontext = new GlobleMasterDataContext())
                {
                    try
                    {

                        string _allowedChars = "0123456789abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ";

                        Random randNum = new Random();
                        char[] chars = new char[7];
                        int allowedCharCount = _allowedChars.Length;
                        string randpwd = "123";
                        //for (int i = 0; i < 7; i++)
                        //{
                        //    chars[i] = _allowedChars[(int)((_allowedChars.Length) * randNum.NextDouble())];
                        //    randpwd = randpwd + chars[i];
                        //}

                        FSL.Cryptography.BlowFish bw = new FSL.Cryptography.BlowFish();
                        bw.Initialize("FLOLOGIC");
                        string newencryptpwd = bw.Encrypt(Convert.ToString(randpwd));

                        var checkobjusr = datacontext.M_EMPLOYEEs.Where(x => x.AD_ID == objeMPLOYEEMaster[0].LOGIN_NAME).FirstOrDefault();
                        if (checkobjusr != null)
                        {
                            string oldmailid = checkobjusr.EMAILID;

                            if (oldmailid != objeMPLOYEEMaster[0].EMAILID)
                            {
                                return "Email ID is not correct...!";
                            }
                            else
                            {
                                checkobjusr.PASSWORD = newencryptpwd;//"ZpZFJFvaIxZPi1kfHJj0Lg==";
                                datacontext.SubmitChanges();

                                string @MAILheader = "Forgot-Password";
                                string MAILBODY = "<pre><font size='3'>Hello Sir/Madam,</font></pre>";
                                MAILBODY += "<pre><font size='3'>Your password is reset.</font></pre>";
                                MAILBODY += "<pre><font size='3'>Login Id is :" + objeMPLOYEEMaster[0].LOGIN_NAME + "</font></pre>";
                                MAILBODY += "<pre><font size='3'>Password is : " + randpwd + "</font></pre>";
                                MAILBODY += "<pre><font size='3'>INTERANET URL:<a data-cke-saved-href=<a data-cke-saved-href={http://172.21.60.109/Login.aspx} href={http://172.21.60.109/Login.aspx}>http://172.21.60.109/Login.aspx</a></font></pre><pre></a>";
                                MAILBODY += "<pre><font size='3'>Regards</font></pre>";
                                MAILBODY += "<pre><font size='3'>Reporting Admin</font></pre>";
                                MAILBODY += "<pre><font size='3' color='red'><i><b>This is a system generated message. We request you not to reply to this message.</b></i></font></pre>";

                                T_MAILLOG objCustomer = new T_MAILLOG();
                                objCustomer.FK_PROCESSID = 0;
                                objCustomer.FK_INSTANCEID = 0;
                                objCustomer.FK_WIID = 0;
                                objCustomer.STEP_NAME = "Forgot-Password";
                                objCustomer.TO_MAIL = objeMPLOYEEMaster[0].EMAILID;
                                objCustomer.CC_MAIL = "";
                                objCustomer.MAIL_BODY = MAILBODY;
                                objCustomer.MAILSENT_FLAG = true;
                                objCustomer.HEADER = @MAILheader;
                                objCustomer.ACTION = "SUBMIT";
                                objCustomer.CREATION_DATE = DateTime.Now;
                                datacontext.T_MAILLOGs.InsertOnSubmit(objCustomer);
                                datacontext.SubmitChanges();

                                return "Password Reset successfully. Reset Password " + randpwd + "...!";
                            }
                        }
                        else
                        {
                            return "Login ID is not available";
                        }

                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }
                }
            }
            else
            {
                return "";
            }
        }
    }


    public class EmployeeMaster
    {
        public string LOGIN_NAME { get; set; }
        public string PASSWORD { get; set; }
        public string CURRENTPWD { get; set; }
        public string EMAILID { get; set; }
    }

    public class HDMAILMaster
    {
        public int FK_PROCESSID { get; set; }
        public int FK_INSTANCEID { get; set; }
        public int FK_WIID { get; set; }
        public string STEP_NAME { get; set; }
        public string TO_MAIL { get; set; }
        public string CC_MAIL { get; set; }
        public string MAIL_BODY { get; set; }
        public string MAILSENT_FLAG { get; set; }
        public string HEADER { get; set; }
        public string ACTION { get; set; }
        public DateTime? CREATION_DATE { get; set; }
    }

    public class ChangePwdData
    {
        public string transactiondata { get; set; }
        public string savetype { get; set; }

    }
}
