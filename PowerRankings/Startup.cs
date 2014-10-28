using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PowerRankings.Startup))]
namespace PowerRankings
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
