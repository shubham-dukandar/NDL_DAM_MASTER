using System.Web;
using System.Web.Mvc;

namespace NDL_DAM_MASTER
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
