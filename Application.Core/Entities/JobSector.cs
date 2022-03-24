using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NsdcTraingPartnerHub.Core.Entities
{
    [Table("JobSector")]
    public class JobSector
    {
        [Key]
        public int Id { get; set; }

        [StringLength(100, MinimumLength = 3, ErrorMessage = "The Job Sector Name should be between 3 to 100 characters")]
        [Required()]
        public string SectorName { get; set; }
        [StringLength(500, MinimumLength = 3, ErrorMessage = "The Description should be between 3 to 500 characters")]
        public string Description { get; set; }
        public DateTime? CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedOn { get; set; } = DateTime.UtcNow;

    }
}
