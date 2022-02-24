using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NintyNineKartStore.Core.Entities;
using NintyNineKartStore.Core.Interfaces;
using NintyNineKartStore.Service.Models;
using NintyNineKartStore.Utility;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NintyNineKartStore.Web.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_User_Admin)]
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper maper;
        private readonly ILogger<CoverTypeController> logger;

        public CoverTypeController(IUnitOfWork unitOfWork,
            IMapper maper,
            ILogger<CoverTypeController> logger)
        {
            this.unitOfWork = unitOfWork;
            this.maper = maper;
            this.logger = logger;
        }
        public async Task<IActionResult> Index()
        {
            var coverTypes = await unitOfWork.CoverTypes.GetAll();
            IEnumerable<CoverTypeDto> result = maper.Map<IList<CoverTypeDto>>(coverTypes);
            return View(result);
        }

        public IActionResult Create()
        {
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task< IActionResult> Create([FromForm] CreateCoverTypeDto obj)
        {
            if (ModelState.IsValid)
            {
                var newCoverType = maper.Map<CoverType>(obj);

                await unitOfWork.CoverTypes.Insert(newCoverType);
                await unitOfWork.Save();

                TempData["success"] = "CoverType created successfully";
                return RedirectToAction("Index");
            }

            return View(obj);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var coverType = await unitOfWork.CoverTypes.Get(x => x.Id == id);

            if (coverType == null)
            {
                NotFound();
            }

            var coverTypeDto = maper.Map<CoverTypeDto>(coverType);
            return View(coverTypeDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, [FromForm] UpdateCoverTypeDto updateCoverTypeDto)
        {

            if (!ModelState.IsValid || id < 1)
            {
                return NotFound();
            }

            var coverType = await unitOfWork.CoverTypes.Get(x => x.Id == id.Value);

            if (coverType == null)
            {
                NotFound();
            }

 
            if (ModelState.IsValid)
            {

                maper.Map(updateCoverTypeDto, coverType);

                unitOfWork.CoverTypes.Update(coverType);

                await unitOfWork.Save();

                TempData["success"] = $"{updateCoverTypeDto.Name} Updated successfully.";

                return RedirectToAction("Index");
            }
            else
            {
                return View(updateCoverTypeDto);
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

            await unitOfWork.CoverTypes.Delete(id.Value);
            await unitOfWork.Save();

            TempData["success"] = $"CoverType Deleted successfully.";

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var coverType = await unitOfWork.CoverTypes.Get(x => x.Id == id);

            if (coverType == null)
            {
                NotFound();
            }

            var coverTypeDto = maper.Map<CoverTypeDto>(coverType);
            return View(coverTypeDto);
        }

    }
}
