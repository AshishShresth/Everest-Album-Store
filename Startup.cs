using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EverestAlbumStore.Startup))]
namespace EverestAlbumStore
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
