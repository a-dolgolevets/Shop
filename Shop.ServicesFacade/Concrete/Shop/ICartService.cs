using System.Threading.Tasks;
using Shop.Domain.Entities.Shop;
using Shop.ServicesFacade.Base;
using Shop.ViewModel.Shop;

namespace Shop.ServicesFacade.Concrete.Shop
{
    public interface ICartService : IService<Cart>
    {
        CartViewModel CreateCart(string sessionId, int customerId = 0);

        CartViewModel CreateCartAfterLogin(CartViewModel cart, string sessionId, int customerId);

        Task<CartViewModel> AddToCart(CartViewModel cart, Product product, int amount);

        Task<CartViewModel> DeleteFromCart(CartViewModel cart, CartItemViewModel cartItem);

        Task EmptyCart(CartViewModel cart);
    }
}