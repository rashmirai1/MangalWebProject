using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MangalWebProject.Startup))]
namespace MangalWebProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
