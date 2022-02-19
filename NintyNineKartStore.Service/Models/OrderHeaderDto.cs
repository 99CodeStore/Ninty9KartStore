using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NintyNineKartStore.Service.Models
{
    public class CreateOrderHeaderDto
    {
        public string ApplicationUserId { get; set; }
        [ValidateNever]
        [ForeignKey("ApplicationUserId")]
        public ApplicationUserDto ApplicationUser { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }
        public DateTime? ShippingDate { get; set; }
        public double OrderTotal { get; set; } = 0.0D;
        public string? OrderStatus { get; set; }
        public string? PaymentStatus { get; set; }
        public string? TrackingNumber { get; set; }
        public string? Carrier { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime? PaymentDueDate { get; set; }

        public string? SessionId { get; set; }
        public string? PaymentIntentId { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string StreetAddress { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string PostalCode { get; set; }
        [Required]
        public string Name { get; set; }
    }

    public class OrderHeaderDto : CreateOrderHeaderDto
    {
        public int Id { get; set; }
    }
}
