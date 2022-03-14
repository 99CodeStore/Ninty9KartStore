using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using NsdcTraingPartnerHub.Core.Entities;
using NsdcTraingPartnerHub.Core.Interfaces;
using NsdcTraingPartnerHub.Service.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NsdcTraingPartnerHub.Web.Areas.TrainingPartner.Controllers
{
    [Area("TrainingPartner")]
    public class CourseController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper maper;
        private readonly ILogger<CourseController> logger;

        public CourseController(IUnitOfWork unitOfWork,
            IMapper maper,
            ILogger<CourseController> logger)
        {
            this.unitOfWork = unitOfWork;
            this.maper = maper;
            this.logger = logger;
        }
        public async Task<IActionResult> Index()
        {
            var courses = await unitOfWork.Courses.GetAll(null, null, new List<string>() { "TrainingPartner", "SponsoringBody" });
            IEnumerable<CourseDto> result = maper.Map<IList<CourseDto>>(courses);
            return View(result);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.SponsoringBodyList = new SelectList(
                maper.Map<IList<SponsoringBody>, IList<SponsoringBodyDto>>(
                    await unitOfWork.SponsoringBodies.GetAll()
                    ), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] CreateCourseDto createCourseDto)
        {

            if (ModelState.IsValid)
            {
                var newCourse = maper.Map<Course>(createCourseDto);

                await unitOfWork.Courses.Insert(newCourse);
                await unitOfWork.Save();

                TempData["success"] = $"{newCourse.CourseName} Created successfully.";

                return RedirectToAction("Index");
            }
            else
            {
                return View(createCourseDto);
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var course = await unitOfWork.Courses.Get(x => x.Id == id );

            if (course == null)
            {
                NotFound();
            }

            ViewBag.SponsoringBodyList = new SelectList(
            maper.Map<IList<SponsoringBody>, IList<SponsoringBodyDto>>(
                await unitOfWork.SponsoringBodies.GetAll()
                ), "Id", "Name");

            var courseDto = maper.Map<CourseDto>(course);
            return View(courseDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, [FromForm] UpdateCourseDto categoryDto)
        {

            if (!ModelState.IsValid || id < 1)
            {
                return NotFound();
            }

            var category = await unitOfWork.Courses.Get(x => x.Id == id.Value);

            if (category == null)
            {
                NotFound();
            }

            if (ModelState.IsValid)
            {

                maper.Map(categoryDto, category);

                unitOfWork.Courses.Update(category);

                await unitOfWork.Save();

                TempData["success"] = $"{categoryDto.CourseName} Updated successfully.";

                return RedirectToAction("Index");
            }
            else
            {
                return View(categoryDto);
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

            await unitOfWork.Courses.Delete(id.Value);
            await unitOfWork.Save();

            TempData["success"] = $"Category Deleted successfully.";

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var course = await unitOfWork.Courses.Get(x => x.Id == id,
                new List<string>() { "TrainingPartner", "SponsoringBody" });

            if (course == null)
            {
                NotFound();
            }

            var courseDto = maper.Map<CourseDto>(course);
            return View(courseDto);
        }

    }
}
