using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NsdcTraingPartnerHub.Service.Models
{
    public class CreateStudentDto
    {
        [DisplayName("Registration No")]
        [StringLength(30, MinimumLength = 5)]
        public string? RegistrationNo { get; set; }

        [DisplayName("Student`s First Name")]
        [Required()]
        [StringLength(50, MinimumLength = 3)]
        public string FirstName { get; set; }

        [DisplayName("Student`s Last Name")]
        [StringLength(20, MinimumLength = 2)]
        public string LastName { get; set; }

        [DisplayName("Father`s Name")]
        [Required()]
        [StringLength(100, MinimumLength = 3)]
        public string FatherName { get; set; }

        [DisplayName("Mother`s Name")]
        [Required()]
        [StringLength(100, MinimumLength = 3)]
        public string MotherName { get; set; }

        [DisplayName("Date of Birth")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? DOB { get; set; }

        [DisplayName("Caste Category")]
        public string CasteCategory { get; set; }

        [DisplayName("Address Line 1")]
        [StringLength(300, MinimumLength = 10)]
        public string? StreetAddress1 { get; set; }

        [DisplayName("Address Line 2")]
        [StringLength(300, MinimumLength = 10)]
        public string? StreetAddress2 { get; set; }

        [DisplayName("Address Line 3")]
        [StringLength(300, MinimumLength = 10)]
        public string? StreetAddress3 { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string? City { get; set; }
        [StringLength(50, MinimumLength = 3)]
        public string? State { get; set; }
        [StringLength(6, MinimumLength = 5)]
        [DisplayName("Postal Code")]
        public string? PostalCode { get; set; }

        public string Email { get; set; }
        [DisplayName("Email Id")]
        public string PhoneNumber { get; set; }

        [DisplayName("Course")]
        public int CourseId { get; set; }

        [DisplayName("Training Center")]
        public int TrainingCenterId { get; set; }
        [DisplayName("Sponsoring Body")]
        public int SponsoringBodyId { get; set; }

        public string ApplicationUserId { get; set; }

        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public virtual ApplicationUserDto ApplicationUser { get; set; }

        [ForeignKey("TrainingCenterId")]
        [ValidateNever]
        public virtual TrainingCenterDto TrainingCenter { get; set; }

        [DisplayName("Sponsoring Body")]
        [ForeignKey("SponsoringBodyId")]
        [ValidateNever]
        public virtual SponsoringBodyDto SponsoringBody { get; set; }

        [DisplayName("Course")]
        [ForeignKey("CourseId")]
        [ValidateNever]
        public virtual CourseDto Course { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime LastChangedOn { get; set; } = DateTime.UtcNow;

    }
    public class StudentDto : CreateStudentDto
    {
        [Required]
        public Guid Id { get; set; }

    }

    public class StudentPageFilter : PagedRequestInput
    {
        public int? CourseId { get; set; }
        public string? SponseringBodyId { get; set; }
    }
}
