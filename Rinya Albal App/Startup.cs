using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Rinya_Albal_App.Startup))]
namespace Rinya_Albal_App
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
