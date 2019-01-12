using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AskFm.Startup))]
namespace AskFm
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
