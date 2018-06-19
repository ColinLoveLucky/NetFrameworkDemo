using QK.QAPP.Infrastructure;
using System.Web;
using System.Web.Mvc;

namespace QK.QAPP.SalesTest
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new CustomHandleErrorAttribute());
            //filters.Add(new HandleErrorAttribute());
            filters.Add(new AuthorizationAttribute());
            filters.Add(new LogicalActionFilterAttribute());
        }
    }
}
