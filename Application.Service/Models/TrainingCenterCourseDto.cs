using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NsdcTraingPartnerHub.Service.Models
{
    public class CreateTrainingCenterCourseDto
    {
        public int Seats { get; set; } = 0;
       [Required]
        public int CourseId { get; set; }
       
        [ForeignKey("CourseId")]
        [ValidateNever]
        public virtual CourseDto Course { get; set; }
        public int TrainingCenterId { get; set; }
       
        [ForeignKey("TrainingCenterId")]
        [ValidateNever]
        public virtual TrainingCenterDto TrainingCenter { get; set; }
     
        [DisplayName("Open for Student Registration")]
        public bool IsRegistrationOpen { get; set; } = false;
    }
    public class TrainingCenterCourseDto: CreateTrainingCenterCourseDto
    {
        [Required]
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow; 
        public bool IsActive { get; set; } = true;
    }
}
