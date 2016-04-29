using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Filter.WebUI.Startup))]
namespace Filter.WebUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
