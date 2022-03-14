using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NsdcTraingPartnerHub.Core.Entities;

namespace NsdcTraingPartnerHub.Web.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
       
        private readonly IMapper mapper;

        public IndexModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,           
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this.mapper = mapper;
        }

        public string Username { get; set; }
        public string Email { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
            public String Name { get; set; }
            [DisplayName("Address")]
            public string StreetAddress { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string PostalCode { get; set; }

        }

        private async Task LoadAsync(ApplicationUser user)
        {

            var userName = await _userManager.GetUserNameAsync(user);

            Email = await _userManager.GetEmailAsync(user);

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber=user.PhoneNumber,
                Name=user.Name,
                StreetAddress=user.StreetAddress,
                City=user.City,
                State=user.State,
                PostalCode=user.PostalCode
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = mapper.Map<ApplicationUser>(await _userManager.GetUserAsync(User));
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = mapper.Map<ApplicationUser>(await _userManager.GetUserAsync(User));
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            user.Name = Input.Name;
            user.StreetAddress = Input.StreetAddress;
            user.City = Input.City;
            user.State = Input.State;
            user.PostalCode = Input.PostalCode;

            await _userManager.UpdateAsync(user);

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
