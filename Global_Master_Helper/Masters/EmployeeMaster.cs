using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global_Master_Helper.Masters
{
    public class EmployeeMaster
    {

        public static List<Employee> getAllEmployee()
        {
            GlobleMasterDataContext db = new GlobleMasterDataContext();
            List<Employee> empData = new List<Employee>();
            var emp = (from e in db.M_EMPLOYEEs
                       join dp in db.M_DEPARTMENTs on e.FK_DEPARTMENT_ID equals dp.PK_DEPARTMENT_ID
                       //join des in db.M_DESIGNATIONs on e.FK_DESIGNATION_ID equals des.PK_DESIGNATION_ID
                       //join gr in db.M_GRADEs on e.FK_GRADE_ID equals gr.PK_GRADE_ID
                       //join ml in db.M_LOCATIONs on e.FK_BRANCH_ID equals ml.PK_LOCATION_ID
                       orderby e.EMP_NAME
                       where e.IS_ACTIVE == 1
                       select new
                       {
                           e.AD_ID,
                           e.EMP_CODE,
                           e.EMP_NAME,
                           e.EMAILID,
                           e.FK_CLIENTID,
                           e.PK_EMP_ID,
                           e.ADDRESS,
                           e.ADMINREPORTING_TO,
                           e.CREATED_BY,
                           e.CREATION_DATE,
                           e.DATE_BIRTH,
                           e.DATE_JOINING,
                           e.FK_BRANCH_ID,
                          // ml.LOCATION_NAME,
                           e.FK_COMPANY_ID,
                           e.FK_COST_CENTER_ID,
                           e.FK_DEPARTMENT_ID,
                           dp.DEPARTMENT_NAME,
                           e.FK_DESIGNATION_ID,
                          // des.DESIGNATION_NAME,
                           e.FK_DIVISION_ID,
                           e.FK_GRADE_ID,
                          // gr.GRADE,
                           e.FK_VERTICAL_ID,
                           e.FUNCTIONALREPORTING_TO,
                           e.IFSCCODE,
                           e.MOBILE_NO,
                           e.REGIONAL_HR,
                           e.LAST_WORKING_DATE

                       }).ToList();


            for (int i = 0; i < emp.Count(); i++)
            {
                Employee obj = new Employee();
                obj.label = emp[i].EMP_NAME + "-" + emp[i].EMP_CODE;
                obj.EMP_NAME = emp[i].EMP_NAME;
                obj.AD_ID = emp[i].AD_ID;
                obj.EMP_CODE = emp[i].EMP_CODE;
                obj.id = emp[i].AD_ID;
                obj.MOBILE_NO = emp[i].MOBILE_NO;
                obj.ADDRESS = emp[i].ADDRESS;
                obj.EMAILID = emp[i].EMAILID;
                obj.FK_DESIGNATION_ID = Convert.ToInt32(emp[i].FK_DESIGNATION_ID);
                //obj.DESIGNATION_NAME = emp[i].DESIGNATION_NAME;
                obj.FK_BRANCH_ID = Convert.ToInt32(emp[i].FK_BRANCH_ID);
                //obj.BRANCH_NAME = emp[i].LOCATION_NAME;
                obj.FK_DEPARTMENT_ID = Convert.ToInt32(emp[i].FK_DEPARTMENT_ID);
                obj.DEPARTMENT_NAME = emp[i].DEPARTMENT_NAME;
                obj.DATE_JOINING = string.Format("{0:dd-MMM-yyyy}", emp[i].DATE_JOINING);
                obj.DATE_BIRTH = string.Format("{0:dd-MMM-yyyy}", emp[i].DATE_BIRTH);
                obj.LAST_WORKING_DATE = string.Format("{0:dd-MMM-yyyy}", emp[i].LAST_WORKING_DATE);
                obj.FK_GRADE_ID = Convert.ToInt32(emp[i].FK_GRADE_ID);
                //obj.GRADE = emp[i].GRADE;
                obj.IFSCODE = emp[i].IFSCCODE;
                obj.FK_COMPANY_ID = Convert.ToInt32(emp[i].FK_COMPANY_ID);
                obj.FUNCTIONALREPORTING_TO = emp[i].ADMINREPORTING_TO;
                obj.FUNCTIONALREPORTING_NAME = emp[i].ADMINREPORTING_TO;
                obj.CLIENT_ID = Convert.ToInt32(emp[i].FK_CLIENTID);
                obj.Regional_Hr = emp[i].REGIONAL_HR;
                obj.PK_EMP_ID = emp[i].PK_EMP_ID;
                empData.Add(obj);
            }
            return empData;
        }


        public static string SaveEmployeeData(string EMP_NAME, string EMP_CODE, string EMAILID, string AD_ID, string CREATED_BY, int FK_DEPARTMENT_ID, int FK_DESIGNATION_ID, int FK_GRADE_ID, int FK_BRANCH_ID, string MOBILE_NO, string ADDRESS)
        {
            string Result = "";
            using (GlobleMasterDataContext db = new GlobleMasterDataContext())
            {
                try
                {

                    db.Connection.Open();
                    db.Transaction = db.Connection.BeginTransaction();
                    M_EMPLOYEE check = (from c in db.M_EMPLOYEEs where c.IS_ACTIVE == 1 && c.EMP_NAME == EMP_NAME && c.EMP_CODE == EMP_CODE && c.AD_ID == AD_ID && c.FK_BRANCH_ID == FK_BRANCH_ID && c.FK_DEPARTMENT_ID == FK_DEPARTMENT_ID && c.FK_DESIGNATION_ID == FK_DESIGNATION_ID && c.FK_GRADE_ID == FK_GRADE_ID select c).FirstOrDefault();
                    if (check == null)
                    {
                        M_EMPLOYEE objempdata = new M_EMPLOYEE();
                        objempdata.EMP_NAME = EMP_NAME;
                        objempdata.EMP_CODE = EMP_CODE;
                        objempdata.AD_ID = AD_ID;
                        objempdata.PASSWORD = "A7Uu+vvWxNzxFtjQtq0a1Q==";
                        objempdata.MOBILE_NO = MOBILE_NO;
                        objempdata.ADDRESS = ADDRESS;
                        objempdata.EMAILID = EMAILID;
                        objempdata.FK_DESIGNATION_ID = Convert.ToInt32(FK_DESIGNATION_ID);
                        objempdata.FK_BRANCH_ID = Convert.ToInt32(FK_BRANCH_ID);
                        objempdata.FK_GRADE_ID = Convert.ToInt32(FK_GRADE_ID);
                        objempdata.FK_DEPARTMENT_ID = Convert.ToInt32(FK_DEPARTMENT_ID);
                        objempdata.IS_ACTIVE = 1;
                        objempdata.CREATED_BY = CREATED_BY;
                        objempdata.CREATION_DATE = DateTime.Now;
                        objempdata.FK_VERTICAL_ID = 0;
                        objempdata.FK_DIVISION_ID = 1;
                        // objempdata.IFSCODE = IFSCODE;
                        objempdata.FK_CLIENTID = 1;
                        db.M_EMPLOYEEs.InsertOnSubmit(objempdata);
                        var Cnt = db.GetChangeSet();
                        var Update_Count = Cnt.Inserts.Count();
                        db.SubmitChanges();
                        db.Transaction.Commit();
                        Result = "true";
                    }
                    else
                    {
                        Result = "Duplicate Entry Found";
                    }
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

        public static string UpdateEmployeeData(string EMP_NAME, string EMP_CODE, string EMAILID, string AD_ID, string CREATED_BY, int PK_EMP_ID, int FK_DEPARTMENT_ID, int FK_DESIGNATION_ID, int FK_GRADE_ID, int FK_BRANCH_ID, string MOBILE_NO, string ADDRESS)
        {
            string Result = "";
            using (GlobleMasterDataContext db = new GlobleMasterDataContext())
            {
                try
                {
                    db.Connection.Open();
                    db.Transaction = db.Connection.BeginTransaction();
                    M_EMPLOYEE check = (from c in db.M_EMPLOYEEs where c.IS_ACTIVE == 1 && c.EMP_NAME == EMP_NAME && c.EMP_CODE == EMP_CODE && c.AD_ID == AD_ID && c.FK_BRANCH_ID == FK_BRANCH_ID && c.FK_DEPARTMENT_ID == FK_DEPARTMENT_ID && c.FK_DESIGNATION_ID == FK_DESIGNATION_ID && c.FK_GRADE_ID == FK_GRADE_ID && c.EMAILID == EMAILID select c).FirstOrDefault();
                    if (check == null)
                    {
                        M_EMPLOYEE product = db.M_EMPLOYEEs.Single(h => h.PK_EMP_ID == PK_EMP_ID);
                        product.EMP_NAME = EMP_NAME;
                        product.EMP_CODE = EMP_CODE;
                        product.AD_ID = AD_ID;
                        product.MOBILE_NO = MOBILE_NO;
                        product.ADDRESS = ADDRESS;
                        product.EMAILID = EMAILID;
                        product.FK_DESIGNATION_ID = Convert.ToInt32(FK_DESIGNATION_ID);
                        product.FK_BRANCH_ID = Convert.ToInt32(FK_BRANCH_ID);
                        product.FK_GRADE_ID = Convert.ToInt32(FK_GRADE_ID);
                        product.FK_DEPARTMENT_ID = Convert.ToInt32(FK_DEPARTMENT_ID);

                        product.IS_ACTIVE = 1;
                        product.CREATED_BY = CREATED_BY;
                        product.CREATION_DATE = DateTime.Now;
                        product.FK_VERTICAL_ID = 0;
                        //    product.FK_COST_CENTRE_ID = Convert.ToInt32(FK_COST_CENTRE_ID);
                        db.SubmitChanges();
                        db.Transaction.Commit();
                        Result = "true";
                    }
                    else
                    {
                        Result = "Duplicate Entry Found";
                    }
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

        public static string DeleteEmployeeData(int pkid)
        {
            string Result = "";
            using (GlobleMasterDataContext db = new GlobleMasterDataContext())
            {
                try
                {
                    db.Connection.Open();
                    db.Transaction = db.Connection.BeginTransaction();
                    M_EMPLOYEE mz = new M_EMPLOYEE();
                    var students = db.M_EMPLOYEEs.SingleOrDefault(s => s.PK_EMP_ID == pkid);
                    if (students != null)
                    {
                        students.IS_ACTIVE = 0;
                        db.SubmitChanges();
                        db.Transaction.Commit();
                        Result = "true";
                    }
                }
                catch (Exception ex)
                {
                    db.Transaction.Rollback();
                    Result = AppConstants.ErrorMessage;
                }
            }
            return Result;
        }

        public static List<Employee> getReportEmployee(string ad_id)
        {
            GlobleMasterDataContext db = new GlobleMasterDataContext();
            List<Employee> empData = new List<Employee>();

            var chk = (from au in db.M_ACCESS_USR_ROLE_MAPs
                       where au.USER_ADID == ad_id && au.IS_ACTIVE == 1 && (au.ACCESS_ROLE_ID == 1 || au.ACCESS_ROLE_ID == 7 || au.ACCESS_ROLE_ID == 6)//portal admin,hgs team
                       select new
                       { au.USER_ADID }
                     ).SingleOrDefault();
            if (chk == null)
            {
                var emp = (from e in db.M_EMPLOYEEs
                           orderby e.EMP_NAME
                           where e.IS_ACTIVE == 1 && e.AD_ID == ad_id
                           select new
                           {
                               e.AD_ID,
                               e.EMP_CODE,
                               e.EMP_NAME,
                               e.EMAILID,
                               e.FK_CLIENTID,
                               e.PK_EMP_ID
                           }).ToList();


                for (int i = 0; i < emp.Count(); i++)
                {
                    Employee obj = new Employee();
                    obj.label = emp[i].EMP_NAME + "-" + emp[i].EMP_CODE;
                    obj.EMP_NAME = emp[i].EMP_NAME;
                    obj.AD_ID = emp[i].AD_ID;
                    obj.EMP_CODE = emp[i].EMP_CODE;
                    obj.id = emp[i].AD_ID;
                    obj.CLIENT_ID = Convert.ToInt32(emp[i].FK_CLIENTID);
                    obj.PK_EMP_ID = emp[i].PK_EMP_ID;
                    empData.Add(obj);
                }
            }
            else
            {

                var emp = (from e in db.M_EMPLOYEEs
                           orderby e.EMP_NAME
                           where e.IS_ACTIVE == 1
                           select new
                           {
                               e.AD_ID,
                               e.EMP_CODE,
                               e.EMP_NAME,
                               e.EMAILID,
                               e.FK_CLIENTID,
                               e.PK_EMP_ID
                           }).ToList();


                for (int i = 0; i < emp.Count(); i++)
                {
                    Employee obj = new Employee();
                    obj.label = emp[i].EMP_NAME + "-" + emp[i].EMP_CODE;
                    obj.EMP_NAME = emp[i].EMP_NAME;
                    obj.AD_ID = emp[i].AD_ID;
                    obj.EMP_CODE = emp[i].EMP_CODE;
                    obj.id = emp[i].AD_ID;
                    obj.CLIENT_ID = Convert.ToInt32(emp[i].FK_CLIENTID);
                    obj.PK_EMP_ID = emp[i].PK_EMP_ID;
                    empData.Add(obj);
                }
            }
            return empData;
        }


    }

    public class Employee
    {

        public int PK_EMP_ID { get; set; }
        public string label { get; set; }
        public string id { get; set; }
        public string AD_ID { get; set; }
        public string PASSWORD { get; set; }
        public string EMP_NAME { get; set; }
        public string EMP_CODE { get; set; }
        public string MOBILE_NO { get; set; }
        public string ADDRESS { get; set; }
        public string EMAILID { get; set; }
        public string CREATED_BY { get; set; }
        public string IFSCODE { get; set; }
        public string DESIGNATION_NAME { get; set; }
        public string BRANCH_NAME { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        public string VERTICAL_NAME { get; set; }
        public string COST_CENTER_NAME { get; set; }
        public string GRADE { get; set; }
        public string ADMINREPORTING_TO { get; set; }
        public string FUNCTIONALREPORTING_TO { get; set; }
        public int FK_DESIGNATION_ID { get; set; }
        public int FK_BRANCH_ID { get; set; }
        public string BRANCH_CODE { get; set; }
        public string BRANCH_ADDRESS { get; set; }
        public int FK_DEPARTMENT_ID { get; set; }
        public int FK_COMPANY_ID { get; set; }
        public int FK_BANK_BRANCH_ID { get; set; }
        public int FK_VERTICAL_ID { get; set; }
        public int FK_COST_CENTRE_ID { get; set; }
        public int IS_ACTIVE { get; set; }
        public int FK_GRADE_ID { get; set; }
        public string DATE_JOINING { get; set; }
        public string DATE_BIRTH { get; set; }
        public string LAST_WORKING_DATE { get; set; }
        public string CREATION_DATE { get; set; }
        public string FUNCTIONALREPORTING_NAME { get; set; }
        public string Regional_Hr { get; set; }

        /// <summary>
        /// Added by Arti 22/Nov/18
        /// </summary>
        public int CLIENT_ID { get; set; }

    }
    public class Employee1
    {
        public string transactionData { get; set; }

    }
}
