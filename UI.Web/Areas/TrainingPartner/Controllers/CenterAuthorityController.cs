using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using NsdcTraingPartnerHub.Core.Entities;
using NsdcTraingPartnerHub.Core.Interfaces;
using NsdcTraingPartnerHub.Service.Models;
using NsdcTraingPartnerHub.Web.Areas.TrainingPartner.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NsdcTraingPartnerHub.Web.Areas.TrainingPartner.Controllers
{
    [Area("TrainingPartner")]
    public class CenterAuthorityController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper maper;
        private readonly ILogger<CenterAuthorityController> logger;
        private readonly UserManager<IdentityUser> userManager;

        public CenterAuthorityController(IUnitOfWork unitOfWork,
            IMapper maper,
            ILogger<CenterAuthorityController> logger,
            UserManager<IdentityUser> _userManager
            )
        {
            this.unitOfWork = unitOfWork;
            this.maper = maper;
            this.logger = logger;
            userManager = _userManager;
        }
        public async Task<IActionResult> Index(int? Id)
        {
            var centerAuthorities = await unitOfWork.CenterAuthorityMembers.GetAll(
                x => x.TrainingCenterId == Id.GetValueOrDefault(),
                null, new List<string>() { "TrainingCenter" });
            IEnumerable<CenterAuthorityMemberDto> result = maper.Map<IList<CenterAuthorityMemberDto>>(centerAuthorities);
            TempData["CenterId"] = Id;

            return View(result);

        }

        public async Task<IActionResult> Create()
        {
            //ViewBag.SponsoringBodyList = new SelectList(
            //    maper.Map<IList<SponsoringBody>, IList<SponsoringBodyDto>>(
            //        await unitOfWork.SponsoringBodies.GetAll()
            //        ), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] CenterAuthorityVM createDto)
        {
            if (ModelState.IsValid)
            {
                createDto.CenterAuthorityMember.TrainingCenter = null;

                if (TempData["CenterId"] != null)
                {
                    createDto.CenterAuthorityMember.TrainingCenterId = Convert.ToInt32(TempData["CenterId"]);
                }
                else
                {
                    ModelState.AddModelError("TrainingCenter", "Invalid Training Center.");
                }

                if (createDto.IsCreateLogin)
                {
                    if (string.IsNullOrEmpty(createDto.Password))
                    {
                        TempData["error"] = $"Password required when Creating login.";
                    }
                }

                var authorityMember = maper.Map<CenterAuthorityMember>(createDto.CenterAuthorityMember);

                await unitOfWork.CenterAuthorityMembers.Insert(authorityMember);

                await unitOfWork.Save();

                if (createDto.IsCreateLogin)
                {

                }

                TempData["success"] = $"{authorityMember.Name} successfully added as a member .";

                return RedirectToAction("Index");
            }
            else
            {
                return View(createDto);
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var centerAuthorityDto = maper.Map<CenterAuthorityMemberDto>(await unitOfWork.CenterAuthorityMembers.Get(x => x.Id == id));

            if (centerAuthorityDto == null)
            {
                NotFound();
            }
            var appuser = await unitOfWork.ApplicationUsers.Get(x => x.Email == centerAuthorityDto.Email);

            CenterAuthorityVM CenterAuthorityModel = new()
            {
                CenterAuthorityMember = centerAuthorityDto,
                IsCreateLogin = appuser != null,
            };

            return View(CenterAuthorityModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, [FromForm] CenterAuthorityVM updateDto)
        {

            if (!ModelState.IsValid || id < 1)
            {
                return NotFound();
            }

            var member = await unitOfWork.CenterAuthorityMembers.Get(x => x.Id == id.Value);

            if (member == null)
            {
                NotFound();
            }

            if (ModelState.IsValid)
            {

                if (TempData["CenterId"] != null)
                {
                    updateDto.CenterAuthorityMember.TrainingCenterId = Convert.ToInt32(TempData["CenterId"]);
                }
                else
                {
                    ModelState.AddModelError("TrainingCenter", "Invalid Training Center.");
                }

                if (updateDto.IsCreateLogin)
                {
                    if (string.IsNullOrEmpty(updateDto.Password))
                    {
                        TempData["error"] = $"Password required when Creating login.";
                    }
                }

                maper.Map(updateDto, member);

                unitOfWork.CenterAuthorityMembers.Update(member);

                var user = await userManager.FindByEmailAsync(member.Email);

                if (user != null)
                {
                    if (!updateDto.IsCreateLogin)
                    {
                        await userManager.DeleteAsync(user);
                        //unitOfWork.ApplicationUsers.Delete(); deleting user
                    }
                    else if (member.Email != updateDto.CenterAuthorityMember.Email)
                    {
                        #region Updating Email for User Login
                        var email = await userManager.GetEmailAsync(user);

                        var userId = await userManager.GetUserIdAsync(user);
                        var code = await userManager.GenerateChangeEmailTokenAsync(user, updateDto.CenterAuthorityMember.Email);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                        await userManager.ChangeEmailAsync(user, updateDto.CenterAuthorityMember.Email, code);

                        #endregion                   
                    }

                    var result = await userManager.RemovePasswordAsync(user);
                    if (result.Succeeded)
                    {
                        result = await userManager.AddPasswordAsync(user, updateDto.Password);
                    }
                }

                await unitOfWork.Save();
                TempData["success"] = $"{member.Name} Updated successfully.";

                return RedirectToAction("Index");
            }
            else
            {
                return View(updateDto);
            }
        }
    }
}
