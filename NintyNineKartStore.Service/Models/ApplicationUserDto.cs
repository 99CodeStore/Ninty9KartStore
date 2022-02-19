using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NintyNineKartStore.Service.Models
{
    public class ApplicationUserDto
    {
        [Required()]
        [StringLength(30, MinimumLength = 3)]
        public string Name { get; set; }
        [StringLength(300, MinimumLength = 10)]
        public string? StreetAddress { get; set; }
        [StringLength(50, MinimumLength = 3)]
        public string? City { get; set; }
        [StringLength(50, MinimumLength = 3)]
        public string? State { get; set; }
        [StringLength(6, MinimumLength = 5)]
        public string? PostalCode { get; set; }

        public int? CompanyId { get; set; }

        [ForeignKey("CompanyId")]
        [ValidateNever]
        public CompanyDto Company { get; set; }

        public string PhoneNumber { get; set; }

    }
}
