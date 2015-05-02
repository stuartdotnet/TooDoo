using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TooDoo.Startup))]
namespace TooDoo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
