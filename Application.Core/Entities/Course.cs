using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NsdcTraingPartnerHub.Core.Entities
{
    [Table("Course")]
    public class Course
    {
        [Key]
        public int Id { get; set; }

        [StringLength(5, MinimumLength = 3, ErrorMessage = "The Course Code should be between 3 to 5 characters")]
        [Required()]
        public string CourseCode { get; set; }

        [Required()]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "The Course Name should be between 3 to 50 characters")]
        public string CourseName { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public string DurationUnit { get; set; }

        [Required()]
        public int SponsoringBodyId { get; set; }
        [ForeignKey("SponsoringBodyId")]
        [ValidateNever]
        public virtual SponsoringBody SponsoringBody { get; set; }

        public int? TrainingPartnerId { get; set; }
        [ForeignKey("TrainingPartnerId")]
        [ValidateNever]
        public virtual TrainingPartner TrainingPartner { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    }
}
