using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NsdcTraingPartnerHub.Core.Entities
{
    [Table("CenterAuthorityMember")]
    public class CenterAuthorityMember
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }

        public int TrainingCenterId { get; set; }

        [ForeignKey("TrainingCenterId")]
        [ValidateNever]
        public TrainingCenter TrainingCenter { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}
