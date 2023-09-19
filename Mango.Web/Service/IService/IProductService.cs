using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface IProductService
    {
        Task<ResponeDto?> GetAllProductsAsync();
        Task<ResponeDto?> GetProductByIdAsync(int id);
        Task<ResponeDto?> CreateProductAsync(ProductDto ProductDto);
        Task<ResponeDto?> UpdateProductAsync(ProductDto ProductDto);
        Task<ResponeDto?> DeleteProductAsync(int id);
    }
}
