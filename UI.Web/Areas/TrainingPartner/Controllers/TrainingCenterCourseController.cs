using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NsdcTraingPartnerHub.Core.Interfaces;
using NsdcTraingPartnerHub.Service.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NsdcTraingPartnerHub.Web.Areas.TrainingPartner.Controllers
{
    [Area("TrainingPartner")]
    public class TrainingCenterCourseController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper maper;
        private readonly ILogger<CenterAuthorityController> logger;

        public TrainingCenterCourseController(IUnitOfWork unitOfWork,
            IMapper maper,
            ILogger<CenterAuthorityController> logger)
        {
            this.unitOfWork = unitOfWork;
            this.maper = maper;
            this.logger = logger;
        }
        public async Task<IActionResult> Index(int? Id)
        {
            var centerCourses = await unitOfWork.CenterAuthorityMembers.GetAll(null, null, new List<string>() { "TrainingCenter" });
            IEnumerable<TrainingCenterCourseDto> result = maper.Map<IList<TrainingCenterCourseDto>>(centerCourses);

            return View(result);
        }
    }
}
