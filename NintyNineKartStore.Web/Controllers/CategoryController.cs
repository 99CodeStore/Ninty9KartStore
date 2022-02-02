using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NintyNineKartStore.Core.Entities;
using NintyNineKartStore.Core.Interfaces;
using NintyNineKartStore.Service.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NintyNineKartStore.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper maper;


        public CategoryController(IUnitOfWork unitOfWork,
            IMapper maper
            )
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
        public async Task<IActionResult> Create([FromForm] CreateCategoryDto newCategoryDto)
        {
            if (newCategoryDto.Name == newCategoryDto.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The DisplayOrder can not be exactly match the Name.");
            }

            if (ModelState.IsValid)
            {
                var newCategory = maper.Map<Category>(newCategoryDto);

                await unitOfWork.Categories.Insert(newCategory);
                await unitOfWork.Save();

                return RedirectToAction("Index");
            }
            else
            {
                return View(newCategoryDto);
            }
        }

        public async Task<IActionResult> Edit(uint? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var category = await unitOfWork.Categories.Get(x => x.Id == id);

            if (category == null)
            {
                NotFound();
            }

            var categoryDto = maper.Map<CategoryDto>(category);
            return View(categoryDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(uint? id, [FromForm] UpdateCategoryDto categoryDto)
        {

            if (!ModelState.IsValid || id < 1)
            {
                return NotFound();
            }

            var category = await unitOfWork.Categories.Get(x => x.Id == id.Value);

            if (category == null)
            {
                NotFound();
            }

            if (categoryDto.Name == categoryDto.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The DisplayOrder can not be exactly match the Name.");
            }

            if (ModelState.IsValid)
            {

                maper.Map(categoryDto, category);

                unitOfWork.Categories.Update(category);

                await unitOfWork.Save();

                return RedirectToAction("Index");
            }
            else
            {
                return View(categoryDto);
            }
        }
       
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(uint? id)
        {
            if ( !ModelState.IsValid || id < 1)
            {
                return NotFound();
            }

            await unitOfWork.Categories.Delete(id.Value);
            await unitOfWork.Save();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(uint? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var category = await unitOfWork.Categories.Get(x => x.Id == id);

            if (category == null)
            {
                NotFound();
            }

            var categoryDto = maper.Map<CategoryDto>(category);
            return View(categoryDto);
        }
    }
}
