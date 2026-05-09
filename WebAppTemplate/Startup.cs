using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(JamesPetBoarding.Startup))]
namespace JamesPetBoarding
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
