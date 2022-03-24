using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NsdcTraingPartnerHub.Service.Models
{
    public class CreateCourseDto
    {
        [Required(ErrorMessage = "Course Name is required!!")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "The Course Name should be between 3 to 100 characters")]
        [DisplayName("Course Name")]
        public string CourseName { get; set; }

        [StringLength(10, MinimumLength = 3, ErrorMessage = "The Course Code should be between 3 to 10 characters")]
        [Required()]
        [DisplayName("Course Code")]
        public string CourseCode { get; set; }

        [StringLength(500, MinimumLength = 3, ErrorMessage = "The Course Description should be between 20 to 500 characters")]
        public string Description { get; set; }
        [Range(1, 500, ErrorMessage = "Duration must be between 1 to 500 only!!")]
        public int Duration { get; set; }
        public string DurationUnit { get; set; }

        [Required()]
        public int SponsoringBodyId { get; set; }
        
        [DisplayName("Sponsoring Body")]
        [ForeignKey("SponsoringBodyId")]
        [ValidateNever]
        public virtual SponsoringBodyDto SponsoringBody { get; set; }

        public int? TrainingPartnerId { get; set; }

        [DisplayName("Training Partner")]
        [ForeignKey("TrainingPartnerId")]
        [ValidateNever]
        public virtual TrainingPartnerDto TrainingPartner { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        [ForeignKey("JobSectorId")]
        [ValidateNever]
        public JobSectorDto JobSector { get; set; }
        [Required]
        public int? JobSectorId { get; set; }

    }

    public class UpdateCourseDto : CreateCourseDto
    {

    }

    public class CourseDto : CreateCourseDto
    {
        [Required]
        public int Id { get; set; }
    }
    public class DeleteCourseDto
    {
        [Required]
        public int Id { get; set; }
    }
}
