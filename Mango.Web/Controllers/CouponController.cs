using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Mango.Web.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponService _couponService;
        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }

        public async Task<IActionResult> CouponIndex()
        {
            List<CouponDto>? list = new();
            ResponeDto? respone = await _couponService.GetAllCouponsAsync();
            if (respone != null && respone.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<CouponDto>>(Convert.ToString(respone.Result));
            }
            else
            {
                TempData["error"] = respone?.Message;
            }
            return View(list);
        }

        public async Task<IActionResult> CouponCreate()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CouponCreate(CouponDto couponDto)
        {
            if (ModelState.IsValid)
            {
                ResponeDto? respone = await _couponService.CreateCouponAsync(couponDto);
                if (respone != null && respone.IsSuccess)
                {
                    TempData["success"] = "Coupon created successfully";

                    return RedirectToAction(nameof(CouponIndex));
                }
                else
                {
                    TempData["error"] = respone?.Message;
                }
            }
            return View(couponDto);
        }

        public async Task<IActionResult> CouponDelete(int couponId)
        {
            ResponeDto? respone = await _couponService.GetCouponByIdAsync(couponId);
            if (respone != null && respone.IsSuccess)
            {
                CouponDto? model = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(respone.Result));
            return View(model);

            }
            else
            {
                TempData["error"] = respone?.Message;
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> CouponDelete(CouponDto couponDto)
        {
            ResponeDto? respone = await _couponService.DeleteCouponAsync(couponDto.CounponId);
            if (respone != null && respone.IsSuccess)
            {
                TempData["success"] = "Coupon deleted successfully";

                return RedirectToAction(nameof(CouponIndex));


            }
            else
            {
                TempData["error"] = respone?.Message;
            }
            return View(couponDto);
        }


        public async Task<IActionResult> CouponEdit(int couponId)
        {
            ResponeDto? respone = await _couponService.GetCouponByIdAsync(couponId);
            if (respone != null && respone.IsSuccess)
            {
                CouponDto? model = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(respone.Result));
                return View(model);

            }
            else
            {
                TempData["error"] = respone?.Message;
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> CouponEdit(CouponDto couponDto)
        {
            ResponeDto? respone = await _couponService.UpdateCouponAsync(couponDto);
            if (respone != null && respone.IsSuccess)
            {
                TempData["success"] = "Coupon deleted successfully";

                return RedirectToAction(nameof(CouponIndex));


            }
            else
            {
                TempData["error"] = respone?.Message;
            }
            return View(couponDto);
        }
    }
}
