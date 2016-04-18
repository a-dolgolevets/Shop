using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Shop.Common.Facade;
using Shop.Domain.Entities.Shop;
using Shop.ServicesFacade.Base;
using Shop.ViewModel.Shop;

namespace Shop.Web.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IService<Order> orderService;
        private readonly IMapper mapper;

        public OrderController(IService<Order> orderService, IMapper mapper)
        {
            this.orderService = orderService;
            this.mapper = mapper;
        }

        public async Task<ActionResult> Index()
        {
            var entities = await orderService.GetAllAsync();
            var model = mapper.Map<IList<OrderViewModel>>(entities);
            return View(model);
        }
    }
}
