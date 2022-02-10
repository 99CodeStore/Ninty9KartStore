using NintyNineKartStore.Service.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NintyNineKartStore.Web.Areas.Customer.ViewModels
{
    public class ShoppingCartViewModel
    {
        public ProductDto productDto { get; set; }
        [Range(1,1000,ErrorMessage="Per Product`s Cart Quantity should be 1 to 1000")]
        public int Count { get; set; }
    }
}
