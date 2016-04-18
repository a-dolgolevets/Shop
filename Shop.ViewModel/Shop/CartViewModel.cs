using System.Collections.Generic;

namespace Shop.ViewModel.Shop
{
    public class CartViewModel
    {
        public CartViewModel()
        {
            
        }

        public CartViewModel(string sessionId)
        {
            CustomerSessionId = sessionId;

            Items = new List<CartItemViewModel>();
        }

        public CartViewModel(string sessionId, int customerId)
            : this(sessionId)
        {
            CustomerId = customerId;
            IsPersistent = customerId > 0;
        }

        public int Id { get; set; }

        public string CustomerSessionId { get; set; }

        public int CustomerId { get; set; }

        public bool IsPersistent { get; set; }

        public IList<CartItemViewModel> Items { get; set; }
    }
}