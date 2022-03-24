﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NsdcTraingPartnerHub.Core.Entities
{
    [Table("Country")]
    public class Country
    {
        [Key]
        public int Id { get; set; }
       
        [StringLength(5, MinimumLength = 2, ErrorMessage = "The Country should be between 2 to 5 characters")]
        [Required()]
        public string? CountryCode { get; set; }

        [StringLength(100, MinimumLength = 3, ErrorMessage = "The Country Name should be between 3 to 100 characters")]
        [Required()]
        public string Name { get; set; }

    }
}
