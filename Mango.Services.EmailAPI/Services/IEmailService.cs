using Mango.Services.EmailAPI.Models.Dto;

namespace Mango.Services.EmailAPI.Services
{
    public interface IEmailService
    {
        public Task EmailCartAndLog(CartDto cart);
    }
}
