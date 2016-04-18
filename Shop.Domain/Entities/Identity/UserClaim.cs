using Microsoft.AspNet.Identity.EntityFramework;
using Shop.Domain.Base;

namespace Shop.Domain.Entities.Identity
{
    public class UserClaim : IdentityUserClaim<int>, IBaseEntity
    {
         
    }
}