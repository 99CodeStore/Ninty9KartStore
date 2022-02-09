using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using NintyNineKartStore.Core.Entities;
using NintyNineKartStore.Core.Interfaces;
using NintyNineKartStore.Service.Models;
using NintyNineKartStore.Web.Areas.Admin.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace NintyNineKartStore.Web.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper maper;
        private readonly ILogger<ProductController> logger;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork,
            IMapper maper,
            ILogger<ProductController> logger,
            IWebHostEnvironment webHostEnvironment
            )
        {
            this.unitOfWork = unitOfWork;
            this.maper = maper;
            this.logger = logger;
            this.webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            var categories = await unitOfWork.Products.GetAll();
            IList<ProductDto> result = maper.Map<IList<ProductDto>>(categories);
            return View(result);
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            var cats = await unitOfWork.Categories.GetAll();

            var coverTypes = await unitOfWork.CoverTypes.GetAll();

            ProductViewModel productViewModel = new()
            {
                Product = new(),
                CategoryList = new SelectList(maper.Map<IList<Category>, IList<CategoryDto>>(cats), "Id", "Name"),
                CoverTypeList = new SelectList(maper.Map<IList<CoverType>, IList<CoverTypeDto>>(coverTypes), "Id", "Name")
            };

            if (id == null || id == 0)
            {
                //insert 
            }
            else
            {
                //Update
                var product = await unitOfWork.Products.Get(x => x.Id == id);
                if (product == null)
                {
                    NotFound();
                }

                productViewModel.Product = maper.Map<ProductDto>(product);

            }
            return View(productViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(int? id, [FromForm] ProductViewModel productViewModel, IFormFile? file)
        {

            if (ModelState.IsValid)
            {
                var wwwRootPath = webHostEnvironment.WebRootPath;

                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();

                    var uploads = Path.Combine(wwwRootPath, @"images\products\");
                    var extension = Path.GetExtension(file.FileName);

                    // Deleting Existing File.
                    if (!string.IsNullOrEmpty(productViewModel.Product.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, productViewModel.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productViewModel.Product.ImageUrl = @"\images\products\" + fileName + extension;
                }

                Product product;

                if (id.HasValue && id > 0)
                {
                    product = await unitOfWork.Products.Get(x => x.Id == id.GetValueOrDefault());

                    if (product == null)
                    {
                        NotFound();
                    }

                    maper.Map(productViewModel.Product, product);

                    unitOfWork.Products.Update(product);

                    await unitOfWork.Save();

                    TempData["success"] = $"{productViewModel.Product.Title} Updated successfully.";

                }
                else
                {

                    product = maper.Map<Product>(productViewModel.Product);

                    await unitOfWork.Products.Insert(product);

                    await unitOfWork.Save();

                    TempData["success"] = $"{productViewModel.Product.Title} Created successfully.";

                }

                return RedirectToAction("Index");
            }
            else
            {
                return View(productViewModel);
            }
        }


        #region API Calls
        [HttpGet]
        public async Task<IActionResult> GetPaggedList(PagedRequestInput pagedRequestInput)
        {
            var products = await unitOfWork.Products.GetPagedList(maper.Map<PagedRequest>(pagedRequestInput), null, null, new List<string> { "Category" });
            IList<ProductDto> result = maper.Map<IList<ProductDto>>(products);
            return Json(new { Data = result });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {

            var product = await unitOfWork.Products.Get(x => x.Id == id);
            if (product == null)
            {
                return Json(new { success = false, message = "Error While Deleting." });
            }


            // Deleting Existing File.
            if (!string.IsNullOrEmpty(product.ImageUrl))
            {

                var oldImagePath = Path.Combine(webHostEnvironment.WebRootPath, product.ImageUrl.TrimStart('\\'));

                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            await unitOfWork.Products.Delete(id.Value);
            await unitOfWork.Save();

            return Json(new { success = true, message = $"{product.Title} Product Deleted successfully." });

        }

        #endregion
    }
}
