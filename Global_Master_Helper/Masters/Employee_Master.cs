using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global_Master_Helper.Masters
{
    public class Employee_Master
    {
        public static List<Employeeold> getEmployee()
        {
            GlobleMasterDataContext db = new GlobleMasterDataContext();
            List<Employeeold> empData = new List<Employeeold>();
            var emp = (from e in db.M_EMPLOYEEs
                    //   join d in db.M_DESIGNATIONs on e.FK_DESIGNATION_ID equals d.PK_DESIGNATION_ID
                      
                       join de in db.M_DEPARTMENTs on e.FK_DEPARTMENT_ID equals de.PK_DEPARTMENT_ID
                    
                   //    join g in db.M_GRADEs on e.FK_GRADE_ID equals g.PK_GRADE_ID
                   
                       from e1 in db.M_EMPLOYEEs.Where(x => x.AD_ID == e.ADMINREPORTING_TO).DefaultIfEmpty()
                       from e2 in db.M_EMPLOYEEs.Where(x1 => x1.AD_ID == e.REGIONAL_HR).DefaultIfEmpty()
                       orderby e.EMP_NAME
                       where e.IS_ACTIVE == 1
                       select new
                       {
                           e.AD_ID,
                           e.EMP_CODE,
                           e.EMP_NAME,
                           e.MOBILE_NO,
                           e.ADDRESS,
                           e.EMAILID,
                           e.FK_DESIGNATION_ID,
                           //d.DESIGNATION_NAME,
                           e.FK_BRANCH_ID,
                           //b.BRANCH_NAME,
                           e.FK_DEPARTMENT_ID,
                           de.DEPARTMENT_NAME,
                           e.DATE_JOINING,
                           e.DATE_BIRTH,
                           e.LAST_WORKING_DATE,
                           // e.FK_DIVISION_ID,
                           // di.DIVISION_NAME,
                           e.FK_GRADE_ID,
                         //  g.GRADE,
                           e.ADMINREPORTING_TO,
                           ADMIN_MANAGER = e1.EMP_NAME,
                           e.REGIONAL_HR,
                           HR = e2.EMP_NAME,
                           // e.IFSCODE,
                           // e.FK_COST_CENTRE_ID,
                           //  c.COST_CENTER_NAME,
                           e.FK_COMPANY_ID,
                           //  co.COMPANY_NAME,
                           e.FK_CLIENTID,
                           e.PK_EMP_ID
                       }).ToList();

            //emp.FindAll(s => s.EMP_NAME(, StringComparison.OrdinalIgnoreCase) >= 0);


            for (int i = 0; i < emp.Count(); i++)
            {
                Employeeold obj = new Employeeold();
                obj.label = emp[i].EMP_NAME + "-" + emp[i].EMP_CODE;
                obj.EMP_NAME = emp[i].EMP_NAME;
                obj.AD_ID = emp[i].AD_ID;
                obj.EMP_CODE = emp[i].EMP_CODE;
                obj.id = emp[i].AD_ID;
                obj.MOBILE_NO = emp[i].MOBILE_NO;
                obj.ADDRESS = emp[i].ADDRESS;
                obj.EMAILID = emp[i].EMAILID;
                obj.FK_DESIGNATION_ID = Convert.ToInt32(emp[i].FK_DESIGNATION_ID);
               // obj.DESIGNATION_NAME = emp[i].DESIGNATION_NAME;
                obj.FK_BRANCH_ID = Convert.ToInt32(emp[i].FK_BRANCH_ID);
                //   obj.BRANCH_NAME = emp[i].BRANCH_NAME;
                obj.FK_DEPARTMENT_ID = Convert.ToInt32(emp[i].FK_DEPARTMENT_ID);
                obj.DEPARTMENT_NAME = emp[i].DEPARTMENT_NAME;
                obj.DATE_JOINING = string.Format("{0:dd-MMM-yyyy}", emp[i].DATE_JOINING);
                obj.DATE_BIRTH = string.Format("{0:dd-MMM-yyyy}", emp[i].DATE_BIRTH);
                obj.LAST_WORKING_DATE = string.Format("{0:dd-MMM-yyyy}", emp[i].LAST_WORKING_DATE);
                obj.REGIONAL_HR = emp[i].REGIONAL_HR;
                obj.HR_NAME = emp[i].HR + "-" + emp[i].REGIONAL_HR;
                obj.FK_GRADE_ID = Convert.ToInt32(emp[i].FK_GRADE_ID);
               // obj.GRADE = emp[i].GRADE;
                
                obj.FK_COMPANY_ID = Convert.ToInt32(emp[i].FK_COMPANY_ID);
                // obj.COMPANY_NAME = emp[i].COMPANY_NAME;
                obj.FUNCTIONALREPORTING_TO = emp[i].ADMINREPORTING_TO;
                obj.FUNCTIONALREPORTING_NAME = emp[i].ADMIN_MANAGER + "-" + emp[i].ADMINREPORTING_TO;
                obj.CLIENT_ID = Convert.ToInt32(emp[i].FK_CLIENTID);
                obj.PK_EMP_ID = emp[i].PK_EMP_ID;
                empData.Add(obj);
            }
            return empData;
        }

        public static string SaveEmployeeData(string EMP_NAME, string EMP_CODE, string MOBILE_NO, string ADDRESS, string EMAILID, string FK_DESIGNATION_ID, string FK_BRANCH_ID, string FK_DEPARTMENT_ID, string DATE_JOINING, string DATE_BIRTH, string LAST_WORKING_DATE, string FK_COST_CENTRE_ID, string IFSCODE, string FK_GRADE_ID, string FK_MANAGER_ID, string HR, string FK_COMPANY_ID, string Created_By, string Client_Id, string ADID)
        {
            string Result = "";
            using (GlobleMasterDataContext db = new GlobleMasterDataContext())
            {
                try
                {

                    db.Connection.Open();
                    db.Transaction = db.Connection.BeginTransaction();
                    M_EMPLOYEE check = (from c in db.M_EMPLOYEEs where c.IS_ACTIVE == 1 && c.EMP_NAME == EMP_NAME && c.EMP_CODE == EMP_CODE && c.AD_ID == ADID select c).FirstOrDefault();
                    if (check == null)
                    {
                        M_EMPLOYEE objempdata = new M_EMPLOYEE();
                        objempdata.EMP_NAME = EMP_NAME;
                        objempdata.EMP_CODE = EMP_CODE;
                        objempdata.AD_ID = ADID;
                        objempdata.PASSWORD = "A7Uu+vvWxNzxFtjQtq0a1Q==";
                        objempdata.MOBILE_NO = MOBILE_NO;
                        objempdata.ADDRESS = ADDRESS;
                        objempdata.EMAILID = EMAILID;
                        objempdata.FK_DESIGNATION_ID = Convert.ToInt32(FK_DESIGNATION_ID);
                        objempdata.FK_BRANCH_ID = Convert.ToInt32(FK_BRANCH_ID);
                        objempdata.FK_COMPANY_ID = Convert.ToInt32(FK_COMPANY_ID);
                        objempdata.FK_DEPARTMENT_ID = Convert.ToInt32(FK_DEPARTMENT_ID);
                        if (DATE_JOINING != "")
                        {
                            objempdata.DATE_JOINING = Convert.ToDateTime(DATE_JOINING);
                        }
                        if (DATE_BIRTH != "")
                        {
                            objempdata.DATE_BIRTH = Convert.ToDateTime(DATE_BIRTH);
                        }
                        if (LAST_WORKING_DATE != "")
                        {
                            objempdata.LAST_WORKING_DATE = Convert.ToDateTime(LAST_WORKING_DATE);
                        }
                        // objempdata.FK_COST_CENTRE_ID = Convert.ToInt32(FK_COST_CENTRE_ID);
                        objempdata.FK_GRADE_ID = Convert.ToInt32(FK_GRADE_ID);
                        objempdata.FUNCTIONALREPORTING_TO = FK_MANAGER_ID;
                        objempdata.ADMINREPORTING_TO = FK_MANAGER_ID;
                        objempdata.REGIONAL_HR = HR;
                        objempdata.IS_ACTIVE = 1;
                        objempdata.CREATED_BY = Created_By;
                        objempdata.CREATION_DATE = DateTime.Now;
                        objempdata.FK_VERTICAL_ID = 0;
                        objempdata.FK_DIVISION_ID = 1;
                        // objempdata.IFSCODE = IFSCODE;
                        objempdata.FK_CLIENTID = Convert.ToInt32(Client_Id);
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

        public static string UpdateEmployeeData(string EMP_NAME, string EMP_CODE, string MOBILE_NO, string ADDRESS, string EMAILID, string FK_DESIGNATION_ID, string FK_BRANCH_ID, string FK_DEPARTMENT_ID, string DATE_JOINING, string DATE_BIRTH, string LAST_WORKING_DATE, string FK_COST_CENTRE_ID, string IFSCODE, string FK_GRADE_ID, string FK_MANAGER_ID, string HR, string FK_COMPANY_ID, string Created_By, string ADID)
        {
            string Result = "";
            using (GlobleMasterDataContext db = new GlobleMasterDataContext())
            {
                try
                {
                    db.Connection.Open();
                    db.Transaction = db.Connection.BeginTransaction();

                    M_EMPLOYEE product = db.M_EMPLOYEEs.Single(h => h.EMP_CODE == EMP_CODE);
                    product.EMP_NAME = EMP_NAME;
                    product.EMP_CODE = EMP_CODE;
                    product.AD_ID = ADID;
                    product.MOBILE_NO = MOBILE_NO;
                    product.ADDRESS = ADDRESS;
                    product.EMAILID = EMAILID;
                    product.FK_DESIGNATION_ID = Convert.ToInt32(FK_DESIGNATION_ID);
                    product.FK_BRANCH_ID = Convert.ToInt32(FK_BRANCH_ID);
                    product.FK_COMPANY_ID = Convert.ToInt32(FK_COMPANY_ID);
                    product.FK_DEPARTMENT_ID = Convert.ToInt32(FK_DEPARTMENT_ID);
                    product.EMP_CODE = EMP_CODE;
                    if (DATE_JOINING != "")
                    {
                        product.DATE_JOINING = Convert.ToDateTime(DATE_JOINING);
                    }
                    if (DATE_BIRTH != "")
                    {
                        product.DATE_BIRTH = Convert.ToDateTime(DATE_BIRTH);
                    }
                    if (LAST_WORKING_DATE != "")
                    {
                        product.LAST_WORKING_DATE = Convert.ToDateTime(LAST_WORKING_DATE);
                    }
                    product.FK_GRADE_ID = Convert.ToInt32(FK_GRADE_ID);
                    product.FUNCTIONALREPORTING_TO = FK_MANAGER_ID;
                    product.ADMINREPORTING_TO = FK_MANAGER_ID;
                    //     product.IFSCODE = IFSCODE;
                    product.REGIONAL_HR = HR;
                    product.IS_ACTIVE = 1;
                    product.CREATED_BY = Created_By;
                    product.CREATION_DATE = DateTime.Now;
                    product.FK_VERTICAL_ID = 0;
                    //    product.FK_COST_CENTRE_ID = Convert.ToInt32(FK_COST_CENTRE_ID);
                    db.SubmitChanges();
                    db.Transaction.Commit();
                    Result = "true";

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
                    Logger1.Error(ex);
                    db.Transaction.Rollback();
                    Result = AppConstants.ErrorMessage;
                }
            }
            return Result;
        }

        public static List<Employeeold> getEmployeeForprocess(List<Employeeold> emplist)
        {
            List<Employeeold> empData = new List<Employeeold>();
            for (int i = 0; i < emplist.Count; i++)
            {
                GlobleMasterDataContext db = new GlobleMasterDataContext();

                var emp = (from e in db.M_EMPLOYEEs
                           //join d in db.M_DESIGNATIONs on e.FK_DESIGNATION_ID equals d.PK_DESIGNATION_ID
                          
                           join de in db.M_DEPARTMENTs on e.FK_DEPARTMENT_ID equals de.PK_DEPARTMENT_ID
                           
                          // join g in db.M_GRADEs on e.FK_GRADE_ID equals g.PK_GRADE_ID
                           from e1 in db.M_EMPLOYEEs.Where(x => x.AD_ID == e.ADMINREPORTING_TO).DefaultIfEmpty()
                           from e2 in db.M_EMPLOYEEs.Where(x1 => x1.AD_ID == e.REGIONAL_HR).DefaultIfEmpty()
                           where e.IS_ACTIVE == 1 && e.AD_ID == emplist[i].AD_ID
                           select new
                           {
                               e.AD_ID,
                               e.EMP_CODE,
                               e.EMP_NAME,
                               e.MOBILE_NO,
                               e.ADDRESS,
                               e.EMAILID,
                               e.FK_DESIGNATION_ID,
                               //d.DESIGNATION_NAME,
                               e.FK_BRANCH_ID,
                               
                               e.FK_DEPARTMENT_ID,
                               de.DEPARTMENT_NAME,
                               e.DATE_JOINING,
                               e.DATE_BIRTH,
                               e.LAST_WORKING_DATE,
                               e.FK_DIVISION_ID,
                            
                               e.FK_GRADE_ID,
                              // g.GRADE,
                               e.ADMINREPORTING_TO,
                               ADMIN_MANAGER = e1.EMP_NAME,
                               e.REGIONAL_HR,
                               HR = e2.EMP_NAME
                           }).ToList();

                foreach (var item in emp)
                {
                    Employeeold obj = new Employeeold();
                    obj.EMP_NAME = item.EMP_NAME + "-" + item.EMP_CODE;
                    obj.EMP_CODE = item.EMP_CODE;
                    obj.AD_ID = item.AD_ID;
                    obj.MOBILE_NO = item.MOBILE_NO;
                    obj.ADDRESS = item.ADDRESS;
                    obj.EMAILID = item.EMAILID;
                    obj.FK_DESIGNATION_ID = Convert.ToInt32(item.FK_DESIGNATION_ID);
                   // obj.DESIGNATION_NAME = item.DESIGNATION_NAME;
                    obj.FK_BRANCH_ID = Convert.ToInt32(item.FK_BRANCH_ID);
                    //   obj.BRANCH_CODE = item.BRANCH_CODE;
                    //  obj.BRANCH_ADDRESS = item.BRANCH_ADDRESS;
                    //  obj.BRANCH_NAME = item.BRANCH_NAME;
                    obj.FK_DEPARTMENT_ID = Convert.ToInt32(item.FK_DEPARTMENT_ID);
                    obj.DEPARTMENT_NAME = item.DEPARTMENT_NAME;
                    obj.DATE_JOINING = Convert.ToString(item.DATE_JOINING);
                    obj.DATE_BIRTH = Convert.ToString(item.DATE_BIRTH);
                    obj.LAST_WORKING_DATE = Convert.ToString(item.LAST_WORKING_DATE);
                    obj.REGIONAL_HR = item.REGIONAL_HR;
                    obj.HR_NAME = item.HR + "-" + item.REGIONAL_HR;
                    obj.FK_GRADE_ID = Convert.ToInt32(item.FK_GRADE_ID);
                   // obj.GRADE = item.GRADE;
                    obj.FK_DIVISION_ID = Convert.ToInt32(item.FK_DIVISION_ID);
                    // obj.DIVISION = item.DIVISION_NAME;
                    obj.FUNCTIONALREPORTING_TO = item.ADMINREPORTING_TO;
                    obj.FUNCTIONALREPORTING_NAME = item.ADMIN_MANAGER + "-" + item.ADMINREPORTING_TO;
                    empData.Add(obj);
                }

            }
            return empData;
        }
        //Added by Arti 22/Nov/18
        public static List<Employee> getEmployeeDataForprocess(int client_id)
        {
            GlobleMasterDataContext db = new GlobleMasterDataContext();
            List<Employee> empData = new List<Employee>();
            var emp = (from e in db.M_EMPLOYEEs
                           //join b in db.M_BRANCHMSTs on e.FK_BRANCH_ID equals b.PK_BRANCHID
                           //join de in db.M_DEPARTMENTs on e.FK_DEPARTMENT_ID equals de.PK_DEPARTMENT_ID
                           //join ar in db.M_AREAMSTs on b.FK_AREAID equals ar.PK_AREAID
                           //join re in db.M_REGIONMSTs on ar.FK_REGIONID equals re.PK_REGIONID
                           //join l in db.M_LOCATIONMSTs on b.FK_CITYID equals l.FK_CITYID
                       where e.IS_ACTIVE == 1 && e.FK_CLIENTID == client_id
                       select new
                       {
                           e.AD_ID,
                           e.EMP_CODE,
                           e.EMP_NAME,
                           e.EMAILID,

                       }).Distinct();//.Take(30)
            foreach (var item in emp)
            {
                Employee obj = new Employee();
                obj.label = item.EMP_NAME + "-" + item.EMP_CODE;
                obj.EMP_CODE = item.EMP_CODE;
                obj.id = item.AD_ID;
                obj.EMAILID = item.EMAILID;

                empData.Add(obj);
            }

            return empData;
        }
        //Added by Arti 22/Nov/18
        public static List<Employee> getDeptForprocess(List<Employee> obj1)
        {
            GlobleMasterDataContext db = new GlobleMasterDataContext();
            List<Employee> empData = new List<Employee>();
            for (int i = 0; i < obj1.Count; i++)
            {
                var emp = (from e in db.M_EMPLOYEEs
                           where e.IS_ACTIVE == 1
                            && e.AD_ID == obj1[i].AD_ID
                           select new
                           {
                               e.AD_ID,
                               e.EMP_NAME
                           }).Distinct().ToList();

                foreach (var item in emp)
                {
                    Employee obj = new Employee();
                    obj.label = item.EMP_NAME;
                    obj.EMP_NAME = item.EMP_NAME;
                    obj.id = item.AD_ID;
                    obj.AD_ID = item.AD_ID;
                    empData.Add(obj);
                }
            }

            return empData;
        }
    }

    public class Employeeold
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

        public string DATE_JOINING1 { get; set; }
        public string DATE_BIRTH1 { get; set; }
        public string LAST_WORKING_DATE1 { get; set; }
        public string CREATION_DATE1 { get; set; }
        public int FK_DIVISION_ID { get; set; }
        public string REGIONAL_HR { get; set; }
        public string DIVISION { get; set; }
        public string FUNCTIONALREPORTING_NAME { get; set; }
        public string HR_NAME { get; set; }
        public string COMPANY_NAME { get; set; }

        public int FK_REGION_ID { get; set; }
        public int FK_LOCATION_ID { get; set; }
        public string LOCATION_NAME { get; set; }
        public int PK_BRANCHID { get; set; }
        public int PK_LOCATIONID { get; set; }
        public int PK_REGIONID { get; set; }
        public string REGION { get; set; }
        public int PK_DEPT_ID { get; set; }
        public string DEPT_NAME { get; set; }
        public int CLIENT_ID { get; set; }

    }
}
