using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global_Master_Helper.Masters.DealNote
{
    public class Currency_Master
    {
        public static List<Currency> getCurrency()
        {
            List<Currency> currency = new List<Currency>();
            try
            {
                using (GlobleMasterDataContext db = new GlobleMasterDataContext())
                {
                    var curr = (from a in db.M_CURRENCies

                                orderby a.CURRENCY_NAME
                                where a.IS_ACTIVE == 1
                                select new
                                {
                                    a.PK_CURRENCY_ID,
                                    a.CURRENCY_NAME,
                                    a.CURRENCY_DESC,
                                    a.FK_CLIENTID,
                                    a.CREATED_BY,
                                    a.CREATED_DATE,
                                    a.CURRENCY_RATE,
                                }).ToList();

                    foreach (var data in curr)
                    {
                        Currency a = new Currency();
                        a.id = Convert.ToInt32(data.PK_CURRENCY_ID);
                        //  a.label = new CultureInfo("en").TextInfo.ToTitleCase(data.CURRENCY_NAME.ToLower());
                        a.label = data.CURRENCY_NAME;
                        a.Currency_Desc = new CultureInfo("en").TextInfo.ToTitleCase(data.CURRENCY_DESC.ToLower());
                        a.FK_CLIENTID = Convert.ToInt32(data.FK_CLIENTID);
                        a.EMP_NAME = data.CREATED_BY;
                        a.CREATION_DATE = Convert.ToDateTime(data.CREATED_DATE);
                        a.CURRENCY_RATE = Convert.ToDecimal(data.CURRENCY_RATE);
                        currency.Add(a);
                    }
                }
                // Logger1.Activity("Department List Return From Helper");
            }
            catch (Exception ex)
            {
                //Logger1.Error(ex);
            }
            return currency;
        }
    }
    public class Currency
    {
        public int id { get; set; }
        public string label { get; set; }
        public string Currency_Desc { get; set; }
        public string EMP_NAME { get; set; }
        public DateTime CREATION_DATE { get; set; }
        public int FK_CLIENTID { get; set; }
        public decimal CURRENCY_RATE { get; set; }
    }
}
