using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Shop.Common.Facade;
using Shop.Domain.Entities.Shop;
using Shop.Domain.Enums;
using Shop.ServicesFacade.Base;
using Shop.ServicesFacade.Concrete.Shop;
using Shop.ViewModel.Shop;
using Shop.Web.Helpers;

namespace Shop.Web.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ICartService cartService;
        private readonly IService<Order> orderService;
        private readonly IMapper mapper;

        public CheckoutController(ICartService cartService, IService<Order> orderService, IMapper mapper)
        {
            this.cartService = cartService;
            this.orderService = orderService;
            this.mapper = mapper;
        }

        public ActionResult Index()
        {
            var cart = Session[Constants.CartSessionKey] as CartViewModel;
            return cart == null ? View("EmptyCart") : View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ConfirmCheckout(OrderViewModel model)
        {
            var cart = Session[Constants.CartSessionKey] as CartViewModel;

            if (cart == null)
            {
                return View("EmptyCart");
            }

            if (ModelState.IsValid)
            {
                var entity = mapper.Map<Order>(model);
                entity.Items = mapper.Map<IList<OrderItem>>(cart.Items);
                entity.Total = entity.Items.Sum(x => x.Amount * x.Price);

                if (cart.IsPersistent)
                {
                    entity.CustomerId = cart.CustomerId;
                    entity.CustomerType = CustomerType.Registered;
                }

                var result = await orderService.CreateAsync(entity);
                if (result.IsSuccess)
                {
                    await cartService.EmptyCart(cart);
                    Session[Constants.CartSessionKey] = null;

                    return View("CheckoutCompleted");    
                }
            }

            return View("Index", model);
        }
    }
}