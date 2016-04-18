using Shop.Domain.Enums;

namespace Shop.ViewModel.Shop
{
    public class OrderViewModel
    {
        public decimal Total { get; set; }

        public string Comment { get; set; }

        public CustomerType CustomerType { get; set; }
    }
}