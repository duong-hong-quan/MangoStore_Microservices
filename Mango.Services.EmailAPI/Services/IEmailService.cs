using Mango.Services.EmailAPI.Message;
using Mango.Services.EmailAPI.Models.Dto;

namespace Mango.Services.EmailAPI.Services
{
    public interface IEmailService
    {
        public Task EmailCartAndLog(CartDto cart);
        public Task RegisterUserEmailAndLog(string email);
        public Task LogOrderPlaced(RewardsMessage rewardsDto);
    
    }
}
