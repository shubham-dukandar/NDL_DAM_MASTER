using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;

using System.Configuration;
using System.Data;
using System.Web.Script.Serialization;



namespace Global_Master_Helper
{
    public class Common
    {
        public static string SaveDocuments(string ProcessName, string foldername, List<DocMaster> objDocDetail)
        {
            string Result = "";
            string activeDir = ConfigurationManager.AppSettings["DOCPATH"].ToString().Trim();
            string path = string.Empty;
            foldername = foldername.Replace("/", "_");
            path = activeDir + "\\" + ProcessName + "\\" + foldername;
            if (Directory.Exists(path))
            {
                string[] directories = Directory.GetFiles(activeDir + "\\" + ProcessName + "\\");
                path = path + "\\";
                foreach (var directory in directories)
                {
                    for (int i = 0; i < objDocDetail.Count; i++)
                    {

                        var sections = directory.Split('\\');
                        var fileName = sections[sections.Length - 1];
                        if (objDocDetail[i].FILENAME == fileName)
                        {
                            System.IO.File.Move(activeDir + "\\" + ProcessName + "\\" + fileName, path + fileName);
                            if (System.IO.File.Exists(activeDir + "\\" + ProcessName + "\\" + fileName))
                            {
                                System.IO.File.Delete(activeDir + "\\" + ProcessName + "\\" + fileName);
                            }
                            Result = "true";
                        }
                    }
                }
            }
            else
            {
                Directory.CreateDirectory(path);
                string[] directories = Directory.GetFiles(activeDir + "\\" + ProcessName + "\\");
                path = path + "\\";
                foreach (var directory in directories)
                {
                    for (int i = 0; i < objDocDetail.Count; i++)
                    {
                        var sections = directory.Split('\\');
                        var fileName = sections[sections.Length - 1];
                        if (objDocDetail[i].FILENAME == fileName)
                        {
                            System.IO.File.Move(activeDir + "\\" + ProcessName + "\\" + fileName, path + fileName);
                            if (System.IO.File.Exists(activeDir + "\\" + ProcessName + "\\" + fileName))
                            {
                                System.IO.File.Delete(activeDir + "\\" + ProcessName + "\\" + fileName);
                            }
                            Result = "true";
                        }
                    }
                }
            }
            //Logger1.Activity("Data Saved Successfully");
            return Result;
        }
        public static List<WorkQueueItems> GetPersonalqitem(string adid, string MasterUsrl, string WfeUrl)
        {
            List<WorkQueueItems> lstWorkQueueItems = new List<WorkQueueItems>();
            GlobleMasterDataContext context = new GlobleMasterDataContext();

            string apiUrl = WfeUrl + "WFE";
            string inputJson = "";
            try
            {
                var inputformytask = new
                {
                    AD_ID = adid,
                    globalMasterUrl = MasterUsrl
                };
                inputJson = (new JavaScriptSerializer()).Serialize(inputformytask);
                HttpClient client = new HttpClient();
                HttpContent inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");
                HttpResponseMessage responseformytask = client.PostAsync(apiUrl + "/getMyTaskDtlData", inputContent).Result;
                List<WorkQueueItems> outputMyTask = (new JavaScriptSerializer()).Deserialize<List<WorkQueueItems>>(responseformytask.Content.ReadAsStringAsync().Result);

                var Objqitem = outputMyTask.ToList();

                foreach (var item in Objqitem)
                {
                    WorkQueueItems wqitem = new WorkQueueItems();
                    wqitem.EMPLOYEE_NAME = item.EMP_NAME + '-' + item.AD_ID;
                    wqitem.PAGE = item.PAGE;
                    wqitem.PK_TRANSID = item.PK_TRANSID;
                    wqitem.PK_PROCESSID = item.PK_PROCESSID;
                    wqitem.PROCESS_NAME = item.PROCESS_NAME;
                    wqitem.INSTANCE_ID = item.INSTANCE_ID;
                    wqitem.STEP_NAME = item.STEP_NAME;
                    wqitem.FK_STEPID = Convert.ToInt32(item.FK_STEPID);
                    wqitem.HEADER_INFO = item.HEADER_INFO;

                    wqitem.ASSIGN_DATE = String.Format("{0:dd-MMM-yyyy H:mm:ss}", Convert.ToDateTime(item.ASSIGN_DATE));//.ToString("dd-MMM-yyyy");
                    wqitem.TARGET_DATE = String.Format("{0:dd-MMM-yyyy H:mm:ss}", Convert.ToDateTime(item.TARGET_DATE));//.ToString("dd-MMM-yyyy");
                    wqitem.INITIATORNAME = item.INITIATORNAME;
                    wqitem.Action = item.Action;
                    
                    lstWorkQueueItems.Add(wqitem);
                }
            }
            catch (Exception ex)
            {
                Logger1.Error(ex);
            }
            return lstWorkQueueItems;
        }
        public static List<audittrail> getAudit(string processId, string instanceId)
        {
            List<audittrail> auditData = new List<audittrail>();
            GlobleMasterDataContext context = new GlobleMasterDataContext();

            var expense = (from eh in context.View_1s
                           where eh.FK_PROCESS_ID == Convert.ToInt32(processId)
                           && eh.FK_INSTANCE_ID == Convert.ToInt32(instanceId)
                           orderby eh.PK_AUDIT_ID descending
                           select new
                           {
                               eh.PK_AUDIT_ID,
                               eh.STEPNAME,
                               // eh.PERFORMERTYPE,
                               ACTIONBYUSER = eh.ACTIONBYEMPNAME,
                               eh.ACTIONDATE,
                               eh.ACTION,
                               eh.REMARK,
                               eh.TAT
                           }).ToList();

            for (int i = 0; i < expense.Count; i++)
            {
                audittrail objExpense = new audittrail();
                objExpense.PK_ID = expense[i].PK_AUDIT_ID;
                objExpense.TAT = Convert.ToInt32(expense[i].TAT);
                objExpense.STEPNAME = new CultureInfo("en").TextInfo.ToTitleCase(expense[i].STEPNAME.ToLower());
                // objExpense.PERFORMERTYPE = new CultureInfo("en").TextInfo.ToTitleCase(expense[i].PERFORMERTYPE.ToLower());
                objExpense.ACTIONBYUSER = new CultureInfo("en").TextInfo.ToTitleCase(expense[i].ACTIONBYUSER.ToLower());
                objExpense.ACTIONDATE = String.Format("{0:dd-MMM-yyyy H:mm:ss}", Convert.ToDateTime(expense[i].ACTIONDATE));//.ToString("dd-MMM-yyyy");
                objExpense.ACTION = new CultureInfo("en").TextInfo.ToTitleCase(expense[i].ACTION.ToLower());
                objExpense.REMARK = new CultureInfo("en").TextInfo.ToTitleCase(expense[i].REMARK.ToLower());

                auditData.Add(objExpense);
            }
            return auditData;
        }
        public static List<DocMaster> getDocumentData(string requestNo)
        {
            List<DocMaster> documentData = new List<DocMaster>();
            GlobleMasterDataContext context = new GlobleMasterDataContext();
            var Docs = (from d in context.M_DOCUMENTs
                        where d.OBJECT_VALUE == requestNo
                        select new
                        {
                            d.PK_ID,
                            d.OBJECT_VALUE,
                            d.FILENAME,
                            d.DOCUMENT_TYPE,
                            d.OBJECT_NAME
                        }).ToList();
            for (int i = 0; i < Docs.Count; i++)
            {
                DocMaster objExpense = new DocMaster();
                objExpense.OBJECT_VALUE = Docs[i].OBJECT_VALUE;
                objExpense.FILENAME = Docs[i].FILENAME;
                objExpense.DOCUMENT_TYPE = Docs[i].DOCUMENT_TYPE;
                objExpense.OBJECT_TYPE = Docs[i].OBJECT_NAME;
                objExpense.PK_ID = Docs[i].PK_ID;
                documentData.Add(objExpense);
            }
            return documentData;
        }

