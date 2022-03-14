using NsdcTraingPartnerHub.Core.Entities;
using NsdcTraingPartnerHub.Service.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NsdcTraingPartnerHub.Web.Areas.Customer.ViewModels
{
    public class ShoppingCartViewModel
    {

        public IList<ShoppingCartDto> ShoppingCartItems { get; set; }

        public OrderHeaderDto OrderHeader { get; set; }

    }
}
