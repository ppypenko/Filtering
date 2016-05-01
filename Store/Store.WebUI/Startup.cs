using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Store.WebUI.Startup))]
namespace Store.WebUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
