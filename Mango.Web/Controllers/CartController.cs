using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace Mango.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;
        public CartController(ICartService cartService, IOrderService orderService)
        {
            _cartService = cartService;
            _orderService = orderService;
        }
        [Authorize]
        public async Task<IActionResult> CartIndex()
        {

            return View(await LoadCartDtoBaseOnLoggedInUser());
        }

        [Authorize]
        public async Task<IActionResult> Checkout()
        {

            return View(await LoadCartDtoBaseOnLoggedInUser());
        }

        [Authorize]
        [ActionName("Checkout")]
        [HttpPost]
        public async Task<IActionResult> Checkout(CartDto cartDto)
        {
            CartDto cart = await LoadCartDtoBaseOnLoggedInUser();
            cart.CartHeader.Phone = cartDto.CartHeader.Phone;
            cart.CartHeader.Email = cartDto.CartHeader.Email;
            cart.CartHeader.Name = cartDto.CartHeader.Name;
            var respone = await _orderService.CreateOrder(cart);
            OrderHeaderDto orderHeaderDto = JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(respone.Result));
            if (respone != null && respone.IsSuccess)
            {
                var domain = Request.Scheme + "://" + Request.Host.Value + "/";
                StripeRequestDto stripeRequestDto = new()
                {
                    ApprovedUrl = domain + "cart/Confirmation?orderId=" + orderHeaderDto.OrderHeaderId,
                    CancelUrl = domain + "cart/checkout",
                    OrderHeader = orderHeaderDto,

                };
                var stripeRespone = await _orderService.CreateStripeSession(stripeRequestDto);
                StripeRequestDto stripeResponeResult = JsonConvert.DeserializeObject<StripeRequestDto>(Convert.ToString(stripeRespone.Result));

                Response.Headers.Add("Location", stripeResponeResult.StripeSessionUrl);
                return new StatusCodeResult(303);
            }
            return View();

        }

        public async Task<IActionResult> Confirmation(int orderId)
        {
            ResponeDto? respone = await _orderService.ValidateStripeSession(orderId);
            if (respone != null && respone.IsSuccess)
            {
                OrderHeaderDto orderHeader = JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(respone.Result));
                if(orderHeader.Status == SD.Status_Approved)
                {
                    return View(orderId);

                }

            }
            return View(orderId);
        }


        public async Task<IActionResult> Remove(int cartDetailsId)
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            ResponeDto? respone = await _cartService.RemoveFromCartAsync(cartDetailsId);
            if (respone != null && respone.IsSuccess)
            {
                TempData["Success"] = "Cart updated succesfully";

                return RedirectToAction(nameof(CartIndex));
            }
            return View();
        }


        private async Task<CartDto> LoadCartDtoBaseOnLoggedInUser()
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            ResponeDto respone = await _cartService.GetCartByUserIdAsync(userId);
            if (respone != null && respone.IsSuccess)
            {
                CartDto cartDto = JsonConvert.DeserializeObject<CartDto>(respone.Result.ToString());
                return cartDto;
            }
            return new CartDto();
        }

        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(CartDto cartDto)
        {
            ResponeDto? respone = await _cartService.ApplyCouponAsync(cartDto);
            if (respone != null && respone.IsSuccess)
            {
                TempData["Success"] = "Cart updated succesfully";
                return RedirectToAction(nameof(CartIndex));
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EmailCart(CartDto cartDto)
        {
            CartDto cart = await LoadCartDtoBaseOnLoggedInUser();
            cart.CartHeader.Email = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Email)?.FirstOrDefault()?.Value;

            ResponeDto? respone = await _cartService.EmailCart(cart);
            if (respone != null && respone.IsSuccess)
            {
                TempData["Success"] = "Email will be processed and sent shortly";
                return RedirectToAction(nameof(CartIndex));
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RemoveCoupon(CartDto cartDto)
        {
            cartDto.CartHeader.CouponCode = "";
            ResponeDto? respone = await _cartService.ApplyCouponAsync(cartDto);
            if (respone != null && respone.IsSuccess)
            {
                TempData["Success"] = "Cart updated succesfully";
                return RedirectToAction(nameof(CartIndex));
            }
            return View();
        }
    }
}
