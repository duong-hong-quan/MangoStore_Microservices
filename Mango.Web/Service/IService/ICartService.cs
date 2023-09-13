using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface ICartService
    {
        Task<ResponeDto?> GetCartByUserIdAsync(string userId);
        Task<ResponeDto?> UpsertAsync(CartDto cartDto);
        Task<ResponeDto?> RemoveFromCartAsync(int cartDetailsId);
        Task<ResponeDto?> ApplyCouponAsync(CartDto cartDto);
       
    }
}
