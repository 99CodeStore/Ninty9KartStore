using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using NintyNineKartStore.Core.Entities;
using NintyNineKartStore.Core.Interfaces;
using NintyNineKartStore.Service.Models;
using NintyNineKartStore.Web.Areas.Admin.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NintyNineKartStore.Web.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper maper;
        private readonly ILogger<ProductController> logger;

        public ProductController(IUnitOfWork unitOfWork,
            IMapper maper,
            ILogger<ProductController> logger
            )
        {
            this.unitOfWork = unitOfWork;
            this.maper = maper;
            this.logger = logger;
        }
        public async Task<IActionResult> Index()
        {
            var categories = await unitOfWork.Products.GetAll();
            IList<ProductDto> result = maper.Map<IList<ProductDto>>(categories);
            return View(result);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] CreateProductDto newProductDto)
        {
            //if (newProductDto.Name == newProductDto.DisplayOrder.ToString())
            //{
            //    ModelState.AddModelError("name", "The DisplayOrder can not be exactly match the Name.");
            //}

            if (ModelState.IsValid)
            {
                var newProduct = maper.Map<Product>(newProductDto);

                await unitOfWork.Products.Insert(newProduct);
                await unitOfWork.Save();

                TempData["success"] = $"{newProduct.Title} Created successfully.";

                return RedirectToAction("Index");
            }
            else
            {
                return View(newProductDto);
            }
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
        public async Task<IActionResult> Upsert(int? id, [FromForm] UpdateProductDto productDto, IFormFile file)
        {

            if (!ModelState.IsValid || id < 1)
            {
                return NotFound();
            }

            var product = await unitOfWork.Products.Get(x => x.Id == id.Value);

            if (product == null)
            {
                NotFound();
            }

            //if (productDto.Title == productDto.DisplayOrder.ToString())
            //{
            //    ModelState.AddModelError("name", "The DisplayOrder can not be exactly match the Name.");
            //}

            if (ModelState.IsValid)
            {

                maper.Map(productDto, product);

                unitOfWork.Products.Update(product);

                await unitOfWork.Save();

                TempData["success"] = $"{productDto.Title} Updated successfully.";

                return RedirectToAction("Index");
            }
            else
            {
                return View(productDto);
            }
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int? id)
        {
            if (!ModelState.IsValid || id < 1)
            {
                return NotFound();
            }

            await unitOfWork.Products.Delete(id.Value);
            await unitOfWork.Save();

            TempData["success"] = $"Product Deleted successfully.";

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var product = await unitOfWork.Products.Get(x => x.Id == id);

            if (product == null)
            {
                NotFound();
            }

            var productDto = maper.Map<ProductDto>(product);
            return View(productDto);
        }
    }
}
