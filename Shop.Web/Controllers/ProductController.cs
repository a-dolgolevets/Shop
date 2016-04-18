using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Shop.Common.Facade;
using Shop.Domain.Entities.Shop;
using Shop.ServicesFacade.Base;
using Shop.ViewModel.Shop;

namespace Shop.Web.Controllers
{
    public class ProductController : Controller
    {
        private static readonly string PhotosBaseUrl = ConfigurationManager.AppSettings["PhotosBaseUrl"];

        private readonly IService<Product> productService;
        private readonly IService<ProductCategory> productCategoryService;
        private readonly IMapper mapper;

        public ProductController(IService<Product> productService, IService<ProductCategory> productCategoryService, IMapper mapper)
        {
            this.productService = productService;
            this.productCategoryService = productCategoryService;
            this.mapper = mapper;
        }

        public async Task<ActionResult> Index()
        {
            var entities = await productService.GetAllAsync();
            var model = mapper.Map<IList<ProductViewModel>>(entities);
            return View(model);
        }

        public async Task<ActionResult> Details(int id)
        {
            var product = await productService.GetAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            var model = mapper.Map<ProductViewModel>(product);
            return View(model);
        }

        [Authorize]
        public async Task<ActionResult> Create()
        {
            ViewBag.CategoryId = new SelectList(await productCategoryService.GetAllAsync(), "Id", "Title");
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProductViewModel model, HttpPostedFileBase photo)
        {
            if (ModelState.IsValid)
            {
                model.PhotoUrl = GetPhotoUrl(photo);

                var entity = mapper.Map<Product>(model);
                await productService.CreateAsync(entity);
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(await productCategoryService.GetAllAsync(), "Id", "Title", model.CategoryId);
            return View(model);
        }

        [Authorize]
        public async Task<ActionResult> Edit(int id)
        {
            var product = await productService.GetAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            ViewBag.CategoryId = new SelectList(await productCategoryService.GetAllAsync(), "Id", "Title", product.CategoryId);
            var model = mapper.Map<ProductViewModel>(product);
            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ProductViewModel model, HttpPostedFileBase photo)
        {
            if (ModelState.IsValid)
            {
                model.PhotoUrl = GetPhotoUrl(photo);

                var entity = mapper.Map<Product>(model);
                await productService.UpdateAsync(entity);
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(await productCategoryService.GetAllAsync(), "Id", "Title", model.CategoryId);
            return View(model);
        }

        [Authorize]
        public async Task<ActionResult> Delete(int id)
        {
            var product = await productService.GetAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            var model = mapper.Map<ProductViewModel>(product);
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await productService.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        #region Private methods
        private string GetPhotoUrl(HttpPostedFileBase photo)
        {
            var photoUrl = string.Empty;
            if (photo != null)
            {
                var photosDirectory = Server.MapPath(PhotosBaseUrl);
                if (!Directory.Exists(photosDirectory))
                {
                    Directory.CreateDirectory(photosDirectory);
                }

                var extension = Path.GetExtension(photo.FileName);
                photoUrl = string.Format("{0}/{1}{2}", PhotosBaseUrl, Guid.NewGuid(), extension);
                var filePath = Server.MapPath(photoUrl);
                photo.SaveAs(filePath);
            }

            return photoUrl;
        }
        #endregion
    }
}
