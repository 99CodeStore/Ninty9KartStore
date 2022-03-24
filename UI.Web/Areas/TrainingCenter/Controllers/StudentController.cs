using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using NsdcTraingPartnerHub.Core.Entities;
using NsdcTraingPartnerHub.Core.Interfaces;
using NsdcTraingPartnerHub.Service.Models;
using NsdcTraingPartnerHub.Utility;
using NsdcTraingPartnerHub.Web.Areas.TrainingCenter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NsdcTraingPartnerHub.Web.Areas.TrainingCenter.Controllers
{
    [Area("TrainingCenter")]
    public class StudentController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<StudentController> logger;

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Registration()
        {
            ViewBag.SponsoringBodyList = new SelectList(
                mapper.Map<IList<SponsoringBody>, IList<SponsoringBodyDto>>(
                    await unitOfWork.SponsoringBodies.GetAll()
                    ), "Id", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration([FromForm] StudentRegistrationVM registrationVM)
        {
            if (ModelState.IsValid)
            {
                var claimIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimIdentity.Claims.FirstOrDefault();

                if (claim != null)
                {
                    registrationVM.Student.ApplicationUserId = claim.Value;
                }

                registrationVM.Student.TrainingCenterId = HttpContext.Session.GetInt32(SD.TrainingCenterId).GetValueOrDefault();

                var newStudent = mapper.Map<Student>(registrationVM.Student);

                newStudent.ApplicationUser = null;
                newStudent.Course = null;
                newStudent.SponsoringBody = null;
                newStudent.TrainingCenter = null;
                newStudent.Id = System.Guid.NewGuid();

                await unitOfWork.Students.Insert(newStudent);
                await unitOfWork.Save();

                TempData["success"] = $"{newStudent.FirstName} {newStudent.LastName} Registered successfully.";

                return RedirectToAction("Registration", "Student");
            }
            else
            {
                return View(registrationVM);
            }
        }

        public IActionResult StudentList()
        {
            return View();
        }

        public async Task<IActionResult> StudentListPage(StudentPageFilter filter)
        {
            Expression<Func<Student, bool>> dataFilter = null;

            var orders = await unitOfWork.Students.GetPagedList(mapper.Map<PagedRequest>(filter), dataFilter, null, new List<string> { "ApplicationUser","Course" });

            IList<StudentDto> orderDtos = mapper.Map<IList<StudentDto>>(orders);
            return Json(new
            {
                Data = orderDtos
            });
        }
        public StudentController(IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<StudentController> logger)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
        }
    }
}
