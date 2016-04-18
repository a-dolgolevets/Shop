using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Shop.Domain.Entities.Shop;
using Shop.ServicesFacade.Base;
using Shop.ServicesFacade.Concrete.Shop;
using Shop.ViewModel.Shop;
using Constants = Shop.Web.Helpers.Constants;

namespace Shop.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService cartService;
        private readonly IService<Product> productService;

        public CartController(ICartService cartService, IService<Product> productService)
        {
            this.cartService = cartService;
            this.productService = productService;
        }

        [ChildActionOnly]
        public ActionResult Get()
        {
            var cart = Session[Constants.CartSessionKey] as CartViewModel;
            if (User.Identity.IsAuthenticated)
            {
                cart = GetCart();
            }

            return PartialView("_Cart", cart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddItem(int productId)
        {
            var product = await productService.GetAsync(productId);

            var cart = GetCart();
            cart = await cartService.AddToCart(cart, product, 1);

            Session[Constants.CartSessionKey] = cart;

            return PartialView("_Cart", cart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteItem(CartItemViewModel model)
        {
            var cart = GetCart();
            cart = await cartService.DeleteFromCart(cart, model);

            Session[Constants.CartSessionKey] = cart;

            return PartialView("_Cart", cart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Empty()
        {
            var cart = GetCart();
            await cartService.EmptyCart(cart);

            Session[Constants.CartSessionKey] = null;

            return PartialView("_Cart");
        }

        #region Private methods
        // Need to avoid async because it is used in child action
        private CartViewModel GetCart()
        {
            var userId = User.Identity.GetUserId<int>();
            var cart = Session[Constants.CartSessionKey] as CartViewModel;
            if (cart == null)
            {
                cart = cartService.CreateCart(Session.SessionID, userId);
            }
            else if (userId != 0 && !cart.IsPersistent)
            {
                cart = cartService.CreateCartAfterLogin(cart, Session.SessionID, userId);
            }

            Session[Constants.CartSessionKey] = cart;

            return cart;
        }
        #endregion
    }
}