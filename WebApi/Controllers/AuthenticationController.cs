using System.Threading.Tasks;
using Entities.DTO_s;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/authentication")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        // Kullanıcı kaydı için metod
        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistrationDto)
        {
            var result = await _authenticationService.RegisterUser(userForRegistrationDto);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }

            return StatusCode(201);  // Başarıyla oluşturuldu
        }

        // Kullanıcı girişi ve token alma metod
        [HttpPost("login")]
        public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto userForAuthenticationDto)
        {
            if (!await _authenticationService.ValidateUser(userForAuthenticationDto))
                return Unauthorized();

            var token = await _authenticationService.CreateToken();

            return Ok(new
            {
                Token = token,
                Message = "Başarıyla giriş yaptınız."
            });
        }
    }
}
