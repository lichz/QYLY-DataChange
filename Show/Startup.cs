using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Show.Startup))]
namespace Show
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
