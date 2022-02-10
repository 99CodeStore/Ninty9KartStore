using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NintyNineKartStore.Core.Entities;
using NintyNineKartStore.Core.Interfaces;
using NintyNineKartStore.Service.Models;
using NintyNineKartStore.Web.Areas.Customer.ViewModels;
using NintyNineKartStore.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace NintyNineKartStore.Web.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper maper;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork, IMapper maper)
        {
            this._logger = logger;
            this.unitOfWork = unitOfWork;
            this.maper = maper;
        }

        public async Task<IActionResult> Index(PagedRequestInput pagedRequestInput)
        {
            var products = await unitOfWork.Products.GetPagedList(maper.Map<PagedRequest>(pagedRequestInput), null, null, new List<string> { "Category" });
            IList<ProductDto> result = maper.Map<IList<ProductDto>>(products);

            return View(result);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<ActionResult> Details(int? Id)
        {
            var cartObj = new ShoppingCartViewModel()
            {
                productDto = maper.Map<ProductDto>(
                    await unitOfWork.Products.Get(
                        x => x.Id == Id.GetValueOrDefault()
                        ,new List<string>() { "Category","CoverType" }
                        )),
                Count = 1
            };
            return View(cartObj);
        }
    }
}
