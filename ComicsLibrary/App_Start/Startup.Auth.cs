using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using ComicsLibrary.Common.Services;
using System.Web.Mvc;

namespace ComicsLibrary
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            Configure(app);
        }

        private static void Configure(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType("GoogleCookie");

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "GoogleCookie"
            });

            var appKeys = DependencyResolver.Current.GetService<IAppKeys>();
            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions
            {
                ClientId = appKeys.GoogleClientId,
                ClientSecret = appKeys.GoogleClientSecret
            });
        }
    }
}