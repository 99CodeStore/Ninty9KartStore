using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NsdcTraingPartnerHub.Core.Entities
{
    [Table("TrainingCenterCourse")]
    public class TrainingCenterCourse
    {
        [Key]
        public int Id { get; set; }
        public int Seats { get; set; } = 0;
        public int CourseId { get; set; }
        [ForeignKey("CourseId")]
        [ValidateNever]
        public Course Course { get; set; }
        public int TrainingCenterId { get; set; }
        [ForeignKey("TrainingCenterId")]
        [ValidateNever]
        public TrainingCenter TrainingCenter { get; set; }
        public bool IsActive { get; set; } = false;
        public bool IsRegistrationOpen { get; set; } = false;
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}
