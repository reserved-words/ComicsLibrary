using ComicsLibrary.ExtensionMethods;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace ComicsLibrary.Filters
{
    public class RequireSecureConnectionAttribute : RequireHttpsAttribute
    {
        public override void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException(nameof(filterContext));
            }

            if (filterContext.HttpContext.Request.IsLocal())
            {
                return;
            }

            base.OnAuthorization(filterContext);
        }
    }
}