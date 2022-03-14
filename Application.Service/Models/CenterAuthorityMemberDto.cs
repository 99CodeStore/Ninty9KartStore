using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using NsdcTraingPartnerHub.Core.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NsdcTraingPartnerHub.Service.Models
{
    public class CreateCenterAuthorityMemberDto
    {
        [Required(ErrorMessage = "Authority Member Name is required!!")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "The Name should be between 3 to 50 characters")]
        public string Name { get; set; }
        public string Designation { get; set; }
        [EmailAddress()]
        [Required()]
        public string Email { get; set; }
        public string PhoneNo { get; set; }

        public int TrainingCenterId { get; set; }

        [ForeignKey("TrainingCenterId")]
        [ValidateNever]
        public virtual TrainingCenter TrainingCenter { get; set; }

    }
    public class CenterAuthorityMemberDto : CreateCenterAuthorityMemberDto
    {
        [Required]
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }

    public class UpdateCenterAuthorityMemberDto : CreateCenterAuthorityMemberDto
    {

    }
    public class DeleteCenterAuthorityMemberDto
    {
        [Required]
        public int Id { get; set; }
    }
}
