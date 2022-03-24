using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using NsdcTraingPartnerHub.Core.Entities;
using NsdcTraingPartnerHub.Core.Interfaces;
using NsdcTraingPartnerHub.Service.Models;
using NsdcTraingPartnerHub.Utility;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NsdcTraingPartnerHub.Web.Areas.TrainingPartner.Controllers
{
    [Area("TrainingPartner")]
    public class TrainingCenterCourseController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper maper;
        private readonly ILogger<TrainingCenterCourseController> logger;

        public TrainingCenterCourseController(IUnitOfWork unitOfWork,
            IMapper maper,
            ILogger<TrainingCenterCourseController> logger)
        {
            this.unitOfWork = unitOfWork;
            this.maper = maper;
            this.logger = logger;
        }
        public async Task<IActionResult> Index(int? id)
        {
            var centerCourses = await unitOfWork.TrainingCenterCourses.GetAll(
                c => c.TrainingCenterId == id.GetValueOrDefault(),
                null, new List<string>() { "Course" });
            IEnumerable<TrainingCenterCourseDto> result = maper.Map<IList<TrainingCenterCourseDto>>(centerCourses);
            TempData["centerId"] = id.GetValueOrDefault();
            return View(result);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.CourseList = new SelectList(await unitOfWork.Courses.GetAll(), "Id", "Name");

            ViewBag.SponsiringBdyList = new SelectList(await unitOfWork.SponsoringBodies.GetAll(), "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] CreateTrainingCenterCourseDto createDto)
        {
            if (ModelState.IsValid)
            {
                createDto.TrainingCenter = null;
                createDto.Course = null;

                if (TempData["CenterId"] != null)
                {
                    createDto.TrainingCenterId = Convert.ToInt32(TempData["CenterId"]);
                }
                else
                {
                    ModelState.AddModelError("TrainingCenter", "Invalid Training Center.");
                    return View(createDto);
                }

                var trainingCenterCourse = maper.Map<TrainingCenterCourse>(createDto);

                await unitOfWork.TrainingCenterCourses.Insert(trainingCenterCourse);

                await unitOfWork.Save();

                TempData["success"] = $"Course successfully added.";

                return RedirectToAction("Index", "CenterAuthority",
                    new { Id = createDto.TrainingCenterId });
            }
            else
            {
                return View(createDto);
            }
        }

        [HttpPost]
        public async Task<JsonResult> SponseringBodyByCourseList(int? id)
        {
            var result = new SelectList(
                await unitOfWork.TrainingCenterCourses.GetAll(
                   (c) => c.Course.SponsoringBodyId == id.GetValueOrDefault() && 
                   c.TrainingCenterId == HttpContext.Session.GetInt32(SD.TrainingCenterId),
                   null,
                   new List<string> { "Course" }
                ), "Course.Id", "Course.CourseName");
            return Json(result);
        }
    }
}
