using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NsdcTraingPartnerHub.Core.Entities
{
    [Table("TrainingCenter")]
    public class TrainingCenter
    {
        [Key]
        public int Id { get; set; }

        [StringLength(10, MinimumLength = 3, ErrorMessage = "The Training Center Code should be between 3 to 10 characters")]
        public string CenterCode { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "The Training Center Name should be between 3 to 200 characters")]
        public string CenterName { get; set; }

        [StringLength(500, MinimumLength = 3, ErrorMessage = "The Detail should be between 3 to 500 characters")]
        public string Detail { get; set; }

        [StringLength(300, MinimumLength = 5)]
        public string? StreetAddress { get; set; }
        [StringLength(200, MinimumLength = 3)]
        public string? City { get; set; }
        [StringLength(50, MinimumLength = 3)]
        public string? State { get; set; }
        [StringLength(6, MinimumLength = 5)]
        public string? PostalCode { get; set; }

        [EmailAddress()]
        public string? Email { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 10)]
        public string PhoneNumber { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public int TrainingPartnerId { get; set; }

        [ForeignKey("TrainingPartnerId")]
        [ValidateNever]
        public virtual TrainingPartner TrainingPartner { get; set; }

        public string Status { get; set; }

    }
}
