using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;

namespace Mango.Web.Service
{
    public class ProductService:IProductService
    {
        private readonly IBaseService _baseService;
        public ProductService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponeDto?> CreateProductAsync(ProductDto ProductDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Utility.SD.ApiType.POST,
                Data = ProductDto,
                Url = SD.ProductAPIBase + "/api/product",
                ContentType = SD.ContentType.MultipartFormData
            });
        }

        public async Task<ResponeDto?> DeleteProductAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Utility.SD.ApiType.DELETE,
                Url = SD.ProductAPIBase + "/api/product/" + id
            });
        }

        public async Task<ResponeDto?> GetAllProductsAsync()
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Utility.SD.ApiType.GET,
                Url = SD.ProductAPIBase + "/api/product"
            });
        }

     

        public async Task<ResponeDto?> GetProductByIdAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Utility.SD.ApiType.GET,
                Url = SD.ProductAPIBase + "/api/product/" + id
            });
        }

        public async Task<ResponeDto?> UpdateProductAsync(ProductDto ProductDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Utility.SD.ApiType.PUT,
                Data = ProductDto,
                Url = SD.ProductAPIBase + "/api/product",
                ContentType = SD.ContentType.MultipartFormData

            });
        }
    }
}
