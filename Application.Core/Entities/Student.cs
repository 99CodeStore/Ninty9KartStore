using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NsdcTraingPartnerHub.Core.Entities
{
    [Table("Student")]
    public class Student
    {
        [Key]
        public Guid? Id { get; set; }
        [Required()]
        [StringLength(50, MinimumLength = 3)]
        public string FirstName { get; set; }
       
        [StringLength(20, MinimumLength = 2)]
        public string LastName { get; set; }

        [Required()]
        [StringLength(100, MinimumLength = 3)]
        public string FatherName { get; set; }

        [Required()]
        [StringLength(100, MinimumLength = 3)]
        public string MotherName { get; set; }

        public DateTime? DOB { get; set; }

        public string CasteCategory { get; set; }

        [StringLength(300, MinimumLength = 10)]
        public string? StreetAddress1 { get; set; }

        [StringLength(300, MinimumLength = 10)]
        public string? StreetAddress2 { get; set; }

        [StringLength(300, MinimumLength = 10)]
        public string? StreetAddress3 { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string? City { get; set; }
        [StringLength(50, MinimumLength = 3)]
        public string? State { get; set; }
        [StringLength(6, MinimumLength = 5)]
        public string? PostalCode { get; set; }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public int CourseId { get; set; }
        public int TrainingCenterId { get; set; }
        public int SponsoringBodyId { get; set; }

        public string ApplicationUserId { get; set; }
       
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public virtual ApplicationUser ApplicationUser { get; set; }
        
        [ForeignKey("TrainingCenterId")]
        [ValidateNever]
        public virtual TrainingCenter TrainingCenter { get; set; }

        [ForeignKey("SponsoringBodyId")]
        [ValidateNever]
        public virtual SponsoringBody SponsoringBody { get; set; }
       
        [ForeignKey("CourseId")]
        [ValidateNever]
        public virtual Course Course { get; set; }

    }
}
