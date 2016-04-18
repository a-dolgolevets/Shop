using System;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Owin;
using Shop.Common.Facade;
using Shop.Domain.Entities.Identity;

namespace Shop.DependencyInjection.Config
{
    public class InjectionConfig
    {
        public string ConnectionString { get; set; }

        public bool SqlDebugLogEnabled { get; set; }

        public IAppBuilder AppBuilder { get; set; }

        public Action<UserManager<User, int>, IDependencyResolver> UserManagerSetupAction { get; set; }

        public Func<IOwinContext> GetOwinContextFunc { get; set; }
    }
}