        public static List<FlowApproval> getFlowodDealNote(int Deptid,int requestforid)
        {
            List<FlowApproval> Flowdata = new List<FlowApproval>();
            GlobleMasterDataContext context = new GlobleMasterDataContext();

            //if (requestforid == 3)
            //{
                var objappr11 = (from fi in context.M_ACCESS_ROLEs
                                 join me in context.M_ACCESS_USR_ROLE_MAPs on fi.PK_ROLE_ID equals me.ACCESS_ROLE_ID
                                 join em in context.M_DEPARTMENTs on me.USER_ADID equals em.DEPARTMENT_HEAD
                                 where me.IS_ACTIVE == 1 && fi.IS_ACTIVE == 1 && em.PK_DEPARTMENT_ID == Deptid //10011
                                 select new
                                 {
                                     APPR_ID = me.USER_ADID,
                                     me.EMP_NAME,
                                     me.EMAILID,
                                     em.DEPARTMENT_NAME

                                 }).Distinct().ToList();
                for (int i = 0; i < objappr11.Count; i++)
                {
                    FlowApproval objfl = new FlowApproval();
                    objfl.DEPARTMENT = objappr11[i].DEPARTMENT_NAME;
                    objfl.APPRNAME = objappr11[i].EMP_NAME;
                    objfl.STEPNAME = "HOD Approval";
                    Flowdata.Add(objfl);
                }
           // }
            

            var objappr112 = (from fi in context.M_ACCESS_ROLEs
                              join me in context.M_ACCESS_USR_ROLE_MAPs on fi.PK_ROLE_ID equals me.ACCESS_ROLE_ID
                              //join em in context.M_EMPLOYEEs on me.USER_ADID equals em.AD_ID
                              where me.IS_ACTIVE == 1 && fi.IS_ACTIVE == 1 && me.ACCESS_ROLE_ID == 7//10011
                              select new
                              {
                                  APPR_ID = me.USER_ADID,
                                  me.EMP_NAME,
                                  me.EMAILID

                              }).Distinct().ToList();
            for (int i = 0; i < objappr112.Count; i++)
            {
                FlowApproval objfl = new FlowApproval();
                objfl.DEPARTMENT = "";
                objfl.APPRNAME = objappr112[i].EMP_NAME;
                objfl.STEPNAME = "Finance Team";
                Flowdata.Add(objfl);
            }

            var objappr113 = (from fi in context.M_ACCESS_ROLEs
                              join me in context.M_ACCESS_USR_ROLE_MAPs on fi.PK_ROLE_ID equals me.ACCESS_ROLE_ID
                             // join em in context.M_EMPLOYEEs on me.USER_ADID equals em.AD_ID
                              where me.IS_ACTIVE == 1 && fi.IS_ACTIVE == 1 && me.ACCESS_ROLE_ID == 8//10011
                              select new
                              {
                                  APPR_ID = me.USER_ADID,
                                  me.EMP_NAME,
                                  me.EMAILID

                              }).Distinct().ToList();
            for (int i = 0; i < objappr113.Count; i++)
            {
                FlowApproval objfl = new FlowApproval();
                objfl.DEPARTMENT = "";
                objfl.APPRNAME = objappr113[i].EMP_NAME;
                objfl.STEPNAME = "CFO Approval";
                Flowdata.Add(objfl);
            }

            var objappr114 = (from fi in context.M_ACCESS_ROLEs
                              join me in context.M_ACCESS_USR_ROLE_MAPs on fi.PK_ROLE_ID equals me.ACCESS_ROLE_ID
                              //join em in context.M_EMPLOYEEs on me.USER_ADID equals em.AD_ID
                              where me.IS_ACTIVE == 1 && fi.IS_ACTIVE == 1 && me.ACCESS_ROLE_ID == 9//10011
                              select new
                              {
                                  APPR_ID = me.USER_ADID,
                                  me.EMP_NAME,
                                  me.EMAILID

                              }).Distinct().ToList();
            for (int i = 0; i < objappr114.Count; i++)
            {
                FlowApproval objfl = new FlowApproval();
                objfl.DEPARTMENT = "";
                objfl.APPRNAME = objappr114[i].EMP_NAME;
                objfl.STEPNAME = "CEO Approval";
                Flowdata.Add(objfl);
            }

            var objappr115 = (from fi in context.M_ACCESS_ROLEs
                              join me in context.M_ACCESS_USR_ROLE_MAPs on fi.PK_ROLE_ID equals me.ACCESS_ROLE_ID
                              // join em in context.M_EMPLOYEEs on me.USER_ADID equals em.AD_ID
                              where me.IS_ACTIVE == 1 && fi.IS_ACTIVE == 1 && me.ACCESS_ROLE_ID == 10//10011
                              select new
                              {
                                  APPR_ID = me.USER_ADID,
                                  me.EMP_NAME,
                                  me.EMAILID

                              }).Distinct().ToList();
            for (int i = 0; i < objappr115.Count; i++)
            {
                FlowApproval objfl = new FlowApproval();
                objfl.DEPARTMENT = "";
                objfl.APPRNAME = objappr115[i].EMP_NAME;
                objfl.STEPNAME = "OPS and Finance Team";
                Flowdata.Add(objfl);
            }
            var objappr116 = (from fi in context.M_ACCESS_ROLEs
                              join me in context.M_ACCESS_USR_ROLE_MAPs on fi.PK_ROLE_ID equals me.ACCESS_ROLE_ID
                              // join em in context.M_EMPLOYEEs on me.USER_ADID equals em.AD_ID
                              where me.IS_ACTIVE == 1 && fi.IS_ACTIVE == 1 && me.ACCESS_ROLE_ID == 11//10011
                              select new
                              {
                                  APPR_ID = me.USER_ADID,
                                  me.EMP_NAME,
                                  me.EMAILID

                              }).Distinct().ToList();
            for (int i = 0; i < objappr116.Count; i++)
            {
                FlowApproval objfl = new FlowApproval();
                objfl.DEPARTMENT = "";
                objfl.APPRNAME = objappr116[i].EMP_NAME;
                objfl.STEPNAME = "SMS Team";
                Flowdata.Add(objfl);
            }

            return Flowdata;
        }
        public static bool InsertAuditTrail(int processid, int instanceid, string stepname, string performertype, string actionbyuser, string action, string remark, string actdate)
        {
            bool flag = false;
            using (GlobleMasterDataContext db = new GlobleMasterDataContext())
            {
                try
                {
                    M_AUDITTRAIL audit = new M_AUDITTRAIL();
                    audit.PROCESSID = processid;
                    audit.INSTANCEID = instanceid;
                    audit.STEPNAME = stepname;
                    audit.PERFORMERTYPE = performertype;
                    audit.ACTIONBYUSER = actionbyuser;
                    audit.ACTIONDATE = DateTime.Now;
                    audit.ACTION = action;
                    audit.REMARK = remark;
                    db.M_AUDITTRAILs.InsertOnSubmit(audit);
                    db.SubmitChanges();
                    flag = true;
                    return flag;
                }
                catch (Exception ex)
                {
                    Logger1.Error(ex);
                    return false;
                    throw ex;
                }
            }
        }
        public static bool SendMail(int processid, int instanceid, int fkwiid, string stepname, string action, string headerinfo, string ExitActionMailSubject, string ExitActionMailId, string ExitActionMailCC, string ExitActionMailBody)
        {
            bool flag = false;
            using (GlobleMasterDataContext db = new GlobleMasterDataContext())
            {
                try
                {
                    T_MAILLOG mail = new T_MAILLOG();
                    mail.FK_PROCESSID = processid;
                    mail.FK_INSTANCEID = instanceid;
                    mail.FK_WIID = fkwiid;
                    mail.STEP_NAME = stepname;
                    mail.ACTION = action;
                    mail.HEADER = ExitActionMailSubject;
                    mail.TO_MAIL = ExitActionMailId;
                    mail.CC_MAIL = ExitActionMailCC;
                    mail.MAIL_BODY = ExitActionMailBody;
                    mail.MAILSENT_FLAG = Convert.ToBoolean(1);
                    mail.CREATION_DATE = DateTime.Now;
                    db.T_MAILLOGs.InsertOnSubmit(mail);
                    db.SubmitChanges();
                    flag = true;
                    return flag;
                }
                catch (Exception ex)
                {
                    Logger1.Error(ex);
                    return false;
                }
            }
        }


    }
    public class FlowApproval
    {

