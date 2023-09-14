using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface IOrderService
    {
        Task<ResponeDto?> CreateOrder(CartDto cartDto);
     
    }
}
