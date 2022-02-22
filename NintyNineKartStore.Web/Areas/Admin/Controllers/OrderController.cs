using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NintyNineKartStore.Core.Entities;
using NintyNineKartStore.Core.Interfaces;
using NintyNineKartStore.Service.Models;
using NintyNineKartStore.Utility;
using NintyNineKartStore.Web.Areas.Admin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NintyNineKartStore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper maper;
        private readonly ILogger<OrderController> logger;

        public OrderViewModel OrderViewModel { get; set; }


        public OrderController(IUnitOfWork unitOfWork,
            IMapper maper,
            ILogger<OrderController> logger)
        {
            this.unitOfWork = unitOfWork;
            this.maper = maper;
            this.logger = logger;
        }
        public async Task<IActionResult> Detail(int? orderId)
        {
            OrderViewModel = new();

            OrderViewModel.OrderHeader = maper.Map<OrderHeaderDto>(
                await unitOfWork.OrderHeaders.Get(o => o.Id == orderId.GetValueOrDefault(), new List<string> { "ApplicationUser" }));

            OrderViewModel.OrderDetails = maper.Map<IEnumerable<OrderDetailDto>>(
              await unitOfWork.OrderDetails.GetAll(o => o.OrderId == orderId.GetValueOrDefault(), null, new List<string> { "Product" }));

            return View(OrderViewModel);
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetPaggedList(OrderPageFilter pageFilter)
        {
            Expression<Func<OrderHeader, bool>> dataFilter = null;

            if (User.IsInRole(SD.Role_User_Admin) || User.IsInRole(SD.Role_User_Employee))
            {
                switch (pageFilter.OrderStatus)
                {
                    case "pending":
                        dataFilter = (o) => o.OrderStatus == SD.OrderStatus.OrderPending;
                        break;
                    case "completed":
                        dataFilter = (o) => o.OrderStatus == SD.OrderStatus.OrderShipped;
                        break;
                    case "inprocess":
                        dataFilter = (o) => o.OrderStatus == SD.OrderStatus.OrderInProcess;
                        break;
                    case "cancelled":
                        dataFilter = (o) => o.OrderStatus == SD.OrderStatus.OrderCancelled;
                        break;
                    case "approved":
                        dataFilter = (o) => o.OrderStatus == SD.OrderStatus.OrderApproved;
                        break;
                    default:
                        dataFilter = null;
                        break;
                }
            }
            else
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

                switch (pageFilter.OrderStatus)
                {
                    case "pending":
                        dataFilter = (o) => o.OrderStatus == SD.OrderStatus.OrderPending && claims.Value == o.ApplicationUserId;
                        break;
                    case "completed":
                        dataFilter = (o) => o.OrderStatus == SD.OrderStatus.OrderShipped && claims.Value == o.ApplicationUserId;
                        break;
                    case "inprocess":
                        dataFilter = (o) => o.OrderStatus == SD.OrderStatus.OrderInProcess && claims.Value == o.ApplicationUserId;
                        break;
                    case "cancelled":
                        dataFilter = (o) => o.OrderStatus == SD.OrderStatus.OrderCancelled && claims.Value == o.ApplicationUserId;
                        break;
                    case "approved":
                        dataFilter = (o) => o.OrderStatus == SD.OrderStatus.OrderApproved && claims.Value == o.ApplicationUserId;
                        break;
                    default:
                        dataFilter = null;
                        break;
                }
            }


            var orders = await unitOfWork.OrderHeaders.GetPagedList(maper.Map<PagedRequest>(pageFilter), dataFilter, null, new List<string> { "ApplicationUser" });

            IList<OrderHeaderDto> orderDtos = maper.Map<IList<OrderHeaderDto>>(orders);
            return Json(new { Data = orderDtos });

        }
    }
}
