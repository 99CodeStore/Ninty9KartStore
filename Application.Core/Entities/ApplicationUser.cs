using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using NsdcTraingPartnerHub.Utility;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NsdcTraingPartnerHub.Core.Entities
{
    [Table("AspNetUsers")]
    public class ApplicationUser : IdentityUser
    {
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

        public int? TrainingCenterId { get; set; }

        [ForeignKey("TrainingCenterId")]
        [ValidateNever]
        public TrainingCenter TrainingCenter { get; set; }

        public string UserCategory { get; set; } = SD.UserCategory.TrainingCenterUser;

    }
}
