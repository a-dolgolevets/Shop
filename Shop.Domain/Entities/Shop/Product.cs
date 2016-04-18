using Shop.Domain.Base;

namespace Shop.Domain.Entities.Shop
{
    public class Product : BaseEntity
    {
        public decimal Price { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string PhotoUrl { get; set; }

        public int CategoryId { get; set; }
        public virtual ProductCategory Category { get; set; }
    }
}