using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface IOrderService
    {
        Task<ResponeDto?> CreateOrder(CartDto cartDto);
        Task<ResponeDto?> CreateStripeSession(StripeRequestDto stripeRequestDto);
        Task<ResponeDto?> ValidateStripeSession(int orderHeaderId);
    }
}
