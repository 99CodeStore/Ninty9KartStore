using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NintyNineKartStore.Core.Entities
{
    [Table("Company")]
    public  class Company
    {
        [Key]
        public int Id { get; set; }
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
        [StringLength(10, MinimumLength = 30)]
        public string? PhoneNumber { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}
