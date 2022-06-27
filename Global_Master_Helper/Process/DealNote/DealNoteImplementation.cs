using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Global_Master_Helper.Process.DealNote
{
    public class DealNoteImplementation
    {

        public static List<ReqDetailForDealNote> getData(int ProcessID, int InstanceID, string MasterUrl, string WfeUrl)
        {
            List<ReqDetailForDealNote> objPO_Data = new List<ReqDetailForDealNote>();
            try
            {
                GlobleMasterDataContext context = new GlobleMasterDataContext();
                var finalpoData = (from hdr in context.T_DEALNOTE_HDRs
                                   join md in context.M_DEPARTMENTs on Convert.ToInt32(hdr.FK_DEPT_ID) equals md.PK_DEPARTMENT_ID
                                  // join me in context.M_EMPLOYEEs on hdr.CREATED_BY equals me.AD_ID
                                   join mv in context.M_REQUESTs on hdr.FK_REQUESTFOR_ID equals mv.PK_REQUEST_ID
                                   join ml in context.M_CATEGORies on hdr.FK_CAT_ID equals ml.PK_CAT_ID
                                   join mn in context.M_SUB_CATEGORies on hdr.FK_SUBCAT_ID equals mn.PK_SUB_CAT_ID
                                   join cu in context.M_CURRENCies on hdr.FK_CURRENCY equals cu.PK_CURRENCY_ID

                                   where hdr.FK_PROCESS_ID == Convert.ToInt32(ProcessID) && hdr.FK_INSTANCE_ID == Convert.ToInt32(InstanceID)

                                   select new
                                   {
                                       hdr.PK_DEALNOTE_HDR_ID,
                                       hdr.FK_PROCESS_ID,
                                       hdr.FK_INSTANCE_ID,
                                       hdr.DNREQUEST_NO,
                                       hdr.CREATED_BY,
                                       hdr.CREATED_DATE,
                                       hdr.SHORT_DESC,
                                       hdr.ROLLOUT_DATE,
                                       hdr.VALIDTILL_DATE,
                                       hdr.AMOUNT,
                                       hdr.DESC_FOR_REQUEST,
                                       hdr.REMARK,
                                       hdr.STATUS,
                                       hdr.FK_DEPT_ID,
                                       hdr.FK_REQUESTFOR_ID,
                                       hdr.FK_CAT_ID,
                                       hdr.FK_SUBCAT_ID,
                                       hdr.FK_CURRENCY,
                                       md.DEPARTMENT_NAME,
                                       mv.REQUEST_NAME,
                                       ml.CATEGORY_NAME,
                                       mn.SUB_CATEGORY_NAME,
                                       mn.SUB_CATEGORY_DESC,
                                       cu.CURRENCY_NAME,
                                       cu.CURRENCY_RATE,
                                       hdr.EMAILID,

                                   }).ToList();


                for (int i = 0; i < finalpoData.Count; i++)
                {
                    ReqDetailForDealNote obj = new ReqDetailForDealNote();
                    obj.FK_PROCESS_ID = Convert.ToInt32(finalpoData[i].FK_PROCESS_ID); ;
                    obj.FK_INSTANCE_ID = Convert.ToInt32(finalpoData[i].FK_INSTANCE_ID);
                    obj.DNREQUEST_NO = finalpoData[i].DNREQUEST_NO;
                    obj.CREATED_DATE = Convert.ToDateTime(finalpoData[i].CREATED_DATE).ToString("dd-MMM-yyyy");
                    obj.CREATED_BY = finalpoData[i].CREATED_BY;

                    obj.FK_DEPT_ID = Convert.ToInt32(finalpoData[i].FK_DEPT_ID);
                    obj.FK_REQUESTFOR_ID = Convert.ToInt32(finalpoData[i].FK_REQUESTFOR_ID);
                    obj.FK_CAT_ID = Convert.ToInt32(finalpoData[i].FK_CAT_ID);
                    obj.FK_SUBCAT_ID = Convert.ToInt32(finalpoData[i].FK_SUBCAT_ID);
                    obj.FK_CURRENCY = Convert.ToInt32(finalpoData[i].FK_CURRENCY);

                    obj.DEPTNAME = (finalpoData[i].DEPARTMENT_NAME);
                    obj.REQUESTFOR = (finalpoData[i].REQUEST_NAME);
                    obj.CATNAME = (finalpoData[i].CATEGORY_NAME);
                    obj.SUBCATNAME = (finalpoData[i].SUB_CATEGORY_NAME);
                    obj.SUBCATDESC = (finalpoData[i].SUB_CATEGORY_DESC);
                    obj.CURRENCY = (finalpoData[i].CURRENCY_NAME);
                    obj.CURRENCY_RATE = Convert.ToDecimal(finalpoData[i].CURRENCY_RATE);

                    obj.calculatedamount = Convert.ToDecimal((finalpoData[i].CURRENCY_RATE) * (finalpoData[i].AMOUNT));
                    obj.REMARK = finalpoData[i].REMARK;
                    obj.SHORT_DESC = finalpoData[i].SHORT_DESC;
                    obj.STATUS = finalpoData[i].STATUS;

                    // Hdr.NEXT_APPR_SEQ = 1;
                    obj.ROLLOUT_DATE = Convert.ToDateTime(finalpoData[i].ROLLOUT_DATE).ToString("dd-MMM-yyyy");
                    obj.VALIDTILL_DATE = Convert.ToDateTime(finalpoData[i].VALIDTILL_DATE).ToString("dd-MMM-yyyy");
                    obj.AMOUNT = Convert.ToDouble(finalpoData[i].AMOUNT);
                    obj.DESC_FOR_REQUEST = finalpoData[0].DESC_FOR_REQUEST;
                    obj.CREATEDEMAIL = finalpoData[i].EMAILID;

                    objPO_Data.Add(obj);
                }
            }
            catch (Exception ex)
            {
                Logger1.Error(ex);
                throw ex;
            }
            return objPO_Data;
        }


        public static List<CurrentApprovalMatrix> getCurrentApprovalMatrixData(string SesssionID, string Flag)
        {
            List<CurrentApprovalMatrix> objPO_Data = new List<CurrentApprovalMatrix>();
            try
            {
                GlobleMasterDataContext context = new GlobleMasterDataContext();
                var finalpoData = (

                    from am in context.M_APPROVAL_MATRIXes
                    join ac in context.M_ACCESS_ROLEs on am.ROLE_NAME equals ac.ROLE_NAME
                    join uam in context.M_ACCESS_USR_ROLE_MAPs on Convert.ToInt32(ac.PK_ROLE_ID) equals Convert.ToInt32(uam.ACCESS_ROLE_ID)
                    where uam.USER_ADID == SesssionID && am.FLAG == Flag



                    select new
                    {
                        am.ROLE_NAME,
                        am.ORDER_ID
                    }).ToList();


                for (int i = 0; i < finalpoData.Count; i++)
                {
                    CurrentApprovalMatrix obj = new CurrentApprovalMatrix();
                    obj.Role_Name = finalpoData[i].ROLE_NAME;
                    obj.Orderid = Convert.ToInt32(finalpoData[i].ORDER_ID);

                    objPO_Data.Add(obj);
                }
            }
            catch (Exception ex)
            {
                Logger1.Error(ex);
                throw ex;
            }
            return objPO_Data;
        }

        public static List<NextApprovalMatrix> getNextApprovalMatrix(int orderid, string Flag)
        {
            List<NextApprovalMatrix> objPO_Data = new List<NextApprovalMatrix>();
            try
            {
                GlobleMasterDataContext context = new GlobleMasterDataContext();
                var finalpoData = (

                    from am in context.M_APPROVAL_MATRIXes
                    join ac in context.M_ACCESS_ROLEs on am.ROLE_NAME equals ac.ROLE_NAME
                    join uam in context.M_ACCESS_USR_ROLE_MAPs on Convert.ToInt32(ac.PK_ROLE_ID) equals Convert.ToInt32(uam.ACCESS_ROLE_ID)
                    where am.ORDER_ID == (orderid + 1) && am.FLAG == Flag



                    select new
                    {
                        am.ROLE_NAME,
                        am.ORDER_ID
                    }).ToList();


                for (int i = 0; i < finalpoData.Count; i++)
                {
                    NextApprovalMatrix obj = new NextApprovalMatrix();
                    obj.Role_Name = finalpoData[i].ROLE_NAME;
                    obj.Orderid = Convert.ToInt32(finalpoData[i].ORDER_ID);

                    objPO_Data.Add(obj);
                }
            }
            catch (Exception ex)
            {
                Logger1.Error(ex);
                throw ex;
            }
            return objPO_Data;
        }



        public static string SaveApprovalData(List<DocMaster> objDocDetail, int processId, int instanceId, string stepName, string action, string SessionAdId, string[] Dval, string hdrStatus, string requestNo, string nextAppEmailID, string MasterUrl, string WfeUrl, string portalurl, string currentapprvremailid, string REMARK, string EMP_NAME)
        {
            string Result = "";

            //  //GLOBAL_WFEDataContext wfe = new GLOBAL_WFEDataContext();
            string WfeUrl1 = WfeUrl + "WFE";
            using (GlobleMasterDataContext context = new GlobleMasterDataContext())
            {
                try
                {
                    string mailsubj = "";
                    string mailbody = "";
                    context.Connection.Open();
                    context.Transaction = context.Connection.BeginTransaction();
                    T_DEALNOTE_HDR hdrobj = context.T_DEALNOTE_HDRs.Single(h => h.FK_PROCESS_ID == Convert.ToInt32(processId) && h.FK_INSTANCE_ID == Convert.ToInt32(instanceId));

                    if (action == "SUBMIT")
                    {
                        hdrobj.REMARK = REMARK;
                        hdrobj.STATUS = hdrStatus;

                        mailsubj = "DealNote Approval";
                        mailbody = "<html><body><h4 style='margin: 20px 0px 0px; font-family: Karla, sans-serif; background-color: rgb(255, 255, 255);'>Dear Sir/Madam,</h4><div style='font-family: Arial, Helvetica, sans-serif; font-size: 12px; background-color: rgb(255, 255, 255);'>&nbsp;</div><div style='font-family: Arial, Helvetica, sans-serif; font-size: 12px; background-color: rgb(255, 255, 255);'>The<b>&nbsp;</b><span style='font-family: Karla, sans-serif;'><b>Request Number: " + requestNo + "</b>&nbsp;is pending for your approval.</span></div><div style='font-family: Arial, Helvetica, sans-serif; font-size: 12px; background-color: rgb(255, 255, 255);'></p>You may click on below link to reach BTS Portal for further desired action. </pre><pre><h3 style='margin: 0px; font-family: Karla, sans-serif;'>URL: <a href='" + portalurl + "' style='color: rgb(17, 85, 204);' target='_blank'>" + portalurl + "</a></h3></p><h3 style='margin: 0px; font-family: Karla, sans-serif; color: rgb(238, 38, 38);'><span style='color:#FF0000;'><span class='im'>This is a system generated mail. Please do not reply to this mail</span></span></h3></div></body></html>";
                    }

                    else if (action == "APPROVE")
                    {
                        hdrobj.REMARK = REMARK;
                        hdrobj.STATUS = hdrStatus;
                        mailsubj = "DealNote Approval";
                        mailbody = "<html><body><h4 style='margin: 20px 0px 0px; font-family: Karla, sans-serif; background-color: rgb(255, 255, 255);'>Dear Sir/Madam,</h4><div style='font-family: Arial, Helvetica, sans-serif; font-size: 12px; background-color: rgb(255, 255, 255);'>&nbsp;</div><div style='font-family: Arial, Helvetica, sans-serif; font-size: 12px; background-color: rgb(255, 255, 255);'>The<b>&nbsp;</b><span style='font-family: Karla, sans-serif;'><b>Request Number: " + requestNo + "</b>&nbsp;is pending for modification.</span></div><div style='font-family: Arial, Helvetica, sans-serif; font-size: 12px; background-color: rgb(255, 255, 255);'></p>You may click on below link to reach BTS Portal for further desired action. </pre><pre><h3 style='margin: 0px; font-family: Karla, sans-serif;'>URL: <a href='" + portalurl + "' style='color: rgb(17, 85, 204);' target='_blank'>" + portalurl + "</a></h3></p><h3 style='margin: 0px; font-family: Karla, sans-serif; color: rgb(238, 38, 38);'><span style='color:#FF0000;'><span class='im'>This is a system generated mail. Please do not reply to this mail</span></span></h3></div></body></html>";
                    }
                    context.SubmitChanges();

                    for (int i = 0; i < objDocDetail.Count; i++)
                    {
                        M_DOCUMENT docObj = new M_DOCUMENT();
                        docObj.OBJECT_NAME = objDocDetail[i].OBJECT_TYPE;
                        docObj.OBJECT_VALUE = requestNo;
                        docObj.DOCUMENT_TYPE = objDocDetail[i].DOCUMENT_TYPE;
                        docObj.FILENAME = objDocDetail[i].FILENAME;
                        context.M_DOCUMENTs.InsertOnSubmit(docObj);
                        context.SubmitChanges();
                    }


                    string nxtstepid = "";
                    var inputforreleaseid = new
                    {
                        FK_PROCESSID = processId,
                        STEPNAME = stepName,
                        ACTION = action,
                    };
                    string inputJson = (new JavaScriptSerializer()).Serialize(inputforreleaseid);
                    HttpClient client = new HttpClient();
                    HttpContent inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");
                    HttpResponseMessage responseforreleaseid = client.PostAsync(WfeUrl1 + "/ReleaseID", inputContent).Result;
                    nxtstepid = responseforreleaseid.Content.ReadAsStringAsync().Result.ToString();
                    nxtstepid = nxtstepid.Replace("\"", "");

                    var inputforreleasestep = new
                    {
                        FK_PROCESSID = processId,
                        INSTANCEID = instanceId,
                        STEPID = Convert.ToInt32(nxtstepid),
                        USRNAME = SessionAdId,
                        USREMAIL = nextAppEmailID,
                        HEADERINFO = requestNo,
                        Approver = Dval,
                        //  REMARK = "Approval submit",
                        REMARK = REMARK,
                        ExitActionMailSubject = mailsubj,
                        ExitActionMailId = nextAppEmailID,
                        ExitActionMailCC = currentapprvremailid,
                        ExitActionMailBody = mailbody,
                        MasterUrl = MasterUrl,
                        ProcessUrl = MasterUrl,
                        WfeUrl = WfeUrl,
                        EMP_NAME = EMP_NAME
                    };
                    inputJson = (new JavaScriptSerializer()).Serialize(inputforreleasestep);
                    client = new HttpClient();
                    inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");
                    HttpResponseMessage responserelasestep = client.PostAsync(WfeUrl1 + "/ReleaseStep", inputContent).Result;
                    string relno = "";
                    relno = responserelasestep.Content.ReadAsStringAsync().Result.ToString();
                    relno = relno.Replace("\"", "");

                    ////wfe.Transaction.Commit();
                    context.Transaction.Commit();

                    Result = "true";
                }
                catch (Exception ex)
                {
                    context.Transaction.Rollback();
                    //  //wfe.Transaction.Rollback();
                    Logger1.Error(ex);
                    Result = AppConstants.ErrorMessage;
                    throw ex;
                }
            }
            return Result;
        }


        public static string SendBackReq(int processId, int instanceId, string stepName, string action, string requester_Mail, string[] Dval, string hdrStatus, string requestNo, string nextAppEmailID, string MasterUrl, string WfeUrl, string portalurl, string currentapprvremailid, string REMARK, string initiatorid, string EMP_NAME)
        {
            string Result = "";

            //  //GLOBAL_WFEDataContext wfe = new GLOBAL_WFEDataContext();
            string WfeUrl1 = WfeUrl + "WFE";
            using (GlobleMasterDataContext context = new GlobleMasterDataContext())
            {
                try
                {
                    string mailsubj = "";
                    string mailbody = "";
                    context.Connection.Open();
                    context.Transaction = context.Connection.BeginTransaction();
                    T_DEALNOTE_HDR hdrobj = context.T_DEALNOTE_HDRs.Single(h => h.FK_PROCESS_ID == Convert.ToInt32(processId) && h.FK_INSTANCE_ID == Convert.ToInt32(instanceId));

                    if (action == "SUBMIT")
                    {
                        hdrobj.REMARK = REMARK;
                        hdrobj.STATUS = hdrStatus;

                        mailsubj = "DealNote Approval";
                        mailbody = "<html><body><h4 style='margin: 20px 0px 0px; font-family: Karla, sans-serif; background-color: rgb(255, 255, 255);'>Dear Sir/Madam,</h4><div style='font-family: Arial, Helvetica, sans-serif; font-size: 12px; background-color: rgb(255, 255, 255);'>&nbsp;</div><div style='font-family: Arial, Helvetica, sans-serif; font-size: 12px; background-color: rgb(255, 255, 255);'>The<b>&nbsp;</b><span style='font-family: Karla, sans-serif;'><b>Request Number: " + requestNo + "</b>&nbsp;is pending for your approval.</span></div><div style='font-family: Arial, Helvetica, sans-serif; font-size: 12px; background-color: rgb(255, 255, 255);'></p>You may click on below link to reach BTS Portal for further desired action. </pre><pre><h3 style='margin: 0px; font-family: Karla, sans-serif;'>URL: <a href='" + portalurl + "' style='color: rgb(17, 85, 204);' target='_blank'>" + portalurl + "</a></h3></p><h3 style='margin: 0px; font-family: Karla, sans-serif; color: rgb(238, 38, 38);'><span style='color:#FF0000;'><span class='im'>This is a system generated mail. Please do not reply to this mail</span></span></h3></div></body></html>";
                    }

                    else if (action == "APPROVE")
                    {
                        hdrobj.REMARK = REMARK;
                        hdrobj.STATUS = hdrStatus;
                        mailsubj = "DealNote Approval";
                        mailbody = "<html><body><h4 style='margin: 20px 0px 0px; font-family: Karla, sans-serif; background-color: rgb(255, 255, 255);'>Dear Sir/Madam,</h4><div style='font-family: Arial, Helvetica, sans-serif; font-size: 12px; background-color: rgb(255, 255, 255);'>&nbsp;</div><div style='font-family: Arial, Helvetica, sans-serif; font-size: 12px; background-color: rgb(255, 255, 255);'>The<b>&nbsp;</b><span style='font-family: Karla, sans-serif;'><b>Request Number: " + requestNo + "</b>&nbsp;is pending for modification.</span></div><div style='font-family: Arial, Helvetica, sans-serif; font-size: 12px; background-color: rgb(255, 255, 255);'></p>You may click on below link to reach BTS Portal for further desired action. </pre><pre><h3 style='margin: 0px; font-family: Karla, sans-serif;'>URL: <a href='" + portalurl + "' style='color: rgb(17, 85, 204);' target='_blank'>" + portalurl + "</a></h3></p><h3 style='margin: 0px; font-family: Karla, sans-serif; color: rgb(238, 38, 38);'><span style='color:#FF0000;'><span class='im'>This is a system generated mail. Please do not reply to this mail</span></span></h3></div></body></html>";
                    }
                    if (action == "SENDBACKCLARIFICATION")
                    {
                        hdrobj.REMARK = REMARK;
                        hdrobj.STATUS = hdrStatus;

                        mailsubj = "DealNote Approval";
                        mailbody = "<html><body><h4 style='margin: 20px 0px 0px; font-family: Karla, sans-serif; background-color: rgb(255, 255, 255);'>Dear Sir/Madam,</h4><div style='font-family: Arial, Helvetica, sans-serif; font-size: 12px; background-color: rgb(255, 255, 255);'>&nbsp;</div><div style='font-family: Arial, Helvetica, sans-serif; font-size: 12px; background-color: rgb(255, 255, 255);'>The<b>&nbsp;</b><span style='font-family: Karla, sans-serif;'><b>Request Number: " + requestNo + "</b>&nbsp;is pending for your approval.</span></div><div style='font-family: Arial, Helvetica, sans-serif; font-size: 12px; background-color: rgb(255, 255, 255);'></p>You may click on below link to reach BTS Portal for further desired action. </pre><pre><h3 style='margin: 0px; font-family: Karla, sans-serif;'>URL: <a href='" + portalurl + "' style='color: rgb(17, 85, 204);' target='_blank'>" + portalurl + "</a></h3></p><h3 style='margin: 0px; font-family: Karla, sans-serif; color: rgb(238, 38, 38);'><span style='color:#FF0000;'><span class='im'>This is a system generated mail. Please do not reply to this mail</span></span></h3></div></body></html>";
                    }

                    else if (action == "SAVEASDRAFT")
                    {
                        hdrobj.REMARK = REMARK;
                        hdrobj.STATUS = hdrStatus;
                        mailsubj = "DealNote Approval";
                        mailbody = "<html><body><h4 style='margin: 20px 0px 0px; font-family: Karla, sans-serif; background-color: rgb(255, 255, 255);'>Dear Sir/Madam,</h4><div style='font-family: Arial, Helvetica, sans-serif; font-size: 12px; background-color: rgb(255, 255, 255);'>&nbsp;</div><div style='font-family: Arial, Helvetica, sans-serif; font-size: 12px; background-color: rgb(255, 255, 255);'>The<b>&nbsp;</b><span style='font-family: Karla, sans-serif;'><b>Request Number: " + requestNo + "</b>&nbsp;is pending for modification.</span></div><div style='font-family: Arial, Helvetica, sans-serif; font-size: 12px; background-color: rgb(255, 255, 255);'></p>You may click on below link to reach BTS Portal for further desired action. </pre><pre><h3 style='margin: 0px; font-family: Karla, sans-serif;'>URL: <a href='" + portalurl + "' style='color: rgb(17, 85, 204);' target='_blank'>" + portalurl + "</a></h3></p><h3 style='margin: 0px; font-family: Karla, sans-serif; color: rgb(238, 38, 38);'><span style='color:#FF0000;'><span class='im'>This is a system generated mail. Please do not reply to this mail</span></span></h3></div></body></html>";
                    }

                    else if (action == "SENDBACK")
                    {
                        hdrobj.REMARK = REMARK;
                        hdrobj.STATUS = hdrStatus;
                        mailsubj = "DEALNOTE Implementation Approval";
                        mailbody = "<html><body><h4 style='margin: 20px 0px 0px; font-family: Karla, sans-serif; background-color: rgb(255, 255, 255);'>Dear Sir/Madam,</h4><div style='font-family: Arial, Helvetica, sans-serif; font-size: 12px; background-color: rgb(255, 255, 255);'>&nbsp;</div><div style='font-family: Arial, Helvetica, sans-serif; font-size: 12px; background-color: rgb(255, 255, 255);'>The<b>&nbsp;</b><span style='font-family: Karla, sans-serif;'><b>Request Number: " + requestNo + "</b>&nbsp;is pending for modification.</span></div><div style='font-family: Arial, Helvetica, sans-serif; font-size: 12px; background-color: rgb(255, 255, 255);'></p>You may click on below link to reach BTS Portal for further desired action. </pre><pre><h3 style='margin: 0px; font-family: Karla, sans-serif;'>URL: <a href='" + portalurl + "' style='color: rgb(17, 85, 204);' target='_blank'>" + portalurl + "</a></h3></p><h3 style='margin: 0px; font-family: Karla, sans-serif; color: rgb(238, 38, 38);'><span style='color:#FF0000;'><span class='im'>This is a system generated mail. Please do not reply to this mail</span></span></h3></div></body></html>";
                    }
                    context.SubmitChanges();

                    string nxtstepid = "";
                    var inputforreleaseid = new
                    {
                        FK_PROCESSID = processId,
                        STEPNAME = stepName,
                        ACTION = action,
                    };
                    string inputJson = (new JavaScriptSerializer()).Serialize(inputforreleaseid);
                    HttpClient client = new HttpClient();
                    HttpContent inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");
                    HttpResponseMessage responseforreleaseid = client.PostAsync(WfeUrl1 + "/ReleaseID", inputContent).Result;
                    nxtstepid = responseforreleaseid.Content.ReadAsStringAsync().Result.ToString();
                    nxtstepid = nxtstepid.Replace("\"", "");

                    var inputforreleasestep = new
                    {
                        FK_PROCESSID = processId,
                        INSTANCEID = instanceId,
                        STEPID = Convert.ToInt32(nxtstepid),
                        USRNAME = initiatorid,
                        USREMAIL = nextAppEmailID,
                        HEADERINFO = requestNo,
                        Approver = Dval,
                        //  REMARK = "Approval submit",
                        REMARK = REMARK,
                        ExitActionMailSubject = mailsubj,
                        ExitActionMailId = nextAppEmailID,
                        ExitActionMailCC = currentapprvremailid,
                        ExitActionMailBody = mailbody,
                        MasterUrl = MasterUrl,
                        ProcessUrl = MasterUrl,
                        WfeUrl = WfeUrl,
                        EMP_NAME = EMP_NAME
                    };
                    inputJson = (new JavaScriptSerializer()).Serialize(inputforreleasestep);
                    client = new HttpClient();
                    inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");
                    HttpResponseMessage responserelasestep = client.PostAsync(WfeUrl1 + "/ReleaseStep", inputContent).Result;
                    string relno = "";
                    relno = responserelasestep.Content.ReadAsStringAsync().Result.ToString();
                    relno = relno.Replace("\"", "");

                    ////wfe.Transaction.Commit();
                    context.Transaction.Commit();

                    Result = "true";
                }
                catch (Exception ex)
                {
                    context.Transaction.Rollback();
                    //  //wfe.Transaction.Rollback();
                    Logger1.Error(ex);
                    Result = AppConstants.ErrorMessage;
                    throw ex;
                }
            }
            return Result;
        }

        public static List<ApprovalData> getApprovalData(int Instanceid, int ProcessId)
        {
            List<ApprovalData> objdata = new List<ApprovalData>();
            try
            {
                GlobleMasterDataContext context = new GlobleMasterDataContext();

                var APP_Data = context.P_GET_APPROVAL_INFO(Instanceid, ProcessId).ToList();


                for (int i = 0; i < APP_Data.Count; i++)
                {
                    ApprovalData poobj = new ApprovalData();

                    poobj.stepid = Convert.ToInt32(APP_Data[i].FK_STEPID);
                    poobj.stepname = APP_Data[i].STEP_NAME;
                    poobj.PERFORMEREMAILID = APP_Data[i].PERFORMER_EMAILID;
                    poobj.id = APP_Data[i].id;
                    poobj.IS_Initiator = APP_Data[i].is_initiator;


                    objdata.Add(poobj);
                }
            }
            catch (Exception ex)
            {
                Logger1.Error(ex);
                throw ex;
            }
            return objdata;
        }

        public static string SMSSaveApprovalData(List<DocMaster> objDocDetail, int processId, int instanceId, string stepName, string action, string SessionAdId, string[] Dval, string hdrStatus, string requestNo, string nextAppEmailID, string MasterUrl, string WfeUrl, string portalurl, string currentapprvremailid, string REMARK,int issmsconfig,string EMP_NAME)
        {
            string Result = "";

            //  //GLOBAL_WFEDataContext wfe = new GLOBAL_WFEDataContext();
            string WfeUrl1 = WfeUrl + "WFE";
            using (GlobleMasterDataContext context = new GlobleMasterDataContext())
            {
                try
                {
                    string mailsubj = "";
                    string mailbody = "";
                    context.Connection.Open();
                    context.Transaction = context.Connection.BeginTransaction();
                    T_DEALNOTE_HDR hdrobj = context.T_DEALNOTE_HDRs.Single(h => h.FK_PROCESS_ID == Convert.ToInt32(processId) && h.FK_INSTANCE_ID == Convert.ToInt32(instanceId));

                    if (action == "SUBMIT")
                    {
                        hdrobj.REMARK = REMARK;
                        hdrobj.STATUS = hdrStatus;
                        hdrobj.IS_SMS_Config = issmsconfig;
                        mailsubj = "DealNote SMS Approval";
                        mailbody = "<html><body><h4 style='margin: 20px 0px 0px; font-family: Karla, sans-serif; background-color: rgb(255, 255, 255);'>Dear Sir/Madam,</h4><div style='font-family: Arial, Helvetica, sans-serif; font-size: 12px; background-color: rgb(255, 255, 255);'>&nbsp;</div><div style='font-family: Arial, Helvetica, sans-serif; font-size: 12px; background-color: rgb(255, 255, 255);'>The<b>&nbsp;</b><span style='font-family: Karla, sans-serif;'><b>Request Number: " + requestNo + "</b>&nbsp;is pending for your approval.</span></div><div style='font-family: Arial, Helvetica, sans-serif; font-size: 12px; background-color: rgb(255, 255, 255);'></p>You may click on below link to reach BTS Portal for further desired action. </pre><pre><h3 style='margin: 0px; font-family: Karla, sans-serif;'>URL: <a href='" + portalurl + "' style='color: rgb(17, 85, 204);' target='_blank'>" + portalurl + "</a></h3></p><h3 style='margin: 0px; font-family: Karla, sans-serif; color: rgb(238, 38, 38);'><span style='color:#FF0000;'><span class='im'>This is a system generated mail. Please do not reply to this mail</span></span></h3></div></body></html>";
                    }

                    else if (action == "APPROVE")
                    {
                        hdrobj.REMARK = REMARK;
                        hdrobj.STATUS = hdrStatus;
                        hdrobj.IS_SMS_Config = issmsconfig;
                        mailsubj = "DealNote SMS Approval";
                        mailbody = "<html><body><h4 style='margin: 20px 0px 0px; font-family: Karla, sans-serif; background-color: rgb(255, 255, 255);'>Dear Sir/Madam,</h4><div style='font-family: Arial, Helvetica, sans-serif; font-size: 12px; background-color: rgb(255, 255, 255);'>&nbsp;</div><div style='font-family: Arial, Helvetica, sans-serif; font-size: 12px; background-color: rgb(255, 255, 255);'>The<b>&nbsp;</b><span style='font-family: Karla, sans-serif;'><b>Request Number: " + requestNo + "</b>&nbsp;is pending for modification.</span></div><div style='font-family: Arial, Helvetica, sans-serif; font-size: 12px; background-color: rgb(255, 255, 255);'></p>You may click on below link to reach BTS Portal for further desired action. </pre><pre><h3 style='margin: 0px; font-family: Karla, sans-serif;'>URL: <a href='" + portalurl + "' style='color: rgb(17, 85, 204);' target='_blank'>" + portalurl + "</a></h3></p><h3 style='margin: 0px; font-family: Karla, sans-serif; color: rgb(238, 38, 38);'><span style='color:#FF0000;'><span class='im'>This is a system generated mail. Please do not reply to this mail</span></span></h3></div></body></html>";
                    }
                    context.SubmitChanges();

                    for (int i = 0; i < objDocDetail.Count; i++)
                    {
                        M_DOCUMENT docObj = new M_DOCUMENT();
                        docObj.OBJECT_NAME = objDocDetail[i].OBJECT_TYPE;
                        docObj.OBJECT_VALUE = requestNo;
                        docObj.DOCUMENT_TYPE = objDocDetail[i].DOCUMENT_TYPE;
                        docObj.FILENAME = objDocDetail[i].FILENAME;
                        context.M_DOCUMENTs.InsertOnSubmit(docObj);
                        context.SubmitChanges();
                    }


                    string nxtstepid = "";
                    var inputforreleaseid = new
                    {
                        FK_PROCESSID = processId,
                        STEPNAME = stepName,
                        ACTION = action,
                    };
                    string inputJson = (new JavaScriptSerializer()).Serialize(inputforreleaseid);
                    HttpClient client = new HttpClient();
                    HttpContent inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");
                    HttpResponseMessage responseforreleaseid = client.PostAsync(WfeUrl1 + "/ReleaseID", inputContent).Result;
                    nxtstepid = responseforreleaseid.Content.ReadAsStringAsync().Result.ToString();
                    nxtstepid = nxtstepid.Replace("\"", "");

                    var inputforreleasestep = new
                    {
                        FK_PROCESSID = processId,
                        INSTANCEID = instanceId,
                        STEPID = Convert.ToInt32(nxtstepid),
                        USRNAME = SessionAdId,
                        USREMAIL = nextAppEmailID,
                        HEADERINFO = requestNo,
                        Approver = Dval,
                        //  REMARK = "Approval submit",
                        REMARK = REMARK,
                        ExitActionMailSubject = mailsubj,
                        ExitActionMailId = nextAppEmailID,
                        ExitActionMailCC = currentapprvremailid,
                        ExitActionMailBody = mailbody,
                        MasterUrl = MasterUrl,
                        ProcessUrl = MasterUrl,
                        WfeUrl = WfeUrl,
                        EMP_NAME=EMP_NAME
                    };
                    inputJson = (new JavaScriptSerializer()).Serialize(inputforreleasestep);
                    client = new HttpClient();
                    inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");
                    HttpResponseMessage responserelasestep = client.PostAsync(WfeUrl1 + "/ReleaseStep", inputContent).Result;
                    string relno = "";
                    relno = responserelasestep.Content.ReadAsStringAsync().Result.ToString();
                    relno = relno.Replace("\"", "");

                    ////wfe.Transaction.Commit();
                    context.Transaction.Commit();

                    Result = "true";
                }
                catch (Exception ex)
                {
                    context.Transaction.Rollback();
                    //  //wfe.Transaction.Rollback();
                    Logger1.Error(ex);
                    Result = AppConstants.ErrorMessage;
                    throw ex;
                }
            }
            return Result;
        }


    }
    public class ReqDetailForDealNote1
    {
        public int PK_INVOICE_HDR_ID { get; set; }
        public int FK_PROCESS_ID { get; set; }
        public int FK_INSTANCE_ID { get; set; }
        public string REQUEST_NO { get; set; }
        public string CREATEDEMAIL { get; set; }
        public string initiatorid { get; set; }
        public string req_mailid { get; set; }
        public string action { get; set; }
        public string Master_API { get; set; }
        public string Tarns_API { get; set; }
        public string WFE_API { get; set; }
        public string DNREQUEST_NO { get; set; }
        public int FK_DEPT_ID { get; set; }
        public string DEPTNAME { get; set; }
        public string SHORT_DESC { get; set; }
        public int FK_REQUESTFOR_ID { get; set; }
        public string REQUESTFOR { get; set; }
        public int FK_CAT_ID { get; set; }

        public string CATNAME { get; set; }
        public int FK_SUBCAT_ID { get; set; }
        public string SUBCATNAME { get; set; }

        public string SUBCATDESC { get; set; }
        public int FK_CURRENCY { get; set; }

        public string CURRENCY { get; set; }

        public decimal CURRENCY_RATE { get; set; }

        public decimal calculatedamount { get; set; }
        public string ROLLOUT_DATE { get; set; }
        public string VALIDTILL_DATE { get; set; }
        public double AMOUNT { get; set; }
        public string DESC_FOR_REQUEST { get; set; }
        public string REMARK { get; set; }
        public string CREATED_BY { get; set; }
        public string CREATED_DATE { get; set; }
        public string STATUS { get; set; }

        public string approver_mail { get; set; }
        public string requester_Mail { get; set; }
        public int NextSeqNo { get; set; }
        public string portalurl { get; set; }
        public string stepName { get; set; }
    }


    public class CurrentApprovalMatrix1
    {
        public string portalurl { get; set; }
        public string Master_API { get; set; }
        public string Flag { get; set; }

        public string SesssionID { get; set; }

        public string Role_Name { get; set; }

        public int Orderid { get; set; }
    }

    public class NextApprovalMatrix1
    {
        public string portalurl { get; set; }
        public string Master_API { get; set; }
        public string Flag { get; set; }

        public string SesssionID { get; set; }

        public string Role_Name { get; set; }

        public int Orderid { get; set; }
    }
    public class ApprovalData1
    {
        public int FK_PROCESSID { get; set; }
        public int INSTANCE_ID { get; set; }

        public string id { get; set; }
        public string label { get; set; }

        public string PERFORMEREMAILID { get; set; }

        public string stepname { get; set; }
        public int stepid { get; set; }
        public int IS_Initiator { get; set; }
        public string EMP_NAME { get; set; }
    }
}
