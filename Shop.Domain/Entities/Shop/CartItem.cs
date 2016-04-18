using Shop.Domain.Base;

namespace Shop.Domain.Entities.Shop
{
    public class CartItem : BaseEntity
    {
        public string Title { get; set; }

        public decimal Price { get; set; }

        public int Amount { get; set; }

        public int CartId { get; set; }
        public virtual Cart Cart { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}