using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NsdcTraingPartnerHub.Service.Models
{
    public class CreateProductDto
    {
        [Required(ErrorMessage = "Product Title is required!!")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "The Product Title should be between 3 to 30 characters")]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Product Serial No is required!!")]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "The Serial No should be between 3 to 15 characters")]
        public string SerialNo { get; set; }

        [Required(ErrorMessage = "Product Manufactuere is required!!")]
        public string Manufacturer { get; set; }

        [DisplayName("Price for 1 - 50 Units")]
        public double Price { get; set; }

        [DisplayName("List Price")]
        public double ListPrice { get; set; }

        [DisplayName("Price for 50 - 100 Units")]
        public double Price50 { get; set; }
        [DisplayName("Price for 100 and more Units")]
        public double Price100 { get; set; }
       
        [DisplayName("Dealer`s Price")]
        public double DelearPrice { get; set; }

        public string ImageUrl { get; set; }

        [Required]
        [DisplayName("Category")]
        public int CategoryId { get; set; }

        public CourseDto Category { get; set; }

        [Required]
        [DisplayName("Cover Type")]
        public int CoverTypeId { get; set; }

        public CenterAuthorityMemberDto CoverType { get; set; }
    }
    public class UpdateProductDto : CreateProductDto
    {

    }
    public class ProductDto : CreateProductDto
    {
        [Required]
        public int Id { get; set; }
    }
    public class DeleteProductDto
    {
        [Required]
        public int Id { get; set; }
    }
}
