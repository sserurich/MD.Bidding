using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MD.Bidding.Web.Startup))]

namespace MD.Bidding.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

        }
    }
}