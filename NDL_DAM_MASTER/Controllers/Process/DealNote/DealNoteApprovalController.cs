using Global_Master_Helper;
using Global_Master_Helper.Process.DealNote;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace NDL_DAM_MASTER.Controllers.Process.DealNote
{
    public class DealNoteApprovalController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage getprocessdata(Global_Master_Helper.Process.DealNote.ReqDetailForDealNote obj)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, Global_Master_Helper.Process.DealNote.DealNoteApproval.getData(obj.FK_PROCESS_ID, obj.FK_INSTANCE_ID, obj.Master_API, obj.WFE_API));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error occured while executing");
                throw ex;
            }
        }


        [HttpPost]
        public HttpResponseMessage getCurrentApprovalMatrix(Global_Master_Helper.Process.DealNote.CurrentApprovalMatrix obj)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, Global_Master_Helper.Process.DealNote.DealNoteApproval.getCurrentApprovalMatrixData(obj.SesssionID, obj.Flag, obj.requestid,obj.Status));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error occured while executing");
                throw ex;
            }
        }
        [HttpPost]
        public HttpResponseMessage getNextApprovalMatrix(Global_Master_Helper.Process.DealNote.NextApprovalMatrix obj)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, Global_Master_Helper.Process.DealNote.DealNoteApproval.getNextApprovalMatrix(obj.Orderid, obj.Flag, obj.Role_Name));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error occured while executing");
                throw ex;
            }
        }

        [HttpPost]
        public HttpResponseMessage ApprovalSave(Global_Master_Helper.Process.DealNote.DealNotSavedata obj)
        {
            string nextAppEmailID = string.Empty;
            string Result = string.Empty;
            string[] Dval = new string[1];

            string nextAppEmailID1 = string.Empty;
            string action1 = string.Empty;
            string currentapprvremailid = string.Empty;
            var hdrStatus = "";

            GlobleMasterDataContext db = new GlobleMasterDataContext();


            try
            {
                if (obj.action == "SUBMIT")
                {
                    var objemp = (
                                  from fi in db.M_ACCESS_ROLEs
                                  join me in db.M_ACCESS_USR_ROLE_MAPs on fi.PK_ROLE_ID equals me.ACCESS_ROLE_ID
                                  // join em in db.M_EMPLOYEEs on me.USER_ADID equals em.AD_ID
                                  where fi.ROLE_NAME == obj.NxtApproverrolename && me.IS_ACTIVE == 1 && fi.IS_ACTIVE == 1 && me.IS_ACTIVE == 1
                                  select new
                                  {
                                      APPR_ID = me.USER_ADID,
                                      me.EMP_NAME,
                                      me.EMAILID,
                                  }
                             ).ToList();


                    //Dval[0] = "flologic1";
                    if (objemp.Count > 0)
                    {
                        Dval = new string[(objemp.Count)];

                        for (int i = 0; i < objemp.Count; i++)
                        {
                            if (i == 0)
                            {
                                nextAppEmailID1 += objemp[i].EMAILID;
                            }
                            else
                            {
                                nextAppEmailID1 += "," + objemp[i].EMAILID;
                            }
                            Dval[i] = objemp[i].APPR_ID;
                            //Dval[i] = "Nilima";
                        }
                    }

                    action1 = "SUBMIT";

                    hdrStatus = "Pending With" +" "+ obj.NxtApproverrolename;
                    nextAppEmailID = nextAppEmailID1;
                    currentapprvremailid = obj.approver_mail;
                }
                else
                {
                    //Dval[0] = "flologic1";
                    Dval[0] = obj.FirstInitName;
                    action1 = "APPROVE";
                    hdrStatus = "Pending With Implementation";
                    //nextAppEmailID = obj.approver_mail;
                }

                List<DealNoteHdr> lsthdrData = new List<DealNoteHdr>();

                List<DocMaster> lstdocData = new List<DocMaster>();

                if (Dval.Length > 0)
                {
                    JObject xmlJObject = JObject.Parse(obj.transactiondata);
                    var hdrData = Convert.ToString(xmlJObject["hdrData"]);
                    var docData = Convert.ToString(xmlJObject["docData"]);


                    lsthdrData = JsonConvert.DeserializeObject<List<DealNoteHdr>>(hdrData.ToString());
                    lstdocData = JsonConvert.DeserializeObject<List<DocMaster>>(docData.ToString());

                    Result = Global_Master_Helper.Process.DealNote.DealNoteApproval.SaveApprovalData(lstdocData, obj.processid, obj.instanceid, obj.stepName, action1, obj.initiatorid, Dval, hdrStatus, obj.DNREQUEST_NO, nextAppEmailID, obj.Master_API, obj.WFE_API, obj.portalurl, currentapprvremailid, obj.REMARK, obj.EMP_NAME);

                    if (Result != "Error occured while saving data")
                    {
                        if (docData != "[]")
                        {
                            string Res = Global_Master_Helper.Common.SaveDocuments("DEALNOTE", Result, lstdocData);
                        }
                        return Request.CreateResponse(HttpStatusCode.OK, Result);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, Result);
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Approver not found");
                }


            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error occured while executing");
                throw ex;
            }
        }

        [HttpPost]
        public HttpResponseMessage SendBack(Global_Master_Helper.Process.DealNote.DealNotSavedata obj)
        {
            string nextAppEmailID = string.Empty;
            string Result = string.Empty;
            string[] Dval = new string[1];

            string nextAppEmailID1 = string.Empty;
            string action1 = string.Empty;
            string currentapprvremailid = string.Empty;
            var hdrStatus = "";
            var stepName = "";

            GlobleMasterDataContext db = new GlobleMasterDataContext();


            try
            {
                if (obj.action == "SAVEASDRAFT")
                {
                    Dval = new string[1];

                    Dval[0] = obj.Requester_ADID;
                    hdrStatus = "Pending With Initiator";
                    action1 = "SAVEASDRAFT";
                    nextAppEmailID = obj.requester_Mail;
                    stepName = "DEALNOTE SUBMISSION";
                }
                if (obj.action == "SENDBACK")
                {
                    Dval = new string[1];

                    Dval[0] = obj.Requester_ADID;
                    hdrStatus = "Pending With Initiator";
                    action1 = "SENDBACK";
                    nextAppEmailID = obj.requester_Mail;
                    stepName = "DEALNOTE APRROVAL";
                }


                if (obj.action == "SENDBACKCLARIFICATION")
                {
                    Dval = new string[1];

                    //Dval[0] = obj.Requester_ADID;
                    Dval[0] = obj.NxtApproverrolename; 
                    hdrStatus = "Pending With Approval";
                    action1 = "SENDBACKCLARIFICATION";
                    nextAppEmailID = obj.requester_Mail;
                    stepName = "DEALNOTE APRROVAL";
                }

                //List<DealNoteHdr> lsthdrData = new List<DealNoteHdr>();

                //List<DocMaster> lstdocData = new List<DocMaster>();

                if (Dval.Length > 0)
                {
                    //JObject xmlJObject = JObject.Parse(obj.transactiondata);
                    //var hdrData = Convert.ToString(xmlJObject["hdrData"]);
                    //var docData = Convert.ToString(xmlJObject["docData"]);


                    //lsthdrData = JsonConvert.DeserializeObject<List<DealNoteHdr>>(hdrData.ToString());
                    //lstdocData = JsonConvert.DeserializeObject<List<DocMaster>>(docData.ToString());

                    Result = Global_Master_Helper.Process.DealNote.DealNoteApproval.SendBackReq(obj.processid, obj.instanceid, stepName, action1, obj.requester_Mail, Dval, hdrStatus, obj.DNREQUEST_NO, nextAppEmailID, obj.Master_API, obj.WFE_API, obj.portalurl, currentapprvremailid, obj.REMARK, obj.NxtApproverrolename, obj.EMP_NAME,obj.requestPerformer);

                    if (Result != "Error occured while saving data")
                    {
                        //if (docData != "[]")
                        //{
                        //    string Res = Global_Master_Helper.Common.SaveDocuments("DEALNOTE", Result, lstdocData);
                        //}
                        return Request.CreateResponse(HttpStatusCode.OK, Result);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, Result);
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Approver not found");
                }


            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error occured while executing");
                throw ex;
            }
        }



        [HttpPost]
        public HttpResponseMessage getApprovalData(Global_Master_Helper.Process.DealNote.ApprovalData obj)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, Global_Master_Helper.Process.DealNote.DealNoteApproval.getApprovalData(obj.INSTANCE_ID, obj.FK_PROCESSID));
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error occured while executing");
                throw ex;
            }
        }


    }
}
