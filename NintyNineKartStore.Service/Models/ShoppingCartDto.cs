using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NintyNineKartStore.Service.Models
{
    public class CreateShoppingCartDto
    {
        [DisplayName("Product")]
        [Required(ErrorMessage = "Product is required!!")]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public ProductDto ProductDto { get; set; }

        [DisplayName("Quantity")]
        [Range(1, 1000, ErrorMessage = "Per Product`s Cart Quantity should be 1 to 1000")]
        public int Count { get; set; }

        [Required(ErrorMessage = "ApplicationUser is required!!")]
        public string ApplicationUserId { get; set; }
        
        [ValidateNever]
        public ApplicationUserDto ApplicationUser { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public double Price { get; set; }
    }

    public class UpdateShoppingCartDto : CreateShoppingCartDto
    {

    }

    public class ShoppingCartDto : CreateShoppingCartDto
    {
        [Required]
        public int Id { get; set; }
    }
    public class DeleteShoppingCartDto
    {
        [Required]
        public int Id { get; set; }
    }
}
