using Microsoft.AspNet.Identity.EntityFramework;
using Shop.Domain.Base;

namespace Shop.Domain.Entities.Identity
{
    public class Role : IdentityRole<int, UserRole>, IBaseEntity
    {

    }
}