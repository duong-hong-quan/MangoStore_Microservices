using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;

namespace Mango.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public IActionResult OrderIndex()
        {
            return View();
        }
        public async Task<IActionResult> OrderDetail(int orderId)
        {
            OrderHeaderDto orderHeaderDto = new OrderHeaderDto();
            string userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            var respone = await _orderService.GetOrder(orderId);
            if (respone != null && respone.IsSuccess)
            {
                orderHeaderDto = JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(respone.Result));
            }
            if (!User.IsInRole(SD.RoleAdmin) && userId != orderHeaderDto.UserId)
            {
                return NotFound();
            }
            return View(orderHeaderDto);
        }
        [HttpGet]
        public IActionResult GetAll(string status)
        {
            IEnumerable<OrderHeaderDto> list;
            string userId = "";
            if (!User.IsInRole(SD.RoleAdmin))
            {
                userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            }
            ResponeDto respone = _orderService.GetAllOrder(userId).GetAwaiter().GetResult();
            if (respone != null && respone.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<OrderHeaderDto>>(Convert.ToString(respone.Result));
                switch (status)
                {
                    case "approved":
                        list = list.Where(u => u.Status == SD.Status_Approved);
                        break;
                    case "readyforpickup":
                        list = list.Where(u => u.Status == SD.Status_ReadyForPickup);
                        break;
                    case "cancelled":
                        list = list.Where(u => u.Status == SD.Stauts_Cancelled);
                        break;
                    default:
                        break;

                }
            }
            else
            {
                list = new List<OrderHeaderDto>();
            }
            return Json(new { data = list });
        }

        [HttpPost("OrderReadyForPickup")]
        public async Task<IActionResult> OrderReadyForPickup(int orderId)
        {
            var respone = await _orderService.UpdateOrderStatus(orderId, SD.Status_ReadyForPickup);
            if (respone != null && respone.IsSuccess)
            {
                TempData["success"] = "Status Updated Successfully";
                return RedirectToAction(nameof(OrderDetail), new { orderId = orderId });

            }
            return View();
        }


        [HttpPost("CompleteOrder")]
        public async Task<IActionResult> CompleteOrder(int orderId)
        {
            var respone = await _orderService.UpdateOrderStatus(orderId, SD.Status_Completed);
            if (respone != null && respone.IsSuccess)
            {
                TempData["success"] = "Status Updated Successfully";
                return RedirectToAction(nameof(OrderDetail), new { orderId = orderId });

            }
            return View();
        }

        [HttpPost("CancelOrder")]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            var respone = await _orderService.UpdateOrderStatus(orderId, SD.Stauts_Cancelled);
            if (respone != null && respone.IsSuccess)
            {
                TempData["success"] = "Status Updated Successfully";
                return RedirectToAction(nameof(OrderDetail), new { orderId = orderId });

            }
            return View();
        }

    }
}
