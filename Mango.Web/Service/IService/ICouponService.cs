using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface ICouponService
    {
        Task<ResponeDto?> GetCouponAsync(string couponCode);
        Task<ResponeDto?> GetAllCouponsAsync();
        Task<ResponeDto?> GetCouponByIdAsync(int id);
        Task<ResponeDto?> CreateCouponAsync(CouponDto couponDto);
        Task<ResponeDto?> UpdateCouponAsync(CouponDto couponDto);
        Task<ResponeDto?> DeleteCouponAsync(int id);
    }
}
