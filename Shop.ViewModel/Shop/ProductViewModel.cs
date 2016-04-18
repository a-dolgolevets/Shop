namespace Shop.ViewModel.Shop
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        public decimal Price { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string PhotoUrl { get; set; }

        public int CategoryId { get; set; }

        public string CategoryTitle { get; set; }
    }
}