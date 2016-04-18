using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using Owin;
using Shop.Common.Facade;
using Shop.Domain.Entities.Identity;
using Shop.ServicesFacade.Concrete.Common;

namespace Shop.Web
{
    public partial class Startup
    {
        public void ConfigureIdentity(UserManager<User, int> manager, IDependencyResolver resolver)
        {
            var appBuilder = resolver.Resolve<IAppBuilder>();

            manager.EmailService = resolver.Resolve<IEmailService>();
            manager.SmsService = resolver.Resolve<ISmsService>();

            manager.UserValidator = new UserValidator<User, int>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            var dataProtectionProvider = appBuilder.GetDataProtectionProvider();
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<User, int>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
        }
    }
}