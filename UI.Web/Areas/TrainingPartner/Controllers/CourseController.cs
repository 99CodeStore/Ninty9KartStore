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
        private readonly IMapper mapper;
        private readonly ILogger<CourseController> logger;

        public CourseController(IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<CourseController> logger)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
        }
        public async Task<IActionResult> Index()
        {
            var courses = await unitOfWork.Courses.GetAll(null, null, new List<string>() { "TrainingPartner", "SponsoringBody" ,"JobSector"});
            IEnumerable<CourseDto> result = mapper.Map<IList<CourseDto>>(courses);
            return View(result);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.SponsoringBodyList = new SelectList(
                mapper.Map<IList<SponsoringBody>, IList<SponsoringBodyDto>>(
                    await unitOfWork.SponsoringBodies.GetAll()
                    ), "Id", "Name");

            ViewBag.JobSectorList = new SelectList(
                   await unitOfWork.JobSectors.GetAll()
                     , "Id", "SectorName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] CreateCourseDto createCourseDto)
        {

            if (ModelState.IsValid)
            {
                var newCourse = mapper.Map<Course>(createCourseDto);

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

            var course = await unitOfWork.Courses.Get(x => x.Id == id);

            if (course == null)
            {
                NotFound();
            }

            ViewBag.SponsoringBodyList = new SelectList(
            mapper.Map<IList<SponsoringBody>, IList<SponsoringBodyDto>>(
                await unitOfWork.SponsoringBodies.GetAll()
                ), "Id", "Name");

            ViewBag.JobSectorList = new SelectList(
                  await unitOfWork.JobSectors.GetAll()
                    , "Id", "SectorName");

            var courseDto = mapper.Map<CourseDto>(course);
            return View(courseDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, [FromForm] UpdateCourseDto courseDto)
        {

            if (!ModelState.IsValid || id < 1)
            {
                return NotFound();
            }

            var course = await unitOfWork.Courses.Get(x => x.Id == id.Value);

            if (course == null)
            {
                NotFound();
            }

            if (ModelState.IsValid)
            {

                mapper.Map(courseDto, course);

                unitOfWork.Courses.Update(course);

                await unitOfWork.Save();

                TempData["success"] = $"{courseDto.CourseName} Updated successfully.";

                return RedirectToAction("Index");
            }
            else
            {
                return View(courseDto);
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

            var courseDto = mapper.Map<CourseDto>(course);
            return View(courseDto);
        }

        [HttpPost]
        public async Task<JsonResult> SponseringBodyByCourseList(int? id)
        {
            var result = new SelectList(
                await unitOfWork.Courses.GetAll(c => c.SponsoringBodyId == id.GetValueOrDefault()), "Id", "CourseName");
            return Json(result);
        }
    }
}
