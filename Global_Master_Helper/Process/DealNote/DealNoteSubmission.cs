using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Global_Master_Helper.Process.DealNote
{
    public class DealNoteSubmission
    {
        public static string SaveData(List<DealNoteHdr> objHdrDetail, List<DocMaster> objDocDetail, string InitiatorId, string[] Dval, string nextAppEmailID, string Req_Email, string action, string Master_API, string WFE_API, int NextSeqNo, string portalurl, string hdrStatus,string EMP_NAME)
        {
            string Result = "";

            using (GlobleMasterDataContext context = new GlobleMasterDataContext())
            {
                try
                {

                    context.Connection.Open();
                    context.Transaction = context.Connection.BeginTransaction();

                    T_DEALNOTE_HDR check = (from c in context.T_DEALNOTE_HDRs where c.FK_REQUESTFOR_ID == objHdrDetail[0].FK_REQUESTFOR_ID && c.FK_DEPT_ID == objHdrDetail[0].FK_DEPT_ID && c.FK_CAT_ID == objHdrDetail[0].FK_CAT_ID && c.FK_SUBCAT_ID == objHdrDetail[0].FK_SUBCAT_ID && c.AMOUNT==objHdrDetail[0].AMOUNT select c).FirstOrDefault();
                    // Convert.ToDateTime(c.VALIDTILL_DATE).Date == Convert.ToDateTime(objHdrDetail[0].VALIDTILL_DATE).Date
                    if (check == null)
                    {

                        string nxtstepid = "";

                        string apiUrl = WFE_API + "WFE";
                        var inputforinstance = new
                        {
                            FK_PROCESSID = 2,
                            USRNAME = InitiatorId,
                            USREMAIL = Req_Email,
                            STEPID = 1,
                        };
                        string inputJson = (new JavaScriptSerializer()).Serialize(inputforinstance);
                        HttpClient client = new HttpClient();
                        HttpContent inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");
                        HttpResponseMessage responseinstnceid = client.PostAsync(apiUrl + "/InstanceID", inputContent).Result;

                        string instanceid = "";

                        instanceid = responseinstnceid.Content.ReadAsStringAsync().Result.ToString();
                        instanceid = instanceid.Replace("\"", "");

                        string RequestNumber = "";
                        context.GETREQUESTNO("DN", ref RequestNumber);
                        //context.GENERATE_REQUEST_ID1(Convert.ToDateTime(objDtlDetail[0].REQ_DATE), "PO", ref RequestNumber);

                        T_DEALNOTE_HDR Hdr = new T_DEALNOTE_HDR();

                        Hdr.FK_PROCESS_ID = 2;
                        Hdr.FK_INSTANCE_ID = Convert.ToInt32(instanceid);
                        Hdr.DNREQUEST_NO = RequestNumber;
                        Hdr.CREATED_DATE = DateTime.Now;
                        Hdr.CREATED_BY = objHdrDetail[0].CREATED_BY;

                        Hdr.FK_DEPT_ID = objHdrDetail[0].FK_DEPT_ID;
                        Hdr.FK_REQUESTFOR_ID = objHdrDetail[0].FK_REQUESTFOR_ID;
                        Hdr.FK_CAT_ID = objHdrDetail[0].FK_CAT_ID;
                        Hdr.FK_SUBCAT_ID = objHdrDetail[0].FK_SUBCAT_ID;
                        Hdr.FK_CURRENCY = objHdrDetail[0].FK_CURRENCY;

                        Hdr.REMARK = objHdrDetail[0].REMARK;
                        Hdr.SHORT_DESC = objHdrDetail[0].SHORT_DESC;
                        //  Hdr.STATUS = objHdrDetail[0].STATUS;
                        Hdr.STATUS = hdrStatus;
                        // Hdr.NEXT_APPR_SEQ = 1;
                        //Hdr.ROLLOUT_DATE = Convert.ToDateTime(objHdrDetail[0].ROLLOUT_DATE);
                        //  Hdr.VALIDTILL_DATE = Convert.ToDateTime(objHdrDetail[0].VALIDTILL_DATE);
                        Hdr.VALIDTILL_DATE = objHdrDetail[0].VALIDTILL_DATE;
                        Hdr.AMOUNT = objHdrDetail[0].AMOUNT;
                        Hdr.DESC_FOR_REQUEST = objHdrDetail[0].DESC_FOR_REQUEST;

                        Hdr.Business_Justification = objHdrDetail[0].Business_Justification;
                        Hdr.Recommendation = objHdrDetail[0].Recommendation;
                        Hdr.Imp_Timelines = objHdrDetail[0].Imp_Timelines;
                        Hdr.EMAILID = objHdrDetail[0].EMAILID;
                        Hdr.FK_REGION_ID = objHdrDetail[0].FK_REGION_ID;
                        Hdr.FK_DIVISION_ID = objHdrDetail[0].FK_DIVISION_ID;

                        context.T_DEALNOTE_HDRs.InsertOnSubmit(Hdr);
                        context.SubmitChanges();


                        for (int i = 0; i < objDocDetail.Count; i++)
                        {
                            M_DOCUMENT docObj = new M_DOCUMENT();
                            docObj.OBJECT_NAME = objDocDetail[i].OBJECT_TYPE;
                            docObj.OBJECT_VALUE = RequestNumber;
                            docObj.DOCUMENT_TYPE = objDocDetail[i].DOCUMENT_TYPE;
                            docObj.FILENAME = objDocDetail[i].FILENAME;
                            docObj.PATH = objDocDetail[i].PATH;
                            context.M_DOCUMENTs.InsertOnSubmit(docObj);
                            context.SubmitChanges();
                        }


                        string stepname = "DEALNOTE SUBMISSION";
                        var inputforreleaseid = new
                        {
                            FK_PROCESSID = 2,
                            STEPNAME = stepname,
                            ACTION = action,
                        };
                        inputJson = (new JavaScriptSerializer()).Serialize(inputforreleaseid);
                        client = new HttpClient();
                        inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");
                        HttpResponseMessage responseforreleaseid = client.PostAsync(apiUrl + "/ReleaseID", inputContent).Result;

                        nxtstepid = responseforreleaseid.Content.ReadAsStringAsync().Result.ToString();
                        nxtstepid = nxtstepid.Replace("\"", "");

                        List<employeeMST> objemp = new List<employeeMST>();
                        string[] NxtDval = new string[1];
                        string SkipnextAppEmailID = string.Empty;
                        if (Dval.Length > 0)
                        {

                            // *********
                            var inputforreleasestep = new
                            {
                                FK_PROCESSID = 2,
                                INSTANCEID = Convert.ToInt32(instanceid),
                                STEPID = Convert.ToInt32(nxtstepid),
                                USRNAME = InitiatorId,
                                USREMAIL = Req_Email,
                                HEADERINFO = RequestNumber,
                                Approver = Dval,
                                REMARK = objHdrDetail[0].REMARK,
                                ExitActionMailSubject = "DEAL NOTE",
                                ExitActionMailId = nextAppEmailID,
                                ExitActionMailCC = Req_Email,
                                ExitActionMailBody = "<html><body><h4 style='margin: 20px 0px 0px; font-family: Karla, sans-serif; background-color: rgb(255, 255, 255);'>Dear Sir/Madam,</h4><div style='font-family: Arial, Helvetica, sans-serif; font-size: 12px; background-color: rgb(255, 255, 255);'>&nbsp;</div><div style='font-family: Arial, Helvetica, sans-serif; font-size: 12px; background-color: rgb(255, 255, 255);'>The<b>&nbsp;</b><span style='font-family: Karla, sans-serif;'><b>Request Number: " + RequestNumber + "</b>&nbsp;is pending for your approval.</span></div><div style='font-family: Arial, Helvetica, sans-serif; font-size: 12px; background-color: rgb(255, 255, 255);'></p>You may click on below link to reach BTS Portal for further desired action. </pre><pre><h3 style='margin: 0px; font-family: Karla, sans-serif;'>URL: <a href='" + portalurl + "' style='color: rgb(17, 85, 204);' target='_blank'>" + portalurl + "</a></h3></p><h3 style='margin: 0px; font-family: Karla, sans-serif; color: rgb(238, 38, 38);'><span style='color:#FF0000;'><span class='im'>This is a system generated mail. Please do not reply to this mail</span></span></h3></div></body></html>",
                                MasterUrl = Master_API,
                                ProcessUrl = Master_API,
                                WfeUrl = WFE_API,
                                EMP_NAME=EMP_NAME

                            };
                            inputJson = (new JavaScriptSerializer()).Serialize(inputforreleasestep);
                            client = new HttpClient();
                            inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");
                            HttpResponseMessage responserelasestep = client.PostAsync(apiUrl + "/ReleaseStep", inputContent).Result;

                            string relno = "";

                            relno = responserelasestep.Content.ReadAsStringAsync().Result.ToString();
                            relno = relno.Replace("\"", "");

                            Result = RequestNumber;

                            //  }

                        }
                    }
                    else
                    {
                        Result = "Duplicate DealNote Request";
                    }
                    context.Transaction.Commit();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.InnerException.Message);
                    context.Transaction.Rollback();
                    Logger1.Error(ex);
                    Result = AppConstants.ErrorMessage;
                    throw ex;
                }
            }
            return Result;

        }

        public static string SaveAsDraftData(List<DealNoteHdr> objHdrDetail, List<DocMaster> objDocDetail, string InitiatorId, string[] Dval, string nextAppEmailID, string Req_Email, string action, string Master_API, string WFE_API, int NextSeqNo, string portalurl, string hdrStatus,string EMP_NAME)
        {
            string Result = "";

            using (GlobleMasterDataContext context = new GlobleMasterDataContext())
            {
                try
                {

                    context.Connection.Open();
                    context.Transaction = context.Connection.BeginTransaction();

                    T_DEALNOTE_HDR check = (from c in context.T_DEALNOTE_HDRs where c.FK_REQUESTFOR_ID == objHdrDetail[0].FK_REQUESTFOR_ID && c.FK_DEPT_ID == objHdrDetail[0].FK_DEPT_ID && c.FK_CAT_ID == objHdrDetail[0].FK_CAT_ID && c.FK_SUBCAT_ID == objHdrDetail[0].FK_SUBCAT_ID && c.AMOUNT == objHdrDetail[0].AMOUNT select c).FirstOrDefault();
                    if (check == null)
                    {

                        string nxtstepid = "";

                        string apiUrl = WFE_API + "WFE";
                        var inputforinstance = new
                        {
                            FK_PROCESSID = 2,
                            USRNAME = InitiatorId,
                            USREMAIL = Req_Email,
                            STEPID = 1,
                        };
                        string inputJson = (new JavaScriptSerializer()).Serialize(inputforinstance);
                        HttpClient client = new HttpClient();
                        HttpContent inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");
                        HttpResponseMessage responseinstnceid = client.PostAsync(apiUrl + "/InstanceID", inputContent).Result;

                        string instanceid = "";

                        instanceid = responseinstnceid.Content.ReadAsStringAsync().Result.ToString();
                        instanceid = instanceid.Replace("\"", "");

                        string RequestNumber = "";
                        context.GETREQUESTNO("DN", ref RequestNumber);

                        T_DEALNOTE_HDR Hdr = new T_DEALNOTE_HDR();

                        Hdr.FK_PROCESS_ID = 2;
                        Hdr.FK_INSTANCE_ID = Convert.ToInt32(instanceid);
                        Hdr.DNREQUEST_NO = RequestNumber;
                        Hdr.CREATED_DATE = DateTime.Now;
                        Hdr.CREATED_BY = objHdrDetail[0].CREATED_BY;

                        Hdr.FK_DEPT_ID = objHdrDetail[0].FK_DEPT_ID;
                        Hdr.FK_REQUESTFOR_ID = objHdrDetail[0].FK_REQUESTFOR_ID;
                        Hdr.FK_CAT_ID = objHdrDetail[0].FK_CAT_ID;
                        Hdr.FK_SUBCAT_ID = objHdrDetail[0].FK_SUBCAT_ID;
                        Hdr.FK_CURRENCY = objHdrDetail[0].FK_CURRENCY;

                        Hdr.REMARK = objHdrDetail[0].REMARK;
                        Hdr.SHORT_DESC = objHdrDetail[0].SHORT_DESC;
                        //  Hdr.STATUS = objHdrDetail[0].STATUS;
                        Hdr.STATUS = hdrStatus;
                        // Hdr.NEXT_APPR_SEQ = 1;
                        // Hdr.ROLLOUT_DATE = Convert.ToDateTime(objHdrDetail[0].ROLLOUT_DATE);
                        //Hdr.VALIDTILL_DATE = Convert.ToDateTime(objHdrDetail[0].VALIDTILL_DATE);
                        Hdr.VALIDTILL_DATE = (objHdrDetail[0].VALIDTILL_DATE);
                        Hdr.AMOUNT = objHdrDetail[0].AMOUNT;
                        Hdr.DESC_FOR_REQUEST = objHdrDetail[0].DESC_FOR_REQUEST;
                        Hdr.Business_Justification = objHdrDetail[0].Business_Justification;
                        Hdr.Recommendation = objHdrDetail[0].Recommendation;
                        Hdr.Imp_Timelines = objHdrDetail[0].Imp_Timelines;
                        Hdr.EMAILID = objHdrDetail[0].EMAILID;
                        Hdr.FK_REGION_ID = objHdrDetail[0].FK_REGION_ID;
                        Hdr.FK_DIVISION_ID = objHdrDetail[0].FK_DIVISION_ID;

                        context.T_DEALNOTE_HDRs.InsertOnSubmit(Hdr);
                        context.SubmitChanges();


                        for (int i = 0; i < objDocDetail.Count; i++)
                        {
                            M_DOCUMENT docObj = new M_DOCUMENT();
                            var filename = (from c in context.M_DOCUMENTs where c.FILENAME == objDocDetail[i].FILENAME && c.DOCUMENT_TYPE == objDocDetail[i].DOCUMENT_TYPE select c).FirstOrDefault();
                            if (filename == null)
                            {
                                docObj.OBJECT_NAME = objDocDetail[i].OBJECT_TYPE;
                                docObj.OBJECT_VALUE = RequestNumber;
                                docObj.DOCUMENT_TYPE = objDocDetail[i].DOCUMENT_TYPE;
                                docObj.FILENAME = objDocDetail[i].FILENAME;
                                docObj.PATH = objDocDetail[i].PATH;
                                context.M_DOCUMENTs.InsertOnSubmit(docObj);
                                context.SubmitChanges();
                            }
                        }

                        //for (int i = 0; i < objDocDetail.Count; i++)
                        //{
                        //    M_DOCUMENT docObj = new M_DOCUMENT();
                        //    var filename = (from c in context.M_DOCUMENTs where c.FILENAME == objDocDetail[i].FILENAME && c.DOCUMENT_TYPE == objDocDetail[i].DOCUMENT_TYPE select c).FirstOrDefault();
                        //    if (filename == null)
                        //    {
                        //        docObj.OBJECT_NAME = objDocDetail[i].OBJECT_TYPE;
                        //        docObj.OBJECT_VALUE = RequestNumber;
                        //        docObj.DOCUMENT_TYPE = objDocDetail[i].DOCUMENT_TYPE;
                        //        docObj.FILENAME = objDocDetail[i].FILENAME;
                        //        docObj.PATH = objDocDetail[i].PATH;
                        //        context.M_DOCUMENTs.InsertOnSubmit(docObj);
                        //        context.SubmitChanges();
                        //    }
                        //}


                        string stepname = "DEALNOTE SUBMISSION";
                        var inputforreleaseid = new
                        {
                            FK_PROCESSID = 2,
                            STEPNAME = stepname,
                            ACTION = "SAVEASDRAFT",
                        };
                        inputJson = (new JavaScriptSerializer()).Serialize(inputforreleaseid);
                        client = new HttpClient();
                        inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");
                        HttpResponseMessage responseforreleaseid = client.PostAsync(apiUrl + "/ReleaseID", inputContent).Result;

                        nxtstepid = responseforreleaseid.Content.ReadAsStringAsync().Result.ToString();
                        nxtstepid = nxtstepid.Replace("\"", "");

                        List<employeeMST> objemp = new List<employeeMST>();
                        string[] NxtDval = new string[1];
                        string SkipnextAppEmailID = string.Empty;
                        if (Dval.Length > 0)
                        {

                            // *********
                            var inputforreleasestep = new
                            {
                                FK_PROCESSID = 2,
                                INSTANCEID = Convert.ToInt32(instanceid),
                                STEPID = Convert.ToInt32(nxtstepid),
                                USRNAME = InitiatorId,
                                USREMAIL = Req_Email,
                                HEADERINFO = RequestNumber,
                                Approver = Dval,
                                REMARK = objHdrDetail[0].REMARK,
                                ExitActionMailSubject = "Deal Note",
                                ExitActionMailId = nextAppEmailID,
                                ExitActionMailCC = Req_Email,
                                ExitActionMailBody = "<html><body><h4 style='margin: 20px 0px 0px; font-family: Karla, sans-serif; background-color: rgb(255, 255, 255);'>Dear Sir/Madam,</h4><div style='font-family: Arial, Helvetica, sans-serif; font-size: 12px; background-color: rgb(255, 255, 255);'>&nbsp;</div><div style='font-family: Arial, Helvetica, sans-serif; font-size: 12px; background-color: rgb(255, 255, 255);'>The<b>&nbsp;</b><span style='font-family: Karla, sans-serif;'><b>Request Number: " + RequestNumber + "</b>&nbsp;is pending for your approval.</span></div><div style='font-family: Arial, Helvetica, sans-serif; font-size: 12px; background-color: rgb(255, 255, 255);'></p>You may click on below link to reach BTS Portal for further desired action. </pre><pre><h3 style='margin: 0px; font-family: Karla, sans-serif;'>URL: <a href='" + portalurl + "' style='color: rgb(17, 85, 204);' target='_blank'>" + portalurl + "</a></h3></p><h3 style='margin: 0px; font-family: Karla, sans-serif; color: rgb(238, 38, 38);'><span style='color:#FF0000;'><span class='im'>This is a system generated mail. Please do not reply to this mail</span></span></h3></div></body></html>",
                                MasterUrl = Master_API,
                                ProcessUrl = Master_API,
                                WfeUrl = WFE_API,
                                EMP_NAME = EMP_NAME
                            };
                            inputJson = (new JavaScriptSerializer()).Serialize(inputforreleasestep);
                            client = new HttpClient();
                            inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");
                            HttpResponseMessage responserelasestep = client.PostAsync(apiUrl + "/ReleaseStep", inputContent).Result;

                            string relno = "";

                            relno = responserelasestep.Content.ReadAsStringAsync().Result.ToString();
                            relno = relno.Replace("\"", "");

                            Result = RequestNumber;

                            //  }

                        }
                    }
                    else
                    {
                        Result = "Duplicate DealNote Request";
                    }
                    context.Transaction.Commit();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.InnerException.Message);
                    context.Transaction.Rollback();
                    Logger1.Error(ex);
                    Result = AppConstants.ErrorMessage;
                    throw ex;
                }
            }
            return Result;

        }

        public static string UpdateData(List<DealNoteHdr> objHdrDetail, List<DocMaster> objDocDetail, string InitiatorId, string[] Dval, string nextAppEmailID, string Req_Email, string action, string Master_API, string WFE_API, int NextSeqNo, string portalurl, string hdrStatus,string EMP_NAME)
        {
            string Result = "";

            using (GlobleMasterDataContext context = new GlobleMasterDataContext())
            {
                try
                {

                    context.Connection.Open();
                    context.Transaction = context.Connection.BeginTransaction();

                    string nxtstepid = "";

                    string apiUrl = WFE_API + "WFE";

                    var Hdr = context.T_DEALNOTE_HDRs.Single(x => x.DNREQUEST_NO == objHdrDetail[0].DNREQUEST_NO);
                    Hdr.CREATED_DATE = DateTime.Now;
                    Hdr.CREATED_BY = objHdrDetail[0].CREATED_BY;

                    Hdr.FK_DEPT_ID = objHdrDetail[0].FK_DEPT_ID;
                    Hdr.FK_REQUESTFOR_ID = objHdrDetail[0].FK_REQUESTFOR_ID;
                    Hdr.FK_CAT_ID = objHdrDetail[0].FK_CAT_ID;
                    Hdr.FK_SUBCAT_ID = objHdrDetail[0].FK_SUBCAT_ID;
                    Hdr.FK_CURRENCY = objHdrDetail[0].FK_CURRENCY;

                    Hdr.REMARK = objHdrDetail[0].REMARK;
                    Hdr.SHORT_DESC = objHdrDetail[0].SHORT_DESC;
                    //  Hdr.STATUS = objHdrDetail[0].STATUS;
                    Hdr.STATUS = hdrStatus;
                    // Hdr.NEXT_APPR_SEQ = 1;
                    //  Hdr.ROLLOUT_DATE = Convert.ToDateTime(objHdrDetail[0].ROLLOUT_DATE);
                    //Hdr.VALIDTILL_DATE = Convert.ToDateTime(objHdrDetail[0].VALIDTILL_DATE);
                    Hdr.VALIDTILL_DATE = objHdrDetail[0].VALIDTILL_DATE;
                    Hdr.AMOUNT = objHdrDetail[0].AMOUNT;
                    Hdr.DESC_FOR_REQUEST = objHdrDetail[0].DESC_FOR_REQUEST;
                    Hdr.Business_Justification = objHdrDetail[0].Business_Justification;
                    Hdr.Recommendation = objHdrDetail[0].Recommendation;
                    Hdr.Imp_Timelines = objHdrDetail[0].Imp_Timelines;
                    Hdr.FK_REGION_ID= objHdrDetail[0].FK_REGION_ID;
                    Hdr.FK_DIVISION_ID = objHdrDetail[0].FK_DIVISION_ID;
                    //  context.T_DEALNOTE_HDRs.InsertOnSubmit(Hdr);
                    context.SubmitChanges();


                    for (int i = 0; i < objDocDetail.Count; i++)
                    {
                        M_DOCUMENT docObj = new M_DOCUMENT();
                        var filename = (from c in context.M_DOCUMENTs where c.FILENAME == objDocDetail[i].FILENAME && c.DOCUMENT_TYPE == objDocDetail[i].DOCUMENT_TYPE select c).FirstOrDefault();
                        if (filename == null)
                        {
                            docObj.OBJECT_NAME = objDocDetail[i].OBJECT_TYPE;
                            docObj.OBJECT_VALUE = objHdrDetail[0].DNREQUEST_NO;
                            docObj.DOCUMENT_TYPE = objDocDetail[i].DOCUMENT_TYPE;
                            docObj.FILENAME = objDocDetail[i].FILENAME;
                            docObj.PATH = objDocDetail[i].PATH;
                            context.M_DOCUMENTs.InsertOnSubmit(docObj);
                            context.SubmitChanges();
                        }
                    }

                    string inputJson = "";
                    string stepname = "DEALNOTE MODIFICATION";
                    var inputforreleaseid = new
                    {
                        FK_PROCESSID = 2,
                        STEPNAME = stepname,
                        ACTION = "SUBMIT",
                    };
                    inputJson = (new JavaScriptSerializer()).Serialize(inputforreleaseid);
                    HttpClient client = new HttpClient();
                    HttpContent inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");
                    HttpResponseMessage responseforreleaseid = client.PostAsync(apiUrl + "/ReleaseID", inputContent).Result;

                    nxtstepid = responseforreleaseid.Content.ReadAsStringAsync().Result.ToString();
                    nxtstepid = nxtstepid.Replace("\"", "");

                    List<employeeMST> objemp = new List<employeeMST>();
                    string[] NxtDval = new string[1];
                    string SkipnextAppEmailID = string.Empty;
                    if (Dval.Length > 0)
                    {

                        // *********
                        var inputforreleasestep = new
                        {
                            FK_PROCESSID = 2,
                            INSTANCEID = Convert.ToInt32(objHdrDetail[0].FK_INSTANCE_ID),
                            STEPID = Convert.ToInt32(nxtstepid),
                            USRNAME = InitiatorId,
                            USREMAIL = Req_Email,
                            HEADERINFO = objHdrDetail[0].DNREQUEST_NO,
                            Approver = Dval,
                            REMARK = objHdrDetail[0].REMARK,
                            ExitActionMailSubject = "DEAL NOTE MODIFICATION",
                            ExitActionMailId = nextAppEmailID,
                            ExitActionMailCC = Req_Email,
                            ExitActionMailBody = "<html><body><h4 style='margin: 20px 0px 0px; font-family: Karla, sans-serif; background-color: rgb(255, 255, 255);'>Dear Sir/Madam,</h4><div style='font-family: Arial, Helvetica, sans-serif; font-size: 12px; background-color: rgb(255, 255, 255);'>&nbsp;</div><div style='font-family: Arial, Helvetica, sans-serif; font-size: 12px; background-color: rgb(255, 255, 255);'>The<b>&nbsp;</b><span style='font-family: Karla, sans-serif;'><b>Request Number: " + objHdrDetail[0].DNREQUEST_NO + "</b>&nbsp;is pending for your approval.</span></div><div style='font-family: Arial, Helvetica, sans-serif; font-size: 12px; background-color: rgb(255, 255, 255);'></p>You may click on below link to reach BTS Portal for further desired action. </pre><pre><h3 style='margin: 0px; font-family: Karla, sans-serif;'>URL: <a href='" + portalurl + "' style='color: rgb(17, 85, 204);' target='_blank'>" + portalurl + "</a></h3></p><h3 style='margin: 0px; font-family: Karla, sans-serif; color: rgb(238, 38, 38);'><span style='color:#FF0000;'><span class='im'>This is a system generated mail. Please do not reply to this mail</span></span></h3></div></body></html>",
                            MasterUrl = Master_API,
                            ProcessUrl = Master_API,
                            WfeUrl = WFE_API,
                            EMP_NAME=EMP_NAME
                        };
                        inputJson = (new JavaScriptSerializer()).Serialize(inputforreleasestep);
                        client = new HttpClient();
                        inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");
                        HttpResponseMessage responserelasestep = client.PostAsync(apiUrl + "/ReleaseStep", inputContent).Result;

                        string relno = "";

                        relno = responserelasestep.Content.ReadAsStringAsync().Result.ToString();
                        relno = relno.Replace("\"", "");

                        Result = objHdrDetail[0].DNREQUEST_NO;

                        //  }

                    }
                    
                    context.Transaction.Commit();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.InnerException.Message);
                    context.Transaction.Rollback();
                    Logger1.Error(ex);
                    Result = AppConstants.ErrorMessage;
                    throw ex;
                }
            }
            return Result;

        }

    }
    public class DealNoteHdr
    {
        public int PK_DEALNOTE_HDR_ID { get; set; }
        public int FK_PROCESS_ID { get; set; }
        public int FK_INSTANCE_ID { get; set; }
        public string DNREQUEST_NO { get; set; }
        public int FK_DEPT_ID { get; set; }
        public string SHORT_DESC { get; set; }
        public int FK_REQUESTFOR_ID { get; set; }
        public int FK_CAT_ID { get; set; }
        public int FK_SUBCAT_ID { get; set; }
        public int FK_CURRENCY { get; set; }
        public string ROLLOUT_DATE { get; set; }
        public string VALIDTILL_DATE { get; set; }
        public decimal AMOUNT { get; set; }
        public string DESC_FOR_REQUEST { get; set; }
        public string REMARK { get; set; }
        public string CREATED_BY { get; set; }
        public string CREATED_DATE { get; set; }
        public string STATUS { get; set; }
        public string Business_Justification { get; set; }
        public string Recommendation { get; set; }
        public string Imp_Timelines { get; set; }

        public string EMAILID { get; set; }
        public int FK_REGION_ID { get; set; }
        public int FK_DIVISION_ID { get; set; }

    }
    public class DealNotSavedata
    {

        public string transactiondata { get; set; }
        public string initiatorid { get; set; }
        public string req_mailid { get; set; }
        public string action { get; set; }
        public string Master_API { get; set; }
        public string Tarns_API { get; set; }
        public string WFE_API { get; set; }
        public string DNREQUEST_NO { get; set; }
        public int FK_DEPT_ID { get; set; }
        public string SHORT_DESC { get; set; }
        public int FK_REQUESTFOR_ID { get; set; }
        public int FK_CAT_ID { get; set; }
        public int FK_SUBCAT_ID { get; set; }
        public int FK_CURRENCY { get; set; }
        public string ROLLOUT_DATE { get; set; }
        public string VALIDTILL_DATE { get; set; }
        public decimal AMOUNT { get; set; }
        public string DESC_FOR_REQUEST { get; set; }
        public string REMARK { get; set; }
        public string CREATED_BY { get; set; }
        public string CREATED_DATE { get; set; }
        public string STATUS { get; set; }
        public int instanceid { get; set; }
        public int processid { get; set; }
        public string approver_mail { get; set; }
        public string requester_Mail { get; set; }
        public int NextSeqNo { get; set; }
        public string portalurl { get; set; }
        public string stepName { get; set; }
        public string Requester_ADID { get; set; }
        public int requestid { get; set; }
        
        public string NxtApproverrolename { get; set; }

        public int IsSmsConfig { get; set; }

        public string Business_Justification { get; set; }
        public string Recommendation { get; set; }
        public string Imp_Timelines { get; set; }

        public int DeptId { get; set; }

        public string EMP_NAME { get; set; }

        public string FirstInitName { get; set; }
        public string requestPerformer { get; set; }


    }
}
