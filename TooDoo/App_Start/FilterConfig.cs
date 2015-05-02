using System.Web;
using System.Web.Mvc;
using TooDoo.Helpers;

namespace TooDoo
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new CustomHandleErrorAttribute());
        }
    }
}
