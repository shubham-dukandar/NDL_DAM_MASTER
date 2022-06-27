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
    public class DealNoteSubmissionController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage savedata(Global_Master_Helper.Process.DealNote.DealNotSavedata obj)
        {
            string nextAppEmailID = string.Empty;
            string Result = string.Empty;
            string[] Dval = new string[0];

            string nextAppEmailID1 = string.Empty;
            string action1 = string.Empty;
            string currentapprvremailid = string.Empty;
            var hdrStatus = "";

            GlobleMasterDataContext db = new GlobleMasterDataContext();


            try
            {
                if (obj.action == "SUBMIT")
                {

                    //if (obj.requestid == 3)
                    //{
                    string rolename = "HOD  Approver";
                    var objemp = (
                 from fi in db.M_ACCESS_ROLEs
                 join me in db.M_ACCESS_USR_ROLE_MAPs on fi.PK_ROLE_ID equals me.ACCESS_ROLE_ID
                 join em in db.M_DEPARTMENTs on me.USER_ADID equals em.DEPARTMENT_HEAD
                 where fi.ROLE_NAME == rolename && me.IS_ACTIVE == 1 && fi.IS_ACTIVE == 1 && me.IS_ACTIVE == 1 && em.PK_DEPARTMENT_ID == obj.DeptId
                 select new
                 {
                     APPR_ID = me.USER_ADID,
                     me.EMP_NAME,
                     me.EMAILID,
                 }).ToList();
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
                            //  Dval[i] = "Nilima";
                        }
                    }

                    action1 = "SUBMIT";
                    hdrStatus = "Pending With HOD Approver";
                    nextAppEmailID = nextAppEmailID1;
                    currentapprvremailid = obj.approver_mail;
                    //}
                    //else
                    //{

                    //    var objemp = (
                    //      from fi in db.M_ACCESS_ROLEs
                    //      join me in db.M_ACCESS_USR_ROLE_MAPs on fi.PK_ROLE_ID equals me.ACCESS_ROLE_ID
                    //      // join em in db.M_EMPLOYEEs on me.USER_ADID equals em.AD_ID
                    //      where fi.ROLE_NAME == rolename && me.IS_ACTIVE == 1 && fi.IS_ACTIVE == 1 && me.IS_ACTIVE == 1
                    //      select new
                    //      {
                    //          APPR_ID = me.USER_ADID,
                    //          me.EMP_NAME,
                    //          me.EMAILID,
                    //      }).ToList();
                    //    if (objemp.Count > 0)
                    //    {
                    //        Dval = new string[(objemp.Count)];

                    //        for (int i = 0; i < objemp.Count; i++)
                    //        {
                    //            if (i == 0)
                    //            {
                    //                nextAppEmailID1 += objemp[i].EMAILID;
                    //            }
                    //            else
                    //            {
                    //                nextAppEmailID1 += "," + objemp[i].EMAILID;
                    //            }
                    //            Dval[i] = objemp[i].APPR_ID;
                    //            //  Dval[i] = "Nilima";
                    //        }
                    //    }

                    //    action1 = "SUBMIT";
                    //    hdrStatus = "Pending With Finance Team";
                    //    nextAppEmailID = nextAppEmailID1;
                    //    currentapprvremailid = obj.approver_mail;
                    //}


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

                    Result = Global_Master_Helper.Process.DealNote.DealNoteSubmission.SaveData(lsthdrData, lstdocData, obj.initiatorid, Dval, nextAppEmailID, obj.req_mailid, obj.action, obj.Master_API, obj.WFE_API, obj.NextSeqNo, obj.portalurl, hdrStatus, obj.EMP_NAME);
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
        public HttpResponseMessage SaveAsDraftData(Global_Master_Helper.Process.DealNote.DealNotSavedata obj)
        {
            string nextAppEmailID = string.Empty;
            string Result = string.Empty;
            string[] Dval = new string[0];

            string nextAppEmailID1 = string.Empty;
            string action1 = string.Empty;
            string currentapprvremailid = string.Empty;
            var hdrStatus = "";

            GlobleMasterDataContext db = new GlobleMasterDataContext();


            try
            {
                if (obj.action == "SAVEASDRAFT")
                {
                    var objemp = (
                                  from fi in db.M_ACCESS_ROLEs
                                  join me in db.M_ACCESS_USR_ROLE_MAPs on fi.PK_ROLE_ID equals me.ACCESS_ROLE_ID
                                  //from fi in db.M_ACCESS_USR_ROLE_MAPs
                                  where me.USER_ADID == obj.initiatorid && me.ACCESS_ROLE_ID == 3
                                  select new
                                  {
                                      me.USER_ADID,
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
                            Dval[i] = objemp[i].USER_ADID;
                            // Dval[i] = "Nilima";
                        }
                    }

                    action1 = "SAVEASDRAFT";
                    hdrStatus = "Pending With Initiator Person";
                    nextAppEmailID = nextAppEmailID1;
                    currentapprvremailid = obj.approver_mail;
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

                    Result = Global_Master_Helper.Process.DealNote.DealNoteSubmission.SaveAsDraftData(lsthdrData, lstdocData, obj.initiatorid, Dval, nextAppEmailID, obj.req_mailid, obj.action, obj.Master_API, obj.WFE_API, obj.NextSeqNo, obj.portalurl, hdrStatus, obj.EMP_NAME);
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
        public HttpResponseMessage Updatedata(Global_Master_Helper.Process.DealNote.DealNotSavedata obj)
        {
            string nextAppEmailID = string.Empty;
            string Result = string.Empty;
            string[] Dval = new string[0];

            string nextAppEmailID1 = string.Empty;
            string action1 = string.Empty;
            string currentapprvremailid = string.Empty;
            var hdrStatus = "";

            GlobleMasterDataContext db = new GlobleMasterDataContext();


            try
            {

                if (obj.action == "SUBMIT")
                {
                    //if (obj.requestid == 3)
                    //{
                    string rolename = "HOD  Approver";
                    var objemp = (
                 from fi in db.M_ACCESS_ROLEs
                 join me in db.M_ACCESS_USR_ROLE_MAPs on fi.PK_ROLE_ID equals me.ACCESS_ROLE_ID
                 join em in db.M_DEPARTMENTs on me.USER_ADID equals em.DEPARTMENT_HEAD
                 where fi.ROLE_NAME == rolename && me.IS_ACTIVE == 1 && fi.IS_ACTIVE == 1 && me.IS_ACTIVE == 1 && em.PK_DEPARTMENT_ID == obj.DeptId
                 select new
                 {
                     APPR_ID = me.USER_ADID,
                     me.EMP_NAME,
                     me.EMAILID,
                 }).ToList();
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
                            //  Dval[i] = "Nilima";
                        }
                    }

                    action1 = "SUBMIT";
                    hdrStatus = "Pending With HOD Approver";
                    nextAppEmailID = nextAppEmailID1;
                    currentapprvremailid = obj.approver_mail;
                    // }
                    //else
                    //{

                    //    var objemp = (
                    //      from fi in db.M_ACCESS_ROLEs
                    //      join me in db.M_ACCESS_USR_ROLE_MAPs on fi.PK_ROLE_ID equals me.ACCESS_ROLE_ID
                    //      // join em in db.M_EMPLOYEEs on me.USER_ADID equals em.AD_ID
                    //      where fi.ROLE_NAME == rolename && me.IS_ACTIVE == 1 && fi.IS_ACTIVE == 1 && me.IS_ACTIVE == 1
                    //      select new
                    //      {
                    //          APPR_ID = me.USER_ADID,
                    //          me.EMP_NAME,
                    //          me.EMAILID,
                    //      }).ToList();
                    //    if (objemp.Count > 0)
                    //    {
                    //        Dval = new string[(objemp.Count)];

                    //        for (int i = 0; i < objemp.Count; i++)
                    //        {
                    //            if (i == 0)
                    //            {
                    //                nextAppEmailID1 += objemp[i].EMAILID;
                    //            }
                    //            else
                    //            {
                    //                nextAppEmailID1 += "," + objemp[i].EMAILID;
                    //            }
                    //            Dval[i] = objemp[i].APPR_ID;
                    //            //  Dval[i] = "Nilima";
                    //        }
                    //    }

                    //    action1 = "SUBMIT";
                    //    hdrStatus = "Pending With HOD Person";
                    //    nextAppEmailID = nextAppEmailID1;
                    //    currentapprvremailid = obj.approver_mail;
                    //}


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

                    Result = Global_Master_Helper.Process.DealNote.DealNoteSubmission.UpdateData(lsthdrData, lstdocData, obj.initiatorid, Dval, nextAppEmailID, obj.req_mailid, obj.action, obj.Master_API, obj.WFE_API, obj.NextSeqNo, obj.portalurl, hdrStatus, obj.EMP_NAME);
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

    }
}
