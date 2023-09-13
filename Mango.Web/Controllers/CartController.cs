using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace Mango.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }
        [Authorize]
        public async Task<IActionResult> CartIndex()
        {

            return View(await LoadCartDtoBaseOnLoggedInUser());
        }

        public async Task<IActionResult> Remove(int cartDetailsId)
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            ResponeDto respone = await _cartService.RemoveFromCartAsync(cartDetailsId);
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
