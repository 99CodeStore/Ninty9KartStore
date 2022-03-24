using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NsdcTraingPartnerHub.Service.Models
{
    public class CreateCourseBatchDto
    {
        [DisplayName("Batch Name")]
        [Required()]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "The Batch Name should be between 3 to 100 characters")]
        public string BatchName { get; set; }

        [DisplayName("Start from")]
        [DataType(DataType.Date)]
        [Required()]
       // [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]       
        public DateTime? StartFrom { get; set; }

        [DataType(DataType.Date)]
        public DateTime? EndTo { get; set; }

        [DisplayName("Course")]
        [Required()]
        public int CourseId { get; set; }

        [ForeignKey("CourseId")]
        [ValidateNever]
        [DisplayName("Course Name")]
        public virtual CourseDto Course { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        [DisplayName("User")]
        public string ApplicationUserId { get; set; }

        [ValidateNever]
        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUserDto ApplicationUser { get; set; }
    }

    public class CourseBatchDto : CreateCourseBatchDto
    {
        [Required]
        public int Id { get; set; }
    }
}