        public string STEPNAME { get; set; }
        public string SEQNO { get; set; }
        public string DEPARTMENT { get; set; }
        public string INVCTYPE { get; set; }
        public string APPRID { get; set; }
        public int FK_DEPT_ID { get; set; }
        public int FK_NOE_ID { get; set; }
        public string EMAILID { get; set; }
        public string APPRNAME { get; set; }
        public string Reqinitiator { get; set; }
        public string id { get; set; }
        public string label { get; set; }
        public double invcamount { get; set; }
        public int FK_IT_INVC_TYPE { get; set; }

        public int FK_REQUEST_FOR_ID { get; set; }
    }

    public class DocMaster
    {
        public int FK_ACTIVITY_ID { get; set; }
        public int FK_DOCUMENT_TYPE { get; set; }
        public string OBJECT_TYPE { get; set; }
        public string OBJECT_VALUE { get; set; }
        public string FILENAME { get; set; }
        public int PK_ID { get; set; }
        public string ROWNAME { get; set; }
        public string FILESIZE { get; set; }
        public string DOCUMENT_TYPE { get; set; }
        public string PATH { get; set; }
    }
    public class employeeMST
    {
        public int PK_EMP_ID { get; set; }
        public string AD_ID { get; set; }

        public string EMP_NAME { get; set; }
        public string EMP_CODE { get; set; }
        public string EMAILID { get; set; }

