using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NintyNineKartStore.Service.Models
{
    public class CreateCategoryDto
    {
        [Required(ErrorMessage = "Category Name is required!!")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "The Category Name should be between 5 to 15 characters")]
        public string Name { get; set; }
        [DisplayName("Display Order")]
        [Range(1, 500, ErrorMessage = "Display Order must be between 1 to 500 only!!")]
        public int DisplayOrder { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
   
    public class UpdateCategoryDto : CreateCategoryDto
    {

    }

    public class CategoryDto : CreateCategoryDto
    {
        [Required]
        public int Id { get; set; }
    }
}
