using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using NintyNineKartStore.Core.Entities;
using NintyNineKartStore.Core.Interfaces;
using NintyNineKartStore.Service.Models;
using NintyNineKartStore.Utility;
using NintyNineKartStore.Web.Areas.Admin.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace NintyNineKartStore.Web.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_User_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper maper;
        private readonly ILogger<CompanyController> logger;


        public CompanyController(IUnitOfWork unitOfWork,
            IMapper maper,
            ILogger<CompanyController> logger
            )
        {
            this.unitOfWork = unitOfWork;
            this.maper = maper;
            this.logger = logger;

        }
        public async Task<IActionResult> Index()
        {
            var companies = await unitOfWork.Companies.GetAll();
            IList<CompanyDto> result = maper.Map<IList<CompanyDto>>(companies);
            return View(result);
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            CompanyDto companyDto = new();

            if (id == null || id == 0)
            {
                //insert 
            }
            else
            {
                //Update
                var company = await unitOfWork.Companies.Get(x => x.Id == id);
                if (company == null)
                {
                    NotFound();
                }

                companyDto = maper.Map<CompanyDto>(company);

            }
            return View(companyDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(int? id, [FromForm] UpdateCompanyDto companyDto)
        {

            if (ModelState.IsValid)
            {
                Company company;

                if (id.HasValue && id > 0)
                {
                    company = await unitOfWork.Companies.Get(x => x.Id == id.GetValueOrDefault());

                    if (company == null)
                    {
                        NotFound();
                    }

                    maper.Map(companyDto, company);

                    unitOfWork.Companies.Update(company);

                    await unitOfWork.Save();

                    TempData["success"] = $"{company.Name} Updated successfully.";

                }
                else
                {

                    company = maper.Map<Company>(companyDto);

                    await unitOfWork.Companies.Insert(company);

                    await unitOfWork.Save();

                    TempData["success"] = $"{company.Name} Created successfully.";

                }

                return RedirectToAction("Index");
            }
            else
            {
                return View(companyDto);
            }
        }


        #region API Calls
        [HttpGet]
        public async Task<IActionResult> GetPaggedList(PagedRequestInput pagedRequestInput)
        {
            var companies = await unitOfWork.Companies.GetPagedList(maper.Map<PagedRequest>(pagedRequestInput));
            IList<CompanyDto> result = maper.Map<IList<CompanyDto>>(companies);
            return Json(new { Data = result });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {

            var company = await unitOfWork.Companies.Get(x => x.Id == id);
            if (company == null)
            {
                return Json(new { success = false, message = "Error While Deleting." });
            }

            await unitOfWork.Companies.Delete(id.Value);
            await unitOfWork.Save();

            return Json(new { success = true, message = $"{company.Name} Company Deleted successfully." });

        }

        #endregion
    }
}
