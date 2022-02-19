using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NintyNineKartStore.Core.Entities
{
    [Table("ShoppingCart")]
    public class ShoppingCart
    {
     
        [Key()]
        public int Id { get; set; }

        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product {get;set;}
        public int Count {get;set;}

        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public double Price { get; set; }

    }
}
