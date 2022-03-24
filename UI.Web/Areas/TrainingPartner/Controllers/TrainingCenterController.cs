using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using NsdcTraingPartnerHub.Core.Entities;
using NsdcTraingPartnerHub.Core.Interfaces;
using NsdcTraingPartnerHub.Service.Models;
using NsdcTraingPartnerHub.Utility;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NsdcTraingPartnerHub.Web.Areas.TrainingPartner.Controllers
{
    [Area("TrainingPartner")]
    public class TrainingCenterController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper maper;
        private readonly ILogger<TrainingCenterController> logger;

        public TrainingCenterController(IUnitOfWork unitOfWork,
            IMapper maper,
            ILogger<TrainingCenterController> logger)
        {
            this.unitOfWork = unitOfWork;
            this.maper = maper;
            this.logger = logger;
        }
        public async Task<IActionResult> Index()
        {
            var trainingCenters = await unitOfWork.TrainingCenters.GetAll(null, null, new List<string>() { "TrainingPartner" });
            IEnumerable<TrainingCenterDto> result = maper.Map<IList<TrainingCenterDto>>(trainingCenters);

            return View(result);
        }

        public async Task<IActionResult> Register()
        {
            var tpList = maper.Map<IList<Core.Entities.TrainingPartner>, IList<TrainingPartnerDto>>(
                     await unitOfWork.TrainingPartners.GetAll()
                     );

            TrainingCenterDto trainingCenterDto = new();

            if (tpList.Count > 1)
            {
                ViewBag.TrainingPartnerList = new SelectList(tpList
               , "Id", "PartnerName");

                trainingCenterDto.TrainingPartner = null;

            }
            else if (tpList.Count==1)
            {
                trainingCenterDto.TrainingPartner = tpList[0];
                trainingCenterDto.TrainingPartnerId = tpList[0].Id;
            }

            return View(trainingCenterDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([FromForm] CreateTrainingCenterDto centerDto)
        {

            if (ModelState.IsValid)
            {
                centerDto.Status = SD.TrainingCenetrStatus.ActiveCenter;
                centerDto.TrainingPartner = null;
                var trainingCenter = maper.Map<Core.Entities.TrainingCenter>(centerDto);

                await unitOfWork.TrainingCenters.Insert(trainingCenter);
                await unitOfWork.Save();

                TempData["success"] = $"{trainingCenter.CenterName} Created successfully.";

                return RedirectToAction("Index", "CenterAuthority", new { Id = trainingCenter.Id });
            }
            else
            {
                return View(centerDto);
            }
        }

    }
}
