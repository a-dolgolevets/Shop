using Microsoft.AspNet.Identity.EntityFramework;
using Shop.Domain.Base;

namespace Shop.Domain.Entities.Identity
{
    public class UserLogin : IdentityUserLogin<int>, IBaseEntity
    {
        public int Id { get; set; }
    }
}