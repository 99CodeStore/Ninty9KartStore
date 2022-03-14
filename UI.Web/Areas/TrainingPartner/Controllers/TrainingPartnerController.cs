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
    public class TrainingPartnerController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper maper;
        private readonly ILogger<TrainingPartnerController> logger;

        public TrainingPartnerController(IUnitOfWork unitOfWork,
            IMapper maper,
            ILogger<TrainingPartnerController> logger)
        {
            this.unitOfWork = unitOfWork;
            this.maper = maper;
            this.logger = logger;
        }
        public async Task<IActionResult> Index()
        {
            var trainingPartners = await unitOfWork.TrainingPartners.GetAll();
            IEnumerable<TrainingPartnerDto> result = maper.Map<IList<TrainingPartnerDto>>(trainingPartners);
            return View(result);
        }
    }
}
