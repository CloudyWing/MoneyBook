using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(MoneyBook.Web.Startup))]
namespace MoneyBook.Web {
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
