using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global_Master_Helper.Masters.DealNote
{
  public  class Department_Master
    {
        public static List<Department> getDepartment()
        {
            List<Department> department = new List<Department>();
            try
            {
                using (GlobleMasterDataContext db = new GlobleMasterDataContext())
                {
                    var Dept = (from a in db.M_DEPARTMENTs
                                    //join me in db.M_EMPLOYEEs on a.DEPARTMENT_HEAD equals me.AD_ID
                                orderby a.DEPARTMENT_NAME
                                where a.IS_ACTIVE == 1 //&& me.IS_ACTIVE == 1
                                select new
                                {
                                    a.PK_DEPARTMENT_ID,
                                    a.DEPARTMENT_NAME,
                                    a.DEPARTMENT_DESC,
                                    a.DEPARTMENT_HEAD,
                                   // me.EMP_NAME,
                                    a.FK_CLIENTID,
                                    a.CREATED_BY,
                                    a.CREATION_DATE
                                }).ToList();

                    foreach (var data in Dept)
                    {
                        Department a = new Department();
                        a.id = Convert.ToInt32(data.PK_DEPARTMENT_ID);
                        a.label = new CultureInfo("en").TextInfo.ToTitleCase(data.DEPARTMENT_NAME.ToLower());
                        a.DEPARTMENT_DESC = new CultureInfo("en").TextInfo.ToTitleCase(data.DEPARTMENT_DESC.ToLower());
                        a.FK_CLIENTID = Convert.ToInt32(data.FK_CLIENTID);
                        a.EMP_NAME = data.CREATED_BY;
                        a.CREATION_DATE = Convert.ToDateTime(data.CREATION_DATE);
                        a.DEPT_HEAD = data.DEPARTMENT_HEAD;
                        //a.EMP_NAME = new CultureInfo("en").TextInfo.ToTitleCase(data.EMP_NAME.ToLower());
                        department.Add(a);
                    }
                }
                // Logger1.Activity("Department List Return From Helper");
            }
            catch (Exception ex)
            {
                //Logger1.Error(ex);
            }
            return department;
        }


    }
    public class Department
    {
        public int id { get; set; }
        public string label { get; set; }
        public string DEPARTMENT_DESC { get; set; }
        public string DEPT_HEAD { get; set; }
        public string EMP_NAME { get; set; }
        public DateTime CREATION_DATE { get; set; }
        public int FK_CLIENTID { get; set; }
        public string DEPTPKID { get; set; }
    }
}