        public int FK_BRANCH_ID { get; set; }
        public string branch { get; set; }

        public string ADMINREPORTING_TO { get; set; }
        public string RmEmpName { get; set; }
        public string RmMailid { get; set; }

        public string Bh_ADID { get; set; }
        public string BhEmpName { get; set; }
        public string BhMailid { get; set; }

        public string Ba_ADID { get; set; }
        public string BaEmpName { get; set; }
        public string BaMailid { get; set; }

        public string GroupName { get; set; }
    }
    public class WorkQueueItems
    {
        public string EMPLOYEE_NAME { get; set; }
        public string AD_ID { get; set; }
        public string PAGE { get; set; }
        public int PK_TRANSID { get; set; }

        public int PK_PROCESSID { get; set; }
        public string PROCESS_NAME { get; set; }
        public int INSTANCE_ID { get; set; }
        public string STEP_NAME { get; set; }
        public string ACCESS_ROLE_NAME { get; set; }
        public int FK_STEPID { get; set; }
        public string HEADER_INFO { get; set; }
        public string ASSIGN_DATE { get; set; }
        public string TARGET_DATE { get; set; }

        public string EMP_NAME { get; set; }

        public string MasterApi { get; set; }
        public string WfeApi { get; set; }

        public string INITIATORNAME { get; set; }
        public string INITIATORADID { get; set; }

