using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Shop.Domain.Entities.Identity;

namespace Shop.ServicesFacade.Concrete.Identity
{
    public interface IUserService
    {
        UserManager<User, int> UserManager { get; }

        SignInManager<User, int> SignInManager { get; }
    }
}