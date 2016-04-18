using System.Collections.Generic;
using Shop.Domain.Base;
using Shop.Domain.Entities.Identity;
using Shop.Domain.Enums;

namespace Shop.Domain.Entities.Shop
{
    public class Order : BaseEntity
    {
        public decimal Total { get; set; }

        public string Comment { get; set; }

        public CustomerType CustomerType { get; set; }

        public int? CustomerId { get; set; }
        public virtual User Customer { get; set; }

        public virtual ICollection<OrderItem> Items { get; set; }
    }
}