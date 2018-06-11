using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebLibraryProject2.Startup))]
namespace WebLibraryProject2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
