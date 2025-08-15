using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Dto;
using NZWalks.API.Repositories.Interfaces;
using NZWalks.Domain.Dtos;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository authRepository;
        private readonly IMapper mapper;

        public AuthController(
            IAuthRepository authRepository,
            IMapper mapper
        )
        {
            this.authRepository = authRepository;
            this.mapper = mapper;
        }


        // POST: /api/Auth/Register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            var registerDto = mapper.Map<RegisterDto>(request);
            var result = await authRepository.Register(registerDto);

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
            var loginDto = mapper.Map<LoginDto>(request);
            var result = await authRepository.Login(loginDto);
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
