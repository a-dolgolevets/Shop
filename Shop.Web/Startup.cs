using System.Configuration;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin;
using Owin;
using Shop.DependencyInjection;
using Shop.DependencyInjection.Config;
using SimpleInjector.Integration.Web.Mvc;

[assembly: OwinStartupAttribute(typeof(Shop.Web.Startup))]
namespace Shop.Web
{
    public partial class Startup
    {
        private static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["DatabaseContext"].ConnectionString;
        private static readonly bool SqlDebugLogEnabled = bool.Parse(ConfigurationManager.AppSettings["Database.SqlDebugLogEnabled"]);

        private static readonly InjectionConfig InjectionConfig = new InjectionConfig
        {
            ConnectionString = ConnectionString,
            SqlDebugLogEnabled = SqlDebugLogEnabled
        };

        public void Configuration(IAppBuilder app)
        {
            InjectionConfig.AppBuilder = app;
            InjectionConfig.UserManagerSetupAction = ConfigureIdentity;
            InjectionConfig.GetOwinContextFunc = () => HttpContext.Current.GetOwinContext();

            InjectionBootstrapper.Configure(InjectionConfig);
            var container = InjectionBootstrapper.GetContainer();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
            ConfigureAuth(app);
        }
    }
}
