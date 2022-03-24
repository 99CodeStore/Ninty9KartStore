using NsdcTraingPartnerHub.Service.Models;
using System.ComponentModel.DataAnnotations;

namespace NsdcTraingPartnerHub.Web.Areas.TrainingPartner.Models
{
    public class CenterAuthorityVM
    {
        public CreateCenterAuthorityMemberDto CenterAuthorityMember { get; set; }
        
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string? Role { get; set; }

        public bool IsCreateLogin { get; set; }

    }

    public class UpdateCenterAuthorityVM : CenterAuthorityVM
    { 
        [Required]
        public int Id { get; set; }

    }
}
