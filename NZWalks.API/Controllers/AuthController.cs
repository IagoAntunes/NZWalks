using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Dto;
using NZWalks.API.Repositories.Interfaces;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository authRepository;

        public AuthController(
            IAuthRepository authRepository
        )
        {
            this.authRepository = authRepository;
        }


        // POST: /api/Auth/Register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            var result = await authRepository.Register(request);

            if(result == true)
            {
                return Ok("Registration successful");
            }

            return BadRequest("Something went wrong");
        }

        // POST: api/Auth/Login
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var result = await authRepository.Login(request);
            if(result != null)
            {
                var response = new LoginResponseDto
                {
                    JwtToken = result,
                };
                return Ok(response);
            }

            return BadRequest("Invalid username or password");

        }

    }
}
