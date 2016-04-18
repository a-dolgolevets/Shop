namespace Shop.ViewModel.Shop
{
    public class CartItemViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }

        public int Amount { get; set; }

        public int CartId { get; set; }

        public int ProductId { get; set; }
    }
}