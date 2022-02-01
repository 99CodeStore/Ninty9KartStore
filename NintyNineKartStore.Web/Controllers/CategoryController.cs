using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NintyNineKartStore.Core.Entities;
using NintyNineKartStore.Core.Interfaces;
using NintyNineKartStore.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NintyNineKartStore.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper maper;

        public CategoryController(IUnitOfWork unitOfWork, IMapper maper)
        {
            this.unitOfWork = unitOfWork;
            this.maper = maper;
        }
        public async Task<IActionResult> Index()
        {
            var categories = await unitOfWork.Categories.GetAll();
            IEnumerable<CategoryDto> result = maper.Map<IList<CategoryDto>>(categories);
            return View(result);
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([FromForm] CreateCategoryDto newCategoryDto)
        {
            if (ModelState.IsValid)
            {
                var newCategory = maper.Map<Category>(newCategoryDto);

                unitOfWork.Categories.Insert(newCategory).Wait();
                unitOfWork.Save().Wait();

                return RedirectToAction("Index"); 
            }
            else
            {
                return View(newCategoryDto);
            }
        }

    }
}
