using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
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
using System.Security.Claims;
using System.Threading.Tasks;

namespace NintyNineKartStore.Web.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IMemoryCache cache;

        public HomeController(
            ILogger<HomeController> logger,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IMemoryCache cache)
        {
            this._logger = logger;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.cache = cache;
        }

        public async Task<IActionResult> Index(PagedRequestInput pagedRequestInput)
        {
            var products = await unitOfWork.Products.GetPagedList(mapper.Map<PagedRequest>(pagedRequestInput), null, null, new List<string> { "Category" });
            IList<ProductDto> result = mapper.Map<IList<ProductDto>>(products);

            return View(result);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
        public async Task<ActionResult> Details(int? ProductId)
        {
            ProductDto productDto = null;
            if (!cache.TryGetValue("Product_" + ProductId.GetValueOrDefault(), out productDto))
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                          .SetSlidingExpiration(TimeSpan.FromSeconds(30));
                // Fetching Product Details from Db
                productDto = mapper.Map<ProductDto>(
                    await unitOfWork.Products.Get(
                        x => x.Id == ProductId.GetValueOrDefault()
                        , new List<string>() { "Category", "CoverType" }
                        ));

                cache.Set("Product_" + ProductId.GetValueOrDefault(), productDto, cacheEntryOptions);

            }
            var cartObj = new ShoppingCartDto()
            {
                ProductDto = productDto,
                ProductId = ProductId.GetValueOrDefault(),
                Count = 1
            };

            return View(cartObj);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
