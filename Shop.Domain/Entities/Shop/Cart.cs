using System.Collections.Generic;
using Shop.Domain.Base;
using Shop.Domain.Entities.Identity;
using Shop.Domain.Enums;

namespace Shop.Domain.Entities.Shop
{
    public class Cart : BaseEntity
    {
        public string CustomerSessionId { get; set; }

        public CustomerType CustomerType { get; set; }

        public int CustomerId { get; set; }
        public virtual User Customer { get; set; }

        public virtual ICollection<CartItem> Items { get; set; }
    }
}