using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using NsdcTraingPartnerHub.Core.Entities;
using NsdcTraingPartnerHub.Core.Interfaces;
using NsdcTraingPartnerHub.Service.Models;
using NsdcTraingPartnerHub.Utility;
using NsdcTraingPartnerHub.Web.Areas.TrainingPartner.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
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
        private readonly IUserStore<IdentityUser> userStore;
        private readonly IUserEmailStore<IdentityUser> emailStore;

        public CenterAuthorityController(IUnitOfWork unitOfWork,
            IMapper maper,
            ILogger<CenterAuthorityController> logger,
            UserManager<IdentityUser> _userManager,
            IUserStore<IdentityUser> userStore
            )
        {
            this.unitOfWork = unitOfWork;
            this.maper = maper;
            this.logger = logger;
            userManager = _userManager;
            this.userStore = userStore;
            emailStore = GetEmailStore();
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

        public async Task<IActionResult> Create(int? centerId)
        {
            //ViewBag.SponsoringBodyList = new SelectList(
            //    maper.Map<IList<SponsoringBody>, IList<SponsoringBodyDto>>(
            //        await unitOfWork.SponsoringBodies.GetAll()
            //        ), "Id", "Name");
            if (centerId.HasValue)
            {
                TempData["CenterId"] = centerId.Value;
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] CenterAuthorityVM createDto)
        {
            if (ModelState.IsValid)
            {
                createDto.CenterAuthorityMember.TrainingCenter = null;

                if (createDto.IsCreateLogin)
                {
                    if (string.IsNullOrEmpty(createDto.Password))
                    {
                        TempData["error"] = $"Password required when Creating login.";
                        return View(createDto);
                    }

                    var existingUser = await userManager.FindByEmailAsync(createDto.CenterAuthorityMember.Email);

                    if (existingUser != null)
                    {
                        TempData["error"] = $"This email id already register with another user.";
                        ModelState.AddModelError("Email", "This email id already register with another user.");
                        return View(createDto);
                    }
                }

                var authorityMember = maper.Map<CenterAuthorityMember>(createDto.CenterAuthorityMember);

                await unitOfWork.CenterAuthorityMembers.Insert(authorityMember);

                await unitOfWork.Save();

                if (createDto.IsCreateLogin)
                {
                    await CreateMemberLogin(createDto);
                }

                TempData["success"] = $"{authorityMember.Name} successfully added as a member .";

                return RedirectToAction("Index", "CenterAuthority",
                    new { Id = createDto.CenterAuthorityMember.TrainingCenterId });
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

            UpdateCenterAuthorityVM CenterAuthorityModel = new()
            {
                CenterAuthorityMember = centerAuthorityDto,
                IsCreateLogin = appuser != null,
                Id = id.GetValueOrDefault()
            };

            return View(CenterAuthorityModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, [FromForm] UpdateCenterAuthorityVM updateDto)
        {

            if (id < 1)
            {
                ModelState.AddModelError("Error1", "Authority Member not found.");
            }

            if (updateDto.IsCreateLogin)
            {
                if (string.IsNullOrEmpty(updateDto.Password))
                {
                    TempData["error"] = $"Password required when Creating login.";
                    ModelState.AddModelError("Error1", $"Password required when Creating login.");
                }
            }

            var member = await unitOfWork.CenterAuthorityMembers.Get(x => x.Id == id.Value);

            if (member == null)
            {
                ModelState.AddModelError("Error2", "Authority Member not found.");
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
                    return View("Edit", updateDto);
                }

                maper.Map(updateDto.CenterAuthorityMember, member);

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
                else if (updateDto.IsCreateLogin)
                {
                    await CreateMemberLogin(updateDto);
                }

                await unitOfWork.Save();
                TempData["success"] = $"{member.Name} Updated successfully.";

                return RedirectToAction("Index", "CenterAuthority",
                    new { Id = updateDto.CenterAuthorityMember.TrainingCenterId });
            }
            else
            {
                return View("Edit", updateDto);
            }
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {

                throw new InvalidOperationException($"Can`t create an instance of '{nameof(ApplicationUser)}'." +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                   $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");

            }
        }

        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<IdentityUser>)userStore;
        }
        private async Task CreateMemberLogin(CenterAuthorityVM createDto)
        {
            var user = CreateUser();

            await userStore.SetUserNameAsync(user, createDto.CenterAuthorityMember.Email, CancellationToken.None);
            await emailStore.SetEmailAsync(user, createDto.CenterAuthorityMember.Email, CancellationToken.None);
            user.Name = createDto.CenterAuthorityMember.Name;
            user.PhoneNumber = createDto.CenterAuthorityMember.PhoneNo;
            user.TrainingCenterId = createDto.CenterAuthorityMember.TrainingCenterId;
            user.UserCategory = SD.UserCategory.TrainingCenterUser;
            user.TrainingCenter = null;
            //user.UserName = createDto.CenterAuthorityMember.Name;
            var result = await userManager.CreateAsync(user, createDto.Password);
            
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            else
            {
                var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                await userManager.ConfirmEmailAsync(user, code);
            }

        }
    }
}
