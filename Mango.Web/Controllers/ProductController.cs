using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> ProductIndex()
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

        public async Task<IActionResult> ProductCreate()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ProductCreate(ProductDto ProductDto)
        {
            if (ModelState.IsValid)
            {
                ResponeDto? respone = await _productService.CreateProductAsync(ProductDto);
                if (respone != null && respone.IsSuccess)
                {
                    TempData["success"] = "Product created successfully";

                    return RedirectToAction(nameof(ProductIndex));
                }
                else
                {
                    TempData["error"] = respone?.Message;
                }
            }
            return View(ProductDto);
        }

        public async Task<IActionResult> ProductDelete(int ProductId)
        {
            ResponeDto? respone = await _productService.GetProductByIdAsync(ProductId);
            if (respone != null && respone.IsSuccess)
            {
                ProductDto? model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(respone.Result));
                return View(model);

            }
            else
            {
                TempData["error"] = respone?.Message;
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> ProductDelete(ProductDto ProductDto)
        {
            ResponeDto? respone = await _productService.DeleteProductAsync(ProductDto.ProductId);
            if (respone != null && respone.IsSuccess)
            {
                TempData["success"] = "Product deleted successfully";

                return RedirectToAction(nameof(ProductIndex));


            }
            else
            {
                TempData["error"] = respone?.Message;
            }
            return View(ProductDto);
        }


        public async Task<IActionResult> ProductEdit(int ProductId)
        {
            if (ModelState.IsValid)
            {
                ResponeDto? respone = await _productService.GetProductByIdAsync(ProductId);
                if (respone != null && respone.IsSuccess)
                {
                    ProductDto? model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(respone.Result));
                    return View(model);

                }
                else
                {
                    TempData["error"] = respone?.Message;
                }
            }

              
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> ProductEdit(ProductDto ProductDto)
        {
            ResponeDto? respone = await _productService.UpdateProductAsync(ProductDto);
            if (respone != null && respone.IsSuccess)
            {
                TempData["success"] = "Product deleted successfully";

                return RedirectToAction(nameof(ProductIndex));


            }
            else
            {
                TempData["error"] = respone?.Message;
            }
            return View(ProductDto);
        }
    }
}
