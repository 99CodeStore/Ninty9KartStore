using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NsdcTraingPartnerHub.Core.Entities
{
    [Table("SponsoringBody")]
    public class SponsoringBody
    {
        [Key]
        public int Id { get; set; }

        [StringLength(5, MinimumLength = 3, ErrorMessage = "The Sponsoring Body Code should be between 3 to 5 characters")]
        [Required()]
        public string Code { get; set; }

        [Required()]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "The Sponsoring Body should be between 3 to 50 characters")]
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}
