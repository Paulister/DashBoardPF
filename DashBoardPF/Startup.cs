using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DashBoardPF.Startup))]
namespace DashBoardPF
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
