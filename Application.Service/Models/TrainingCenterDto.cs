using AutoMapper;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using NsdcTraingPartnerHub.Core.Entities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NsdcTraingPartnerHub.Service.Models
{
    [AutoMap(typeof(TrainingCenter))]
    public class CreateTrainingCenterDto
    {
        [StringLength(10, MinimumLength = 3, ErrorMessage = "The Training Center Code should be between 3 to 10 characters")]
        public string CenterCode { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "The Company Name should be between 3 to 100 characters")]
        [DisplayName("Training Center Name")]
        public string CenterName { get; set; }
        [StringLength(500, MinimumLength = 3, ErrorMessage = "The Detail should be between 3 to 500 characters")]
        public string Detail { get; set; }

        [DisplayName("Address")]

        [StringLength(300, MinimumLength = 3, ErrorMessage = "The Address be between 3 to 500 characters")]
        public string? StreetAddress { get; set; }
        [StringLength(50, MinimumLength = 3)]
        public string? City { get; set; }
        [StringLength(50, MinimumLength = 3)]
        public string? State { get; set; }
        [StringLength(6, MinimumLength = 5)]
        public string? PostalCode { get; set; }
       
        [StringLength(30, MinimumLength = 10)]
        [Required]
        public string PhoneNumber { get; set; }

        [EmailAddress()]
        public string? Email { get; set; }
        [Required]
        public int? TrainingPartnerId { get; set; }

        [DisplayName("Training Partner")]
        [ForeignKey("TrainingPartnerId")]
        [ValidateNever]
        public virtual TrainingPartnerDto TrainingPartner { get; set; }
        public string Status { get; set; } 
    }
    [AutoMap(typeof(TrainingCenter))]
    public class UpdateCompanyDto : CreateTrainingCenterDto
    {
        [Required]
        public int Id { get; set; }
    }

    [AutoMap(typeof(TrainingCenter))]
    public class TrainingCenterDto : UpdateCompanyDto
    {
        public DateTime CreatedOn { get; set; }
    }
}
