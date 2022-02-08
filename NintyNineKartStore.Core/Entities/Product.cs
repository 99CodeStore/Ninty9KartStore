using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NintyNineKartStore.Core.Entities
{
    [Table("Product")]
    public class Product
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Product Title is required!!")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "The Product Title should be between 5 to 30 characters")]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Product Serial No is required!!")]
        [StringLength(15,MinimumLength =3, ErrorMessage = "The Serial No should be between 3 to 15 characters")]
        public string SerialNo { get; set; }

        [Required(ErrorMessage = "Product Manufactuere is required!!")]
        public string Manufactuere { get; set; }

        [DisplayName("Max. Retail Price")]  
        public double Price { get; set; }

        [DisplayName("List Price")]
        public double ListPrice { get; set; }

        [DisplayName("50 Units Price")]
        public double Price50 { get; set; }
        [DisplayName("List Price")]
        public double Price100 { get; set; }
        [DisplayName("List Price")]
        public double DelearPrice { get; set; }

        public string ImageUrl { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        [Required]
        public int CoverTypeId { get; set; }
        
        [ForeignKey("CoverTypeId")]
        public CoverType CoverType { get; set; }        

        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}
