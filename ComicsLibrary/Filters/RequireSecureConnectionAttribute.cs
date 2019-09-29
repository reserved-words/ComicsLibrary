using System;
using System.Web.Mvc;

namespace ComicsLibrary.Filters
{
    public class RequireSecureConnectionAttribute : RequireHttpsAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException(nameof(filterContext));
            }

            if (filterContext.HttpContext.Request.IsLocal)
            {
                return;
            }

            base.OnAuthorization(filterContext);
        }
    }
}