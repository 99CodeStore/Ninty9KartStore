using NsdcTraingPartnerHub.Service.Models;
using System.Collections.Generic;

namespace NsdcTraingPartnerHub.Web.Areas.Admin.ViewModels
{
    public class OrderViewModel
    {
        public OrderHeaderDto OrderHeader{ get; set; }
        public IEnumerable<OrderDetailDto> OrderDetails { get; set; }
    }
}
