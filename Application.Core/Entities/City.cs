using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NsdcTraingPartnerHub.Core.Entities
{
    [Table("City")]
    public class City {
        [Key]
        public int Id { get; set; }

        [StringLength(5, MinimumLength = 2, ErrorMessage = "The City should be between 2 to 5 characters")]
        [Required()]
        public string CityCode { get; set; }
       
        [StringLength(100, MinimumLength = 3, ErrorMessage = "The City Name should be between 3 to 100 characters")]
        [Required()]
        public string CityName { get; set; }
        [Required()]
        public int? CountryId { get; set; }
        [Required()]
        public int? StateId { get; set; }

        [ForeignKey("StateId")]
        [ValidateNever]
        public virtual State State { get; set; }
        
        [ForeignKey("CountryId")]
        [ValidateNever]
        public virtual Country Country { get; set; }

    }
}
