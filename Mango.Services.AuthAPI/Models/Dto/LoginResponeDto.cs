namespace Mango.Services.AuthAPI.Models.Dto
{
    public class LoginResponeDto
    {
        public UserDto User { get; set; }
        public string Token { get; set; }
    }
}
