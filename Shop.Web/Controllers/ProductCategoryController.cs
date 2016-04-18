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
    public class ProductCategoryController : Controller
    {
        private readonly IService<ProductCategory> productCategoryService;
        private readonly IMapper mapper;

        public ProductCategoryController(IService<ProductCategory> productCategoryService, IMapper mapper)
        {
            this.productCategoryService = productCategoryService;
            this.mapper = mapper;
        }

        public async Task<ActionResult> Index()
        {
            var entities = await productCategoryService.GetAllAsync();
            var model = mapper.Map<IList<ProductCategoryViewModel>>(entities);
            return View(model);
        }

        public async Task<ActionResult> Details(int id)
        {
            var productCategory = await productCategoryService.GetAsync(id);
            if (productCategory == null)
            {
                return HttpNotFound();
            }

            var model = mapper.Map<ProductCategoryViewModel>(productCategory);
            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProductCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = mapper.Map<ProductCategory>(model);
                await productCategoryService.CreateAsync(entity);
                return RedirectToAction("Index");
            }

            return View(model);
        }

        public async Task<ActionResult> Edit(int id)
        {
            var productCategory = await productCategoryService.GetAsync(id);
            if (productCategory == null)
            {
                return HttpNotFound();
            }

            var model = mapper.Map<ProductCategoryViewModel>(productCategory);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ProductCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = mapper.Map<ProductCategory>(model);
                await productCategoryService.UpdateAsync(entity);
                return RedirectToAction("Index");
            }

            return View(model);
        }

        public async Task<ActionResult> Delete(int id)
        {
            var productCategory = await productCategoryService.GetAsync(id);
            if (productCategory == null)
            {
                return HttpNotFound();
            }

            var model = mapper.Map<ProductCategoryViewModel>(productCategory);
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await productCategoryService.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
