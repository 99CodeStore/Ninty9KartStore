using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NsdcTraingPartnerHub.Core.Entities
{
    [Table("TcCourseBatch")]
    public class CourseBatch
    {
        [Key]
        public int Id { get; set; }

        [Required()]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "The Batch Name should be between 3 to 100 characters")]
        public string BatchName { get; set; }

        [DisplayName("Start from")]
        [DataType(DataType.Date)]
        [Required()]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? StartFrom { get; set; }
        [DataType(DataType.Date)]
        public DateTime? EndTo { get; set; }

        [Required()]
        public int CourseId { get; set; }

        [ForeignKey("CourseId")]
        [ValidateNever]
        public Course Course { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        [StringLength(450, MinimumLength = 20)]
        public string ApplicationUserId { get; set; }

        [ValidateNever]
        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
