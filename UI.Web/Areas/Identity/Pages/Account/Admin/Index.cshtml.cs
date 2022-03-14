using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using NsdcTraingPartnerHub.Core.Entities;
using NsdcTraingPartnerHub.Core.Interfaces;
using NsdcTraingPartnerHub.Utility;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;

namespace NsdcTraingPartnerHub.Web.Areas.Identity.Pages.Account.Admin
{
    public class IndexModel : PageModel
    {
        private readonly IUserStore<IdentityUser> userStore;
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly ILogger<RegisterModel> logger;
        private readonly IEmailSender emailSender;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper maper;
        private readonly IUserEmailStore<IdentityUser> emailStore;

        public IndexModel(IUserStore<IdentityUser> userStore,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager,
            IUnitOfWork unitOfWork,
             IMapper maper)
        {
            this.userStore = userStore;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
            this.emailSender = emailSender;
            this.roleManager = roleManager;
            this.unitOfWork = unitOfWork;
            this.maper = maper;

            emailStore = GetEmailStore();
        }

        public async Task<IActionResult> OnGet()
        {
            if (!await roleManager.RoleExistsAsync(SD.UserRole.AdminUser))
            {
                await roleManager.CreateAsync(new IdentityRole(SD.UserRole.AdminUser));
                await roleManager.CreateAsync(new IdentityRole(SD.UserRole.TraingPartnerAdminUser));
                await roleManager.CreateAsync(new IdentityRole(SD.UserRole.TrainingCenterUser));
                await roleManager.CreateAsync(new IdentityRole(SD.UserRole.CenterAdminUser));
                await roleManager.CreateAsync(new IdentityRole(SD.UserRole.TranineeUser));
                await roleManager.CreateAsync(new IdentityRole(SD.UserRole.ReportUser));
                await roleManager.CreateAsync(new IdentityRole(SD.UserRole.TraingPartnerUser));
            }

            string returnUrl = null;

            returnUrl ??= Url.Content("~/");

            var appUser = await unitOfWork.ApplicationUsers.GetAll(u => u.UserCategory == SD.UserCategory.TrainingPartnerUser);

            if (appUser == null || appUser.Count == 0)
            {
                return Page();
            }
            else
            {
                return LocalRedirect(returnUrl);
            }

        }
        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = CreateUser();
                await userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                user.StreetAddress = Input.StreetAddress;
                user.Name = Input.Name;
                user.City = Input.City;
                user.State = Input.State;
                user.PostalCode = Input.PostalCode;
                user.PhoneNumber = Input.PhoneNumber;
                user.UserCategory = SD.UserCategory.TrainingPartnerUser;

                var result = await userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    logger.LogInformation("A Admin User account created  with password.");

                    await userManager.AddToRoleAsync(user, SD.UserRole.AdminUser);

                    var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }
        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required()]
            [StringLength(30, MinimumLength = 3)]
            public string Name { get; set; }
            [StringLength(300, MinimumLength = 10)]
            public string? StreetAddress { get; set; }
            [StringLength(50, MinimumLength = 3)]
            public string? City { get; set; }
            [StringLength(50, MinimumLength = 3)]
            public string? State { get; set; }
            [StringLength(6, MinimumLength = 5)]
            public string? PostalCode { get; set; }
            public string? PhoneNumber { get; set; }

            public string? Role { get; set; }
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
    }
}
