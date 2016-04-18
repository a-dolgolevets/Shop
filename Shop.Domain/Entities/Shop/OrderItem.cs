using Shop.Domain.Base;

namespace Shop.Domain.Entities.Shop
{
    public class OrderItem : BaseEntity
    {
        public decimal Price { get; set; }

        public int Amount { get; set; }

        public int OrderId { get; set; }
        public virtual Order Order { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}