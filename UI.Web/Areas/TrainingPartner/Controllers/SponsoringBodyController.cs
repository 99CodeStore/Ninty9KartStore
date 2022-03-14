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
    public class SponsoringBodyController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper maper;
        private readonly ILogger<SponsoringBodyController> logger;

        public SponsoringBodyController(IUnitOfWork unitOfWork,
            IMapper maper,
            ILogger<SponsoringBodyController> logger)
        {
            this.unitOfWork = unitOfWork;
            this.maper = maper;
            this.logger = logger;
        }
        public async Task<IActionResult> Index()
        {
            var sponsoringBodies = await unitOfWork.SponsoringBodies.GetAll();
            IEnumerable<SponsoringBodyDto> result = maper.Map<IList<SponsoringBodyDto>>(sponsoringBodies);
            return View(result);
        }
    }
}
