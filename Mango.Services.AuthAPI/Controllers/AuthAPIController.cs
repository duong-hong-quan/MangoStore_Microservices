using Mango.Services.AuthAPI.Models.Dto;
using Mango.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly IAuthService _authService;
        private ResponeDto _respone;

        public AuthAPIController(IAuthService authService)
        {
            _authService = authService;
            _respone = new ResponeDto();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto model)
        {
            var errorMessage = await _authService.Register(model);
            if(!string.IsNullOrEmpty(errorMessage))
            {
                _respone.IsSuccess = false;
                _respone.Message = errorMessage;
                return BadRequest(_respone);
            }
            
            return Ok(_respone);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto model)
        {
            var loginRespone = await _authService.Login(model);
            if(loginRespone.User == null)
            {
                _respone.IsSuccess = false;
                _respone.Message = "Username or password is incorrect";
                return BadRequest(_respone);
            }
           _respone.Result = loginRespone;  
            return Ok(_respone);
        }
        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole(RegistrationRequestDto model)
        {
            var assignRoleSuccessful = await _authService.AssignRole(model.Email, model.Role);
            if (!assignRoleSuccessful)
            {
                _respone.IsSuccess = false;
                _respone.Message = "Error encountered";
                return BadRequest(_respone);
            }
            return Ok(_respone);
        }

    }
}
