using NintyNineKartStore.Service.Models;
using System.Collections.Generic;

namespace NintyNineKartStore.Web.Areas.Admin.ViewModels
{
    public class OrderViewModel
    {
        public OrderHeaderDto OrderHeader{ get; set; }
        public IEnumerable<OrderDetailDto> OrderDetails { get; set; }
    }
}
