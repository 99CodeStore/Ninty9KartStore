using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NintyNineKartStore.Core.Entities;
using NintyNineKartStore.Core.Interfaces;
using NintyNineKartStore.Service.Models;
using NintyNineKartStore.Utility;
using NintyNineKartStore.Web.Areas.Customer.ViewModels;
using Stripe.Checkout;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NintyNineKartStore.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<ShoppingCartController> logger;
        private readonly IEmailSender emailSender;

        [BindProperty]
        public ShoppingCartViewModel shoppingCart { get; set; }
        public ShoppingCartController(IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<ShoppingCartController> logger,
            IEmailSender emailSender
            )
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
            this.emailSender = emailSender;
        }
        public async Task<IActionResult> Index()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.Claims.FirstOrDefault();

            var cartItems = await unitOfWork.ShoppingCarts.GetAll(
                        u => u.ApplicationUserId == claim.Value,
                        null,
                        new List<string> { "Product" }
                        );

            var cartItemsDto = mapper.Map<IList<ShoppingCartDto>>(cartItems);

            shoppingCart = new ShoppingCartViewModel()
            {
                ShoppingCartItems = cartItemsDto,
                OrderHeader = new() { OrderTotal = cartItemsDto.Sum(i => i.Count * i.Price) }
            };

            return View(shoppingCart);
        }

        public async Task<IActionResult> Summary()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.Claims.FirstOrDefault();

            var cartItems = await unitOfWork.ShoppingCarts.GetAll(
                        u => u.ApplicationUserId == claim.Value,
                        null,
                        new List<string> { "Product" }
                        );

            var applicationUser = await unitOfWork.ApplicationUsers.Get(u => u.Id == claim.Value);

            var cartItemsDto = mapper.Map<IList<ShoppingCartDto>>(cartItems);

            shoppingCart = new ShoppingCartViewModel()
            {
                ShoppingCartItems = cartItemsDto
            };

            shoppingCart = new ShoppingCartViewModel()
            {
                ShoppingCartItems = cartItemsDto,
                OrderHeader = new()
                {
                    OrderTotal = cartItemsDto.Sum(i => i.Count * i.Price),
                    Name = applicationUser.Name,
                    PhoneNumber = applicationUser.PhoneNumber,
                    Email = applicationUser.Email,
                    StreetAddress = applicationUser.StreetAddress,
                    City = applicationUser.City,
                    State = applicationUser.State,
                    PostalCode = applicationUser.PostalCode
                }
            };

            return View(shoppingCart);
        }

        [HttpPost]
        [ActionName("PlaceOrder")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceOrder()
        {

            if (ModelState.IsValid)
            {
                var claimIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimIdentity.Claims.FirstOrDefault();

                shoppingCart.ShoppingCartItems = mapper.Map<IList<ShoppingCartDto>>(await unitOfWork.ShoppingCarts.GetAll(
                           u => u.ApplicationUserId == claim.Value,
                           null,
                           new List<string> { "Product" }
                           ));
                if (shoppingCart.ShoppingCartItems == null || shoppingCart.ShoppingCartItems.Count == 0)
                {
                    ModelState.AddModelError("", $"There is no Items on the cart. So Order can not be placed.");
                    TempData["error"] = $"There is no Items on the cart. So Order can not be placed.";
                    return RedirectToAction(nameof(Summary));
                }

                shoppingCart.OrderHeader.OrderTotal = shoppingCart.ShoppingCartItems.Sum(i => i.Count * i.Price);


                shoppingCart.OrderHeader.OrderDate = System.DateTime.Now;
                shoppingCart.OrderHeader.ApplicationUserId = claim.Value;

                var appUser = await unitOfWork.ApplicationUsers.Get(u => u.Id == claim.Value);
                if (appUser.CompanyId.GetValueOrDefault() > 0)
                {
                    shoppingCart.OrderHeader.PaymentStatus = SD.PaymentStatus.PaymentDelayed;
                    shoppingCart.OrderHeader.OrderStatus = SD.OrderStatus.OrderApproved;
                }
                else
                {
                    shoppingCart.OrderHeader.PaymentStatus = SD.PaymentStatus.PaymentPending;
                    shoppingCart.OrderHeader.OrderStatus = SD.OrderStatus.OrderPending;
                }
                var orderHeaderDb = mapper.Map<OrderHeader>(shoppingCart.OrderHeader);

                await unitOfWork.OrderHeaders.Insert(orderHeaderDb);
                await unitOfWork.Save();

                shoppingCart.OrderHeader.Id = orderHeaderDb.Id;

                foreach (var item in shoppingCart.ShoppingCartItems)
                {
                    OrderDetail orderDetail = new()
                    {
                        ProductId = item.ProductId,
                        OrderId = shoppingCart.OrderHeader.Id,
                        Price = item.Price,
                        Count = item.Count,
                    };

                    await unitOfWork.OrderDetails.Insert(orderDetail);

                    await unitOfWork.Save();
                }

                if (appUser.CompanyId.GetValueOrDefault() == 0)
                {

                    #region Order Payment  
                    var domain = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}/";
                    var options = new SessionCreateOptions
                    {
                        PaymentMethodTypes = new List<string> { "card" },

                        LineItems = new List<SessionLineItemOptions>(),
                        Mode = "payment",
                        SuccessUrl = domain + $"customer/ShoppingCart/OrderConfirmation?id={shoppingCart.OrderHeader.Id}",
                        CancelUrl = domain + $"customer/ShoppingCart/Index?id={shoppingCart.OrderHeader.Id}"
                    };

                    foreach (var item in shoppingCart.ShoppingCartItems)
                    {
                        var sessionLineItem = new SessionLineItemOptions
                        {
                            PriceData = new SessionLineItemPriceDataOptions
                            {
                                UnitAmount = (long)item.Price * 100,//20.00=>2000
                                Currency = "usd",
                                ProductData = new SessionLineItemPriceDataProductDataOptions
                                {
                                    Name = item.ProductDto.Title,
                                },

                            },
                            Quantity = item.Count,
                        };

                        options.LineItems.Add(sessionLineItem);
                    }

                    var service = new SessionService();
                    Session session = service.Create(options);

                    await unitOfWork.OrderHeaders.UpdatePaymentDetail(shoppingCart.OrderHeader.Id, session.Id, session.PaymentIntentId);

                    await unitOfWork.Save();

                    Response.Headers.Add("Location", session.Url);
                    return new StatusCodeResult(303);
                    #endregion

                }
                else
                {
                    return RedirectToAction(nameof(OrderConfirmation), new { id = shoppingCart.OrderHeader.Id });
                }
            }
            else
            {
                TempData["error"] = $"There is no Items on the cart. So Order can not be placed.";
                return RedirectToAction(nameof(Summary));
            }
        }

        public async Task<IActionResult> OrderConfirmation(int id)
        {
            var orderHeader = await unitOfWork.OrderHeaders.Get(o => o.Id == id,
                new() { "ApplicationUser" });

            if (orderHeader.PaymentStatus != SD.PaymentStatus.PaymentDelayed)
            {
                var service = new SessionService();

                Session session = await service.GetAsync(orderHeader.SessionId);

                if (session.PaymentStatus.ToLower() == "paid")
                {
                    await unitOfWork.OrderHeaders.UpdateStatus(orderHeader.Id, SD.OrderStatus.OrderApproved, SD.PaymentStatus.PaymentApproved);
                    await unitOfWork.Save();
                }
            }
            // Sending Order Confirmation in Email
            await emailSender.SendEmailAsync(
                orderHeader.Email,
                $"Order Confirmation",
                $"Hi,{orderHeader.Name}<br /> Your Order has been successfully placed.");

            var shoppingCarts = await unitOfWork.ShoppingCarts.GetAll(sc => sc.ApplicationUserId == orderHeader.ApplicationUserId);

            unitOfWork.ShoppingCarts.DeleteRange(mapper.Map<IList<ShoppingCart>>(shoppingCarts));

            await unitOfWork.Save();

            return View(id);
        }

        public async Task<IActionResult> Plus(int cartId)
        {
            var cart = await unitOfWork.ShoppingCarts.Get(x => x.Id == cartId);

            unitOfWork.ShoppingCarts.IncreaseQuantity(cart, 1);

            var productDto = mapper.Map<ProductDto>(await unitOfWork.Products.Get(p => p.Id == cart.ProductId));

            cart.Price = GetPriceBasedOnQuantity(cart.Count, productDto.Price, productDto.Price50, productDto.Price100);

            await unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Minus(int cartId)
        {
            var cart = await unitOfWork.ShoppingCarts.Get(x => x.Id == cartId);

            if (cart.Count > 1)
            {
                unitOfWork.ShoppingCarts.DecreaseQuantity(cart, 1);

                var productDto = mapper.Map<ProductDto>(await unitOfWork.Products.Get(p => p.Id == cart.ProductId));

                cart.Price = GetPriceBasedOnQuantity(cart.Count, productDto.Price, productDto.Price50, productDto.Price100);
            }
            else
            {
                await unitOfWork.ShoppingCarts.Delete(cartId);
            }

            await unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Remove(int cartId)
        {

            await unitOfWork.ShoppingCarts.Delete(cartId);

            await unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost()]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> AddToCart([FromForm] ShoppingCartDto cartDto)
        {
            if (!ModelState.IsValid)
            {
                var claimIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimIdentity.Claims.FirstOrDefault();
                cartDto.ApplicationUserId = claim.Value;

                var cartListDb = await unitOfWork.ShoppingCarts.GetAll(
                    x => x.ProductId == cartDto.ProductId && x.ApplicationUserId == claim.Value);

                var productDto = mapper.Map<ProductDto>(await unitOfWork.Products.Get(p => p.Id == cartDto.ProductId));

                if (cartListDb.Count > 0)
                {
                    unitOfWork.ShoppingCarts.IncreaseQuantity(cartListDb[0], cartDto.Count);
                    cartListDb[0].Price = GetPriceBasedOnQuantity(cartListDb[0].Count, productDto.Price, productDto.Price50, productDto.Price100);
                }
                else
                {

                    cartDto.Price = GetPriceBasedOnQuantity(cartDto.Count, productDto.Price, productDto.Price50, productDto.Price100);

                    var cart = mapper.Map<ShoppingCart>(cartDto);

                    await unitOfWork.ShoppingCarts.Insert(cart);

                }

                await unitOfWork.Save();

                HttpContext.Session.SetInt32(SD.ShoppinghCart.ShoppingCartItemsCount,
                        (await unitOfWork.ShoppingCarts.GetAll(u => u.ApplicationUserId == claim.Value)).Count
                        );

            }

            return RedirectToAction(nameof(Index));
        }

        private double GetPriceBasedOnQuantity(double quantity, double price, double price50, double price100)
        {
            if (quantity <= 50)
            {
                return price;
            }
            else
            {
                if (quantity <= 100)
                {
                    return price50;
                }
                return price100;
            }
        }

    }
}
