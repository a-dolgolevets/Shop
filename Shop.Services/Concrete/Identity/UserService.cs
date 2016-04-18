using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Shop.Domain.Entities.Identity;
using Shop.ServicesFacade.Concrete.Identity;

namespace Shop.Services.Concrete.Identity
{
    public class UserService : IUserService
    {
        public UserService(UserManager<User, int> userManager, SignInManager<User, int> signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public UserManager<User, int> UserManager { get; private set; }

        public SignInManager<User, int> SignInManager { get; private set; }
    }
}