using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NsdcTraingPartnerHub.Core.Entities;
using NsdcTraingPartnerHub.Core.Interfaces;
using NsdcTraingPartnerHub.Service.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NsdcTraingPartnerHub.Web.Areas.TrainingPartner.Controllers
{
    [Area("TrainingPartner")]
    public class JobSectorController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<JobSectorController> logger;

        public async Task<IActionResult> Index()
        {
            var jobSectors = await unitOfWork.JobSectors.GetAll();
            IEnumerable<JobSectorDto> result = mapper.Map<IList<JobSectorDto>>(jobSectors);
            return View(result);

        }
        public async Task<IActionResult> Create(int? id)
        {
            if (id.HasValue)
            {
                var jobSector = await unitOfWork.JobSectors.Get(x => x.Id == id);
                if (jobSector == null)
                {
                    TempData["error"] = $"Job Sector not found.";
                    return View();
                }
                return View(mapper.Map<JobSectorDto>(jobSector));
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int? id, [FromForm] JobSectorDto sectorDto)
        {
            if (ModelState.IsValid)
            {
                if (id.GetValueOrDefault() > 0)
                {
                    var jobSector = await unitOfWork.JobSectors.Get(x => x.Id == id.Value);

                    if (jobSector == null)
                    {
                        TempData["error"] = $"Record not found to update.";
                        return View(sectorDto);
                    }

                    mapper.Map(sectorDto, jobSector);

                    unitOfWork.JobSectors.Update(jobSector);

                    await unitOfWork.Save();

                    TempData["success"] = $"{sectorDto.SectorName} Updated successfully.";
                }
                else
                {
                    var newSector = mapper.Map<JobSector>(sectorDto);

                    await unitOfWork.JobSectors.Insert(newSector);
                    await unitOfWork.Save();

                    TempData["success"] = $"{newSector.SectorName} Created successfully.";
                }

                return RedirectToAction("Index");
            }
            else
            {
                return View(sectorDto);
            }
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {

            var jobSector  = await unitOfWork.JobSectors.Get(x => x.Id == id);
            if (jobSector == null)
            {
                return Json(new { success = false, message = "Error While Deleting." });
            }

            await unitOfWork.JobSectors.Delete(id.Value);
            await unitOfWork.Save();

            return Json(new { success = true, message = $"Job Section <b>{jobSector.SectorName}</b> Deleted successfully." });

        }

        public JobSectorController(IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<JobSectorController> logger)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
        }
    }
}
