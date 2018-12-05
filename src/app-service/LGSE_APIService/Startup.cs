using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(LGSE_APIService.Startup))]

namespace LGSE_APIService
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
            
        }
    }
}