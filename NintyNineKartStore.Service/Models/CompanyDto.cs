using AutoMapper;
using NintyNineKartStore.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NintyNineKartStore.Service.Models
{
    [AutoMap(typeof(Company))]
    public class CreateCompanyDto
    {
        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "The Company Name should be between 3 to 30 characters")]
        public string Name { get; set; }

        [StringLength(300, MinimumLength = 10)]
        public string? StreetAddress { get; set; }
        [StringLength(50, MinimumLength = 3)]
        public string? City { get; set; }
        [StringLength(50, MinimumLength = 3)]
        public string? State { get; set; }
        [StringLength(6, MinimumLength = 5)]
        public string? PostalCode { get; set; }
        [StringLength(30, MinimumLength = 10)]
        public string? PhoneNumber { get; set; }
    }
    [AutoMap(typeof(Company))]
    public class UpdateCompanyDto : CreateCompanyDto
    {
        [Required]
        public int Id { get; set; }
    }

    [AutoMap(typeof(Company))]
    public class CompanyDto : UpdateCompanyDto
    {
        public DateTime CreatedOn { get; set; }
    }
}
