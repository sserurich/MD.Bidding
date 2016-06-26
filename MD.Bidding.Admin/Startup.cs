using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MD.Bidding.Admin.Startup))]

namespace MD.Bidding.Admin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

        }
    }
}