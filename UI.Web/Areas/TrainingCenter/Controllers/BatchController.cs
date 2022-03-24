using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using NsdcTraingPartnerHub.Core.Entities;
using NsdcTraingPartnerHub.Core.Interfaces;
using NsdcTraingPartnerHub.Service.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NsdcTraingPartnerHub.Web.Areas.TrainingCenter.Controllers
{

    [Area("TrainingCenter")]
    public class BatchController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<BatchController> logger;

        public async Task<IActionResult> Index(int? courseId = 0)
        {
            var batches = await unitOfWork.CourseBatches.GetAll(
            b => b.CourseId == courseId.GetValueOrDefault() || courseId.GetValueOrDefault() == 0
            , null, new List<string>() { "Course", "ApplicationUser" });
            IEnumerable<CourseBatchDto> result = mapper.Map<IList<CourseBatchDto>>(batches);

            ViewBag.CourseId = courseId.GetValueOrDefault();

            return View(result);

        }

        public async Task<IActionResult> Create(int? Id, int? courseId = 0)
        {
            ViewBag.CourseList = new SelectList(
                   await unitOfWork.Courses.GetAll()
                     , "Id", "CourseName");
            if (Id.HasValue)
            {
                var batch = mapper.Map<CourseBatchDto>(
                 await unitOfWork.CourseBatches.Get(
                        b => b.Id == Id.GetValueOrDefault(),new List<string> { "Course"}
                        ));
                return View(batch);
            }
            else
            {
                return View(new CourseBatchDto() { CourseId = courseId.GetValueOrDefault() });
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int? Id, [FromForm] CourseBatchDto batchDto)
        {
            if (ModelState.IsValid)
            {

                batchDto.ApplicationUser = null;
                batchDto.Course = null;

                if (batchDto.Id > 0)
                {

                    var batch = await unitOfWork.CourseBatches.Get(x => x.Id == batchDto.Id);

                    if (batch == null)
                    {
                        TempData["error"] = $"Record not found to update.";
                        return View(batchDto);
                    }
                    batchDto.ApplicationUserId = batch.ApplicationUserId;

                    mapper.Map(batchDto, batch);

                    unitOfWork.CourseBatches.Update(batch);

                    await unitOfWork.Save();

                    TempData["success"] = $"{batchDto.BatchName} Updated successfully.";
                }
                else
                {

                    var claimIdentity = (ClaimsIdentity)User.Identity;
                    var claim = claimIdentity.Claims.FirstOrDefault();

                    if (claim != null)
                    {
                        batchDto.ApplicationUserId = claim.Value;
                    }

                    var newBatch = mapper.Map<CourseBatch>(batchDto);

                    await unitOfWork.CourseBatches.Insert(newBatch);
                    await unitOfWork.Save();

                    TempData["success"] = $"{newBatch.BatchName} Created successfully.";

                }
                return RedirectToAction("Index");
            }
            else
            {
                return View(batchDto);
            }
        }

        [HttpDelete()]
         public async Task<IActionResult> Delete(int? id)
        {

            var batch = await unitOfWork.CourseBatches.Get(x => x.Id == id);
            if (batch == null)
            {
                return Json(new { success = false, message = "Error While Deleting." });
            }

            await unitOfWork.CourseBatches.Delete(id.Value);
            await unitOfWork.Save();

            return Json(new { success = true, message = $"Batch <b>{batch.BatchName}</b> Deleted successfully." });

        }


        public BatchController(IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<BatchController> logger)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
        }
    }
}
