using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(APS.Startup))]
namespace APS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {

            // Map SignalR to project
            

            ConfigureAuth(app);
            var idProvider = new CustomUserIdProvider();

            GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), () => idProvider);
            app.MapSignalR();
        }
    }
}
