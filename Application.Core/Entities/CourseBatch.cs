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
    [Table("TcCourseBatch")]
    public  class CourseBatch
    {
        [Key]
        public int Id { get; set; }
        [Required()]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "The Batch Name should be between 3 to 50 characters")]
        public string BatchName { get; set; }
        public DateTime? StartFrom { get; set; }
        public DateTime? EndTo{ get; set; }
        public int CourseId { get; set; }
        [ForeignKey("CourseId")]
        [ValidateNever]
        public Course Course { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}
