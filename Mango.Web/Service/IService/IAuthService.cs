using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface IAuthService
    {
        Task<ResponeDto?> LoginAsync(LoginRequestDto loginRequestDto);
        Task<ResponeDto?> RegisterAsync(RegistrationRequestDto registrationRequestDto);
        Task<ResponeDto?> AssignRoleAsync(RegistrationRequestDto registrationRequestDto);

    }
}
