using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NsdcTraingPartnerHub.Service.Models
{
    public class CreateJobSectorDto
    {
        [StringLength(100, MinimumLength = 3, ErrorMessage = "The Job Sector Name should be between 3 to 100 characters")]
        [Required()]
        public string SectorName { get; set; }
        [StringLength(500, MinimumLength = 3, ErrorMessage = "The Description should be between 3 to 500 characters")]
        public string Description { get; set; }

    }
    public class JobSectorDto : CreateJobSectorDto
    {
        public int? Id { get; set; }
        public DateTime? CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedOn { get; set; } = DateTime.UtcNow;
    }
}
