using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;
using Shop.Domain.Base;
using Shop.Domain.Entities.Shop;

namespace Shop.Domain.Entities.Identity
{
    public class User : IdentityUser<int, UserLogin, UserRole, UserClaim>, IBaseEntity
    {
        public virtual ICollection<Cart> Carts { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}