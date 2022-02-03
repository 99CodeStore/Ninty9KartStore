using System;
using System.ComponentModel.DataAnnotations;

namespace NintyNineKartStore.Service.Models
{
    public class CreateCoverTypeDto
    {
        [Required(ErrorMessage = "CoverType Name is required!!")]
        [StringLength(25, MinimumLength = 3, ErrorMessage = "The CoverType Name should be between 3 to 25 characters")]
        public string Name { get; set; }
    }
    public class CoverTypeDto : CreateCoverTypeDto
    {
        [Required]
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }

    public class UpdateCoverTypeDto : CreateCoverTypeDto
    {

    }
    public class DeleteCoverTypeDto
    {
        [Required]
        public int Id { get; set; }
    }
}
