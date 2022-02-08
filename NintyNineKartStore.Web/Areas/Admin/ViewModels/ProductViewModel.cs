using Microsoft.AspNetCore.Mvc.Rendering;
using NintyNineKartStore.Service.Models;
using System.Collections.Generic;

namespace NintyNineKartStore.Web.Areas.Admin.ViewModels
{
    public class ProductViewModel
    {
        public ProductDto Product { get; set; }
        public IEnumerable<SelectListItem> CategoryList { get; set; }
        public IEnumerable<SelectListItem> CoverTypeList { get; set; }
    }
}
