using System.Collections.Generic;
using Shop.Domain.Base;

namespace Shop.Domain.Entities.Shop
{
    public class ProductCategory : BaseEntity
    {
        public string Title { get; set; }

        public virtual ICollection<Product> Products {get; set; }
    }
}