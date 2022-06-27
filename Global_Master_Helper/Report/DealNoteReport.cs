using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global_Master_Helper.Report
{
    public class DealNoteReport
    {
        public static List<DealNoteHdrData> getDealNoteHdrData()
        {
            List<DealNoteHdrData> objdata = new List<DealNoteHdrData>();
            try
            {
                GlobleMasterDataContext context = new GlobleMasterDataContext();

                var finalDNData = context.P_GET_RPT_DEAL_NOTE_HDR().ToList();


                for (int i = 0; i < finalDNData.Count; i++)
                {
                    DealNoteHdrData obj = new DealNoteHdrData();


                    obj.FK_PROCESS_ID = Convert.ToInt32(finalDNData[i].FK_PROCESS_ID);
                    obj.FK_INSTANCE_ID = Convert.ToInt32(finalDNData[i].FK_INSTANCE_ID);
                    obj.DNREQUEST_NO = finalDNData[i].DNREQUEST_NO;
                    obj.CREATED_DATE = Convert.ToDateTime(finalDNData[i].CREATED_DATE).ToString("dd-MMM-yyyy");
                    obj.CREATED_BY = finalDNData[i].CREATED_BY;
                    obj.DEPARTMENT_NAME = finalDNData[i].DEPARTMENT_NAME;
                    obj.REQUEST_NAME = finalDNData[i].REQUEST_NAME;
                    obj.REQUEST_DESC = finalDNData[i].REQUEST_DESC;
                    obj.SHORT_DESC = finalDNData[i].SHORT_DESC;
                    obj.CATEGORY_NAME = finalDNData[i].CATEGORY_NAME;
                    obj.SUB_CATEGORY_NAME = finalDNData[i].SUB_CATEGORY_NAME;
                    obj.SUB_CATEGORY_DESC = finalDNData[i].SUB_CATEGORY_DESC;
                    obj.VALIDTILL_DATE = finalDNData[i].VALID_TILL_DATE;
                    obj.CURRENCY_NAME = finalDNData[i].CURRENCY_NAME;
                    obj.DESC_FOR_REQUEST = finalDNData[i].DESC_FOR_REQUEST;
                    obj.AMOUNT = Convert.ToDecimal(finalDNData[i].AMOUNT);
                    //obj.INVOICE_DATE = Convert.ToDateTime(finalDNData[i].INVOICE_DATE).ToString("dd-MMM-yyyy"); IS_SMS_CONFIG
                    obj.REMARK = finalDNData[i].REMARK;
                    obj.STATUS = finalDNData[i].STATUS;
                    obj.IS_SMS_CONFIG = finalDNData[i].IS_SMS_CONFIG;

                    obj.Business_Justification = finalDNData[i].Business_Justification;
                    obj.Recommendation = finalDNData[i].Recommendation;
                    obj.Imp_Timelines = finalDNData[i].Imp_Timelines;
                    obj.PENDING_WITH = finalDNData[i].PENDING_WITH;

                    obj.REGION_NAME = finalDNData[i].REGION_NAME;
                    obj.DIVISION_NAME = finalDNData[i].DIVISION_NAME;
                    objdata.Add(obj);
                }
            }
            catch (Exception ex)
            {
                Logger1.Error(ex);
                throw ex;
            }
            return objdata;
        }
    }

    public class DealNoteHdrData
    {
        public int FK_PROCESS_ID { get; set; }
        public int FK_INSTANCE_ID { get; set; }
        public string DNREQUEST_NO { get; set; }
        public string CREATED_BY { get; set; }
        public string CREATED_DATE { get; set; }
        public string STATUS { get; set; }
        public string SHORT_DESC { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        public string REQUEST_NAME { get; set; }

        public string REQUEST_DESC { get; set; }
        public string CATEGORY_NAME { get; set; }

        public string ROLLOUT_DATE { get; set; }
        public string VALIDTILL_DATE { get; set; }
        public decimal AMOUNT { get; set; }
        public string DESC_FOR_REQUEST { get; set; }
        public string REMARK { get; set; }


        public string SUB_CATEGORY_NAME { get; set; }
        public string SUB_CATEGORY_DESC { get; set; }
        public string CURRENCY_NAME { get; set; }
        public string IS_SMS_CONFIG { get; set; }
        public string stepName { get; set; }
        public string Requester_ADID { get; set; }
        public int requestid { get; set; }

        public string NxtApproverrolename { get; set; }

        public string Business_Justification { get; set; }
        public string Recommendation { get; set; }
        public string Imp_Timelines { get; set; }

        public int DeptId { get; set; }

        public string PENDING_WITH { get; set; }
        public string REGION_NAME { get; set; }
        public string DIVISION_NAME { get; set; }
    }
}
