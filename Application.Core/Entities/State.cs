using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NsdcTraingPartnerHub.Core.Entities
{
    [Table("State")]
    public class State
    {
        [Key]
        public int Id { get; set; }

        [StringLength(5, MinimumLength = 2, ErrorMessage = "The State should be between 2 to 5 characters")]
        [Required()]
        public string? StateCode { get; set; }

        [StringLength(100, MinimumLength = 3, ErrorMessage = "The State Name should be between 3 to 100 characters")]
        [Required()]
        public string Name { get; set; }
        [Required()]
        public int? CountryId { get; set; }

        [ForeignKey("CountryId")]
        [ValidateNever]
        public virtual Country Country { get; set; }
    }
}
