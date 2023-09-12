using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Mango.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        public HomeController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            List<ProductDto>? list = new();
            ResponeDto? respone = await _productService.GetAllProductsAsync();
            if (respone != null && respone.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(respone.Result));
            }
            else
            {
                TempData["error"] = respone?.Message;
            }
            return View(list);
        }
        [Authorize]
        public async Task<IActionResult> ProductDetails(int productId)
        {
            ProductDto model = new();
            ResponeDto? respone = await _productService.GetProductByIdAsync(productId);
            if (respone != null && respone.IsSuccess)
            {
                model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(respone.Result));
            }
            else
            {
                TempData["error"] = respone?.Message;
            }
            return View(model);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}