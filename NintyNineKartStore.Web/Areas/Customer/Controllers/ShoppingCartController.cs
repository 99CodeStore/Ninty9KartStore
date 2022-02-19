using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NintyNineKartStore.Core.Entities;
using NintyNineKartStore.Core.Interfaces;
using NintyNineKartStore.Service.Models;
using NintyNineKartStore.Utility;
using NintyNineKartStore.Web.Areas.Customer.ViewModels;
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

        public ShoppingCartViewModel shoppingCart { get; set; }
        public ShoppingCartController(IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<ShoppingCartController> logger
            )
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
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

            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.Claims.FirstOrDefault();

            var cartItems = mapper.Map<IList<ShoppingCartDto>>(await unitOfWork.ShoppingCarts.GetAll(
                       u => u.ApplicationUserId == claim.Value,
                       null,
                       new List<string> { "Product" }
                       ));

            var applicationUser = await unitOfWork.ApplicationUsers.Get(u => u.Id == claim.Value);

            shoppingCart = new ShoppingCartViewModel()
            {
                ShoppingCartItems = cartItems,
                OrderHeader = new()
                {
                    OrderTotal = cartItems.Sum(i => i.Count * i.Price),
                    Name = applicationUser.Name,
                    PhoneNumber = applicationUser.PhoneNumber,
                    StreetAddress = applicationUser.StreetAddress,
                    City = applicationUser.City,
                    State = applicationUser.State,
                    PostalCode = applicationUser.PostalCode,
                    PaymentStatus = SD.PaymentStatus.PaymentPending,
                    OrderStatus = SD.OrderStatus.OrderPending,
                    OrderDate = System.DateTime.Now,
                    ApplicationUserId = claim.Value
                }
            };

            await unitOfWork.OrderHeaders.Insert(mapper.Map<OrderHeader>(shoppingCart.OrderHeader));

            await unitOfWork.Save();

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
            }

            await unitOfWork.Save();

            unitOfWork.ShoppingCarts.DeleteRange(mapper.Map<IList<ShoppingCart>>(shoppingCart.ShoppingCartItems));
            
            await unitOfWork.Save();

            return RedirectToAction(nameof(OrderConfirmation));
        }

        public async Task<IActionResult> OrderConfirmation(int id)
        {
            return View();
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

            //var count = unitOfWork.ShoppingCarts.GetAll(u => u.ApplicationUserId == cart.ApplicationUserId).ToList().Count;
            //HttpContext.Session.SetInt32(SD.SessionCart, count);
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
