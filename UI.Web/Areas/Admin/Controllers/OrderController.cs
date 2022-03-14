using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NsdcTraingPartnerHub.Core.Entities;
using NsdcTraingPartnerHub.Core.Interfaces;
using NsdcTraingPartnerHub.Service.Models;
using NsdcTraingPartnerHub.Utility;
using NsdcTraingPartnerHub.Web.Areas.Admin.ViewModels;
using Stripe;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;


namespace NsdcTraingPartnerHub.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<OrderController> logger;
        [BindProperty]
        public OrderViewModel OrderViewModel { get; set; }

        public OrderController(IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<OrderController> logger)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
        }
        public async Task<IActionResult> Detail(int? orderId)
        {
            await GetOrderDetail(orderId);

            return View(OrderViewModel);
        }
        public async Task<IActionResult> _Detail(int? orderId)
        {
            await GetOrderDetail(orderId);

            return PartialView(OrderViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateOrderDetail()
        {
            if (ModelState.IsValid)
            {
                var orderHeader = await unitOfWork.OrderHeaders.Get(o => o.Id == OrderViewModel.OrderHeader.Id);

                orderHeader.Name = OrderViewModel.OrderHeader.Name;
                orderHeader.PhoneNumber = OrderViewModel.OrderHeader.PhoneNumber;
                orderHeader.StreetAddress = OrderViewModel.OrderHeader.StreetAddress;
                orderHeader.City = OrderViewModel.OrderHeader.City;
                orderHeader.Email = OrderViewModel.OrderHeader.Email;
                orderHeader.State = OrderViewModel.OrderHeader.State;
                orderHeader.PostalCode = OrderViewModel.OrderHeader.PostalCode;

                if (OrderViewModel.OrderHeader.Carrier != null)
                {
                    orderHeader.Carrier = OrderViewModel.OrderHeader.Carrier;
                }

                if (OrderViewModel.OrderHeader.TrackingNumber != null)
                {
                    orderHeader.TrackingNumber = OrderViewModel.OrderHeader.TrackingNumber;
                }

                unitOfWork.OrderHeaders.Update(orderHeader);

                await unitOfWork.Save();
                TempData["success"] = $"Order details updated successfully.";
                return RedirectToAction("Detail", "Order", new { orderId = OrderViewModel.OrderHeader.Id });
            }
            else
            {
                TempData["error"] = $"Order details failed to update.";
                return RedirectToAction("Detail", "Order", new { orderId = OrderViewModel.OrderHeader.Id });
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StartProcessing()
        {
            await unitOfWork.OrderHeaders.UpdateStatus(OrderViewModel.OrderHeader.Id, SD.OrderStatus.OrderInProcess);
            await unitOfWork.Save();

            TempData["success"] = $"Order Status updated successfully.";
            return RedirectToAction("Detail", "Order", new { orderId = OrderViewModel.OrderHeader.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ShipOrder()
        {
            var orderHeader = await unitOfWork.OrderHeaders.Get(o => o.Id == OrderViewModel.OrderHeader.Id);
            orderHeader.ShippingDate = DateTime.Now;
            orderHeader.TrackingNumber = OrderViewModel.OrderHeader.TrackingNumber;
            orderHeader.Carrier = OrderViewModel.OrderHeader.Carrier;
            orderHeader.OrderStatus = SD.OrderStatus.OrderShipped;
            if (orderHeader.PaymentStatus == SD.PaymentStatus.PaymentDelayed)
            {
                orderHeader.PaymentDueDate = DateTime.Now.AddDays(30);
            }
            unitOfWork.OrderHeaders.Update(orderHeader);
            await unitOfWork.Save();

            TempData["success"] = $"Order Shipped successfully.";
            return RedirectToAction("Detail", "Order", new { orderId = OrderViewModel.OrderHeader.Id });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelOrder()
        {
            var orderHeader = await unitOfWork.OrderHeaders.Get(u => u.Id == OrderViewModel.OrderHeader.Id);

            if (orderHeader.PaymentStatus == SD.PaymentStatus.PaymentApproved)
            {
                var options = new RefundCreateOptions
                {
                    Reason = RefundReasons.RequestedByCustomer,
                    PaymentIntent = orderHeader.PaymentIntentId
                };

                var service = new RefundService();
                Refund refund = service.Create(options);

                await unitOfWork.OrderHeaders.UpdateStatus(orderHeader.Id, SD.OrderStatus.OrderCancelled, SD.PaymentStatus.Refunded);
            }
            else
            {
                await unitOfWork.OrderHeaders.UpdateStatus(orderHeader.Id, SD.OrderStatus.OrderCancelled);
            }

            await unitOfWork.Save();

            TempData["success"] = $"Order Cancelled successfully.";
            return RedirectToAction("Detail", "Order", new { orderId = OrderViewModel.OrderHeader.Id });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Pay_Now()
        {
            OrderViewModel.OrderHeader = mapper.Map<OrderHeaderDto>(
                await unitOfWork.OrderHeaders.Get(
                    u => u.Id == OrderViewModel.OrderHeader.Id,
                    new List<string> { "ApplicationUser" }));

            OrderViewModel.OrderDetails = mapper.Map<IList<OrderDetailDto>>(
                await unitOfWork.OrderDetails.GetAll(
                    u => u.OrderId == OrderViewModel.OrderHeader.Id,
                    null,
                    new List<string> { "Product" })
            );

            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.Claims.FirstOrDefault();
            
            if (claim.Value != OrderViewModel.OrderHeader.ApplicationUserId && !User.IsInRole(SD.Role_User_Admin))
            {
                TempData["error"] = $"You are not authorized to make payment for this order.";
                return RedirectToAction(nameof(Detail), new { orderId = OrderViewModel.OrderHeader.Id });
            }

            //stripe settings 
            var domain = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}/";

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                  "card",
                },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = domain + $"admin/order/PaymentConfirmation?orderId={OrderViewModel.OrderHeader.Id}",
                CancelUrl = domain + $"cu/order/details?orderId={OrderViewModel.OrderHeader.Id}",
            };

            foreach (var item in OrderViewModel.OrderDetails)
            {

                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Price * 100),//20.00 -> 2000
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Title
                        },

                    },
                    Quantity = item.Count,
                };
                options.LineItems.Add(sessionLineItem);

            }

            var service = new SessionService();
            Session session = service.Create(options);
            await unitOfWork.OrderHeaders.UpdatePaymentDetail(OrderViewModel.OrderHeader.Id, session.Id, session.PaymentIntentId);

            await unitOfWork.Save();
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }
        public async Task<IActionResult> PaymentConfirmation(int orderId)
        {
            OrderHeader orderHeader = await unitOfWork.OrderHeaders.Get(u => u.Id == orderId);
            if (orderHeader != null && orderHeader.PaymentStatus == SD.PaymentStatus.PaymentDelayed)
            {
                var service = new SessionService();
                Session session = service.Get(orderHeader.SessionId);
                //check the stripe status
                if (session.PaymentStatus.ToLower() == "paid")
                {
                    await unitOfWork.OrderHeaders.UpdateStatus(orderId, orderHeader.OrderStatus, SD.PaymentStatus.PaymentApproved);
                    await unitOfWork.Save();
                }
            }
            return View(orderId);
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
                        dataFilter = (o) => claims.Value == o.ApplicationUserId;
                        break;
                }
            }

            var orders = await unitOfWork.OrderHeaders.GetPagedList(mapper.Map<PagedRequest>(pageFilter), dataFilter, null, new List<string> { "ApplicationUser" });

            IList<OrderHeaderDto> orderDtos = mapper.Map<IList<OrderHeaderDto>>(orders);
            return Json(new { Data = orderDtos });

        }
        private async Task GetOrderDetail(int? orderId)
        {
            OrderViewModel = new();

            OrderViewModel.OrderHeader = mapper.Map<OrderHeaderDto>(
                await unitOfWork.OrderHeaders.Get(o => o.Id == orderId.GetValueOrDefault(), new List<string> { "ApplicationUser" }));

            OrderViewModel.OrderDetails = mapper.Map<IEnumerable<OrderDetailDto>>(
              await unitOfWork.OrderDetails.GetAll(o => o.OrderId == orderId.GetValueOrDefault(), null, new List<string> { "Product" }));
        }
    }
}
