using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using NsdcTraingPartnerHub.Core.Entities;
using NsdcTraingPartnerHub.Core.Interfaces;
using NsdcTraingPartnerHub.Service.Models;
using NsdcTraingPartnerHub.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NsdcTraingPartnerHub.Web.Areas.TrainingCenter.Controllers
{
    [Area("TrainingCenter")]
    public class TrainingCenterDashboardController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper maper;
        private readonly ILogger<TrainingCenterDashboardController> logger;

        public TrainingCenterDashboardController(IUnitOfWork unitOfWork,
            IMapper maper,
            ILogger<TrainingCenterDashboardController> logger)
        {
            this.unitOfWork = unitOfWork;
            this.maper = maper;
            this.logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }
       
        public async Task<IActionResult> CourseList(int? centerId)
        {

            var courses = await unitOfWork.TrainingCenterCourses.GetAll(
                tc => tc.TrainingCenterId == HttpContext.Session.GetInt32(SD.TrainingCenterId)
                , null, new List<string>() { "TrainingCenter", "Course" });
            IEnumerable<TrainingCenterCourseDto> result = maper.Map<IList<TrainingCenterCourseDto>>(courses);
            return View(result);

        }


    }
}
