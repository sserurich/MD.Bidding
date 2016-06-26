using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MD.Bidding.Web.Umbraco.Startup))]
namespace MD.Bidding.Web.Umbraco
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