        public string Action { get; set; }
    }
    public class audittrail
    {
        public int PK_ID { get; set; }
        public string STEPNAME { get; set; }
        public string PERFORMERTYPE { get; set; }
        public string ACTIONBYUSER { get; set; }
        public string ACTIONDATE { get; set; }
        public string ACTION { get; set; }
        public string REMARK { get; set; }
        public string PROCESSID { get; set; }
        public string INSTANCEID { get; set; }
        public string id { get; set; }
        public string label { get; set; }
        public string EMAILID { get; set; }
        public string DURATION { get; set; }
        public int TAT { get; set; }
    }
    public class AuditMaster
    {
        public int FK_PROCESSID { get; set; }
        public int INSTANCEID { get; set; }
        public string STEPNAME { get; set; }
        public string PERFORMERTYPE { get; set; }
        public string ACTIONBYUSR { get; set; }
        public string ACTION { get; set; }
        public string REMARK { get; set; }
        public string ACTIONDATE { get; set; }

    }
    public class SendMailMaster
    {
        public int FK_PROCESSID { get; set; }
        public int INSTANCEID { get; set; }
        public string STEPID { get; set; }
        public string STEPNAME { get; set; }
        public string ACTION { get; set; }
        public string USRNAME { get; set; }
        public string USREMAIL { get; set; }
        public string HEADERINFO { get; set; }
        public string[] Approver { get; set; }
        public string REMARK { get; set; }
        public string ExitActionMailSubject { get; set; }
        public string ExitActionMailId { get; set; }
        public string ExitActionMailCC { get; set; }
        public string ExitActionMailBody { get; set; }
        public int FK_WIID { get; set; }
    }

}
