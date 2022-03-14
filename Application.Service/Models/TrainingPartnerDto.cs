using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NsdcTraingPartnerHub.Service.Models
{
    public class CreateTrainingPartnerDto
    {
        [Required]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "The Training Partner Name should be between 3 to 200 characters")]

        public string PartnerName { get; set; }

        [StringLength(500, MinimumLength = 3, ErrorMessage = "The Detail should be between 3 to 500 characters")]
        public string Detail { get; set; }

        [StringLength(300, MinimumLength = 10)]
        public string? StreetAddress { get; set; }
        [StringLength(200, MinimumLength = 3)]
        public string? City { get; set; }
        [StringLength(50, MinimumLength = 3)]
        public string? State { get; set; }
        [StringLength(6, MinimumLength = 5)]
        public string? PostalCode { get; set; }

        [EmailAddress()]
        public string? Email { get; set; }
        [Required]
        [StringLength(10, MinimumLength = 30)]
        public string? PhoneNumber { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
    public class TrainingPartnerDto : CreateTrainingPartnerDto
    {
        [Required]
        public int Id { get; set; }
        
    }
}
