using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(ComicsLibrary.Startup))]
namespace ComicsLibrary
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
