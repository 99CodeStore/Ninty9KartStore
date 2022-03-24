using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using NsdcTraingPartnerHub.Core.Interfaces;
using NsdcTraingPartnerHub.Service.Models;
using NsdcTraingPartnerHub.Utility;
using NsdcTraingPartnerHub.Web.Areas.TrainingCenter.Models;
using NsdcTraingPartnerHub.Web.Models;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NsdcTraingPartnerHub.Web.Controllers
{
    [Area("TrainingCenter")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IMemoryCache cache;

        [BindProperty]
        public UserDashboardViewModel UserDashboardVM { get; set; }

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
            //var products = await unitOfWork.Products.GetPagedList(mapper.Map<PagedRequest>(pagedRequestInput), null, null, new List<string> { "Category" });
            //  IList<ProductDto> result = mapper.Map<IList<ProductDto>>(products);
            //result

            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.Claims.FirstOrDefault();

            if (claim != null)
            {
                var user = await unitOfWork.ApplicationUsers.Get(u => u.Id == claim.Value);
                if (user != null)
                {
                    UserDashboardVM = new() { Name = user.Name, UserCategory = user.UserCategory };

                    if (user.UserCategory == SD.UserCategory.TrainingCenterUser)
                    {
                        UserDashboardVM.TrainingCenter = mapper.Map<TrainingCenterDto>(await unitOfWork.TrainingCenters.Get(tc => tc.Id == user.TrainingCenterId));
                        HttpContext.Session.SetInt32(SD.TrainingCenterId, user.TrainingCenterId.GetValueOrDefault());
                    }
                }
            }

            return View(UserDashboardVM);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
        //public async Task<ActionResult> Details(int? ProductId)
        //{
        //    ProductDto productDto = null;
        //    if (!cache.TryGetValue("Product_" + ProductId.GetValueOrDefault(), out productDto))
        //    {
        //        var cacheEntryOptions = new MemoryCacheEntryOptions()
        //                  .SetSlidingExpiration(TimeSpan.FromSeconds(30));
        //        // Fetching Product Details from Db
        //        productDto = mapper.Map<ProductDto>(
        //            await unitOfWork.Products.Get(
        //                x => x.Id == ProductId.GetValueOrDefault()
        //                , new List<string>() { "Category", "CoverType" }
        //                ));

        //        cache.Set("Product_" + ProductId.GetValueOrDefault(), productDto, cacheEntryOptions);

        //    }
        //    var cartObj = new ShoppingCartDto()
        //    {
        //        ProductDto = productDto,
        //        ProductId = ProductId.GetValueOrDefault(),
        //        Count = 1
        //    };

        //    return View(cartObj);
        //}
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
