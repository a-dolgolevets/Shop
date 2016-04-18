using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Shop.Common.Facade;
using Shop.DependencyInjection.Config;
using Shop.DependencyInjection.Resolver;
using Shop.Domain.Entities.Identity;
using Shop.Mapping;
using Shop.Repositories.Base;
using Shop.Repositories.Context;
using Shop.RepositoriesFacade.Base;
using Shop.Services.Base;
using Shop.Services.Concrete.Common;
using Shop.Services.Concrete.Identity;
using Shop.Services.Concrete.Shop;
using Shop.ServicesFacade.Base;
using Shop.ServicesFacade.Concrete.Common;
using Shop.ServicesFacade.Concrete.Identity;
using Shop.ServicesFacade.Concrete.Shop;
using SimpleInjector;
using SimpleInjector.Advanced;
using SimpleInjector.Integration.Web;

namespace Shop.DependencyInjection.SimpleInjector
{
    internal static class ContainerProvider
    {
        private static Container container;

        public static Container GetContainer()
        {
            return container;
        }

        public static IDependencyResolver GetResolver()
        {
            return container.GetInstance<IDependencyResolver>();
        }

        public static void Configure(InjectionConfig config)
        {
            if (container == null)
            {
                container = new Container();
                container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

                RegisterShop(config);

                RegisterMvc();
                RegisterOwinAndIdentity(config);

                container.Verify();
            }
        }

        #region Registrations

        private static void RegisterMvc()
        {
            container.RegisterMvcControllers(Assembly.GetCallingAssembly());
            container.RegisterMvcIntegratedFilterProvider();
        }

        private static void RegisterOwinAndIdentity(InjectionConfig config)
        {
            container.RegisterSingleton(config.AppBuilder);

            container.RegisterPerWebRequest<UserManager<User, int>>();
            container.RegisterPerWebRequest<RoleManager<Role, int>>();

            container.RegisterPerWebRequest<IUserStore<User, int>>(() => new UserStore<User, Role, int, UserLogin, UserRole, UserClaim>(container.GetInstance<IDatabaseContext>() as DatabaseContext));
            container.RegisterPerWebRequest<IRoleStore<Role, int>>(() => new RoleStore<Role, int, UserRole>(container.GetInstance<IDatabaseContext>() as DatabaseContext));

            container.RegisterInitializer<UserManager<User, int>>(manager => config.UserManagerSetupAction(manager, container.GetInstance<IDependencyResolver>()));

            container.RegisterPerWebRequest<SignInManager<User, int>>();

            // Owin authentication for SignInManager 
            // There is no OWIN context at verification time, so we need to mock it
            container.RegisterPerWebRequest(() => container.IsVerifying() 
                ? new OwinContext(new Dictionary<string, object>()).Authentication
                : config.GetOwinContextFunc().Authentication);
        }

        private static void RegisterShop(InjectionConfig config)
        {
            container.RegisterSingleton<IDependencyResolver>(new DependencyResolver(container));
            container.RegisterSingleton<IMapper, Mapper>();

            container.Register<IDatabaseContext>(() => new DatabaseContext(config.ConnectionString, config.SqlDebugLogEnabled), Lifestyle.Scoped);

            RegisterRepositories();
            RegisterServices();
        }

        private static void RegisterRepositories()
        {
            container.Register(typeof(IRepository<>), typeof(Repository<>), Lifestyle.Scoped);
        }

        private static void RegisterServices()
        {
            container.Register(typeof(IService<>), typeof(Service<>), Lifestyle.Scoped);

            container.Register<ICartService, CartService>(Lifestyle.Scoped);

            container.Register<IEmailService, EmailService>(Lifestyle.Scoped);
            container.Register<ISmsService, SmsService>(Lifestyle.Scoped);

            container.Register<IUserService, UserService>(Lifestyle.Scoped);
        }

        #endregion
    }
}