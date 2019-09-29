using ComicsLibrary.Filters;
using System.Web;
using System.Web.Mvc;

namespace ComicsLibrary
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new GoogleAuthorizeAttribute());
            filters.Add(new HandleErrorAttribute());
            filters.Add(new RequireSecureConnectionAttribute());
        }
    }
}
