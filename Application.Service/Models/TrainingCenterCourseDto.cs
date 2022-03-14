using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NsdcTraingPartnerHub.Service.Models
{
    public class CreateTrainingCenterCourseDto
    {
        public int Seats { get; set; } = 0;
        public int CourseId { get; set; }
       
        [ForeignKey("CourseId")]
        [ValidateNever]
        public CourseDto Course { get; set; }
        public int TrainingCenterId { get; set; }
       
        [ForeignKey("TrainingCenterId")]
        [ValidateNever]
        public TrainingCenterDto TrainingCenter { get; set; }
     
        public bool IsRegistrationOpen { get; set; } = false;
    }
    public class TrainingCenterCourseDto: CreateTrainingCenterCourseDto
    {
        [Required]
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow; 
        public bool IsActive { get; set; } = false;
    }
}
