using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NintyNineKartStore.Core.Entities
{
    [Table("CoverType")]
    public class CoverType
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = "CoverType Name is required!!")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "The CoverType Name should be between 5 to 20 characters")]
        public string Name { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}
