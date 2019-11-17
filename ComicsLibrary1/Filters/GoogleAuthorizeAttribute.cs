using ComicsLibrary.Common.Services;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace ComicsLibrary.Filters
{
    public class GoogleAuthorizeAttribute : AuthorizeAttribute
    {
        private IAppKeys _appKeys = DependencyResolver.Current.GetService<IAppKeys>();

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.RequestContext.HttpContext.Request.IsAuthenticated)
            {
                var validUser = _appKeys.ValidGmailLogin;
                var user = filterContext.HttpContext?.User as ClaimsPrincipal;
                var email = user?.FindFirst(ClaimTypes.Email)?.Value;
                if (email == null || !email.Equals(validUser))
                {
                    var urlHelper = new UrlHelper(filterContext.RequestContext);
                    filterContext.Result = new RedirectResult(urlHelper.Action("Unauthorized", "Error"));
                    return;
                }
            }

            base.OnAuthorization(filterContext);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.HttpContext
                .GetOwinContext().Authentication
                .Challenge("Google");
        }
    }
}