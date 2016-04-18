using System.Linq;
using System.Threading.Tasks;
using Shop.Common.Facade;
using Shop.Domain.Entities.Shop;
using Shop.RepositoriesFacade.Base;
using Shop.Services.Base;
using Shop.ServicesFacade.Concrete.Shop;
using Shop.ViewModel.Shop;

namespace Shop.Services.Concrete.Shop
{
    public class CartService : Service<Cart>, ICartService
    {
        private readonly IRepository<CartItem> cartItemRepository;
        private readonly IMapper mapper;

        public CartService(IRepository<Cart> repository, IRepository<CartItem> cartItemRepository, IMapper mapper) 
            : base(repository)
        {
            this.cartItemRepository = cartItemRepository;
            this.mapper = mapper;
        }

        public CartViewModel CreateCart(string sessionId, int customerId)
        {
            var cart = new CartViewModel(sessionId, customerId);
            if (cart.IsPersistent)
            {
                var existing = Get(x => x.CustomerId == customerId);
                if (existing == null)
                {
                    var result = Create(mapper.Map<Cart>(cart));
                    existing = result.Entity;
                }

                cart = mapper.Map<CartViewModel>(existing);
            }

            return cart;
        }

        public CartViewModel CreateCartAfterLogin(CartViewModel cart, string sessionId, int customerId)
        {
            cart.IsPersistent = true;
            cart.CustomerSessionId = sessionId;
            cart.CustomerId = customerId;

            if (cart.Items.Any())
            {
                var existing = Get(x => x.CustomerId == customerId);
                if (existing != null)
                {
                    Repository.Delete(existing.Id);
                }    
            }

            var result = Create(mapper.Map<Cart>(cart));
            cart.Id = result.Entity.Id;

            return cart;
        }

        public async Task<CartViewModel> AddToCart(CartViewModel cart, Product product, int amount)
        {
            var existingItem = cart.Items.SingleOrDefault(x => x.ProductId == product.Id);
            if (existingItem != null)
            {
                existingItem.Amount += amount;
                if (cart.IsPersistent)
                {
                    cartItemRepository.Update(mapper.Map<CartItem>(existingItem));
                    await SaveChangesAsync();
                }
            }
            else
            {
                var cartItem = mapper.Map<CartItemViewModel>(product);
                cartItem.Amount = amount;
                cartItem.CartId = cart.Id;
                cart.Items.Add(cartItem);

                if (cart.IsPersistent)
                {
                    var entity = cartItemRepository.Create(mapper.Map<CartItem>(cartItem));
                    await SaveChangesAsync();
                    cartItem.Id = entity.Id;
                }
            }

            return cart;
        }

        public async Task<CartViewModel> DeleteFromCart(CartViewModel cart, CartItemViewModel cartItem)
        {
            var existingItem = cart.Items.SingleOrDefault(x => x.ProductId == cartItem.ProductId);
            if (existingItem != null)
            {
                cart.Items.Remove(existingItem);

                if (cart.Items.Count == 0)
                {
                    await EmptyCart(cart);
                    return null;
                }

                if (cart.IsPersistent)
                {
                    cartItemRepository.Delete(existingItem.Id);
                    await SaveChangesAsync();
                }
            }

            return cart;
        }

        public async Task EmptyCart(CartViewModel cart)
        {
            if (cart != null && cart.IsPersistent)
            {
                await DeleteAsync(cart.Id);
            }
        }
    }
}