using Azure.Core;
using Microsoft.AspNetCore.Identity;
using NZWalks.API.Models.Dto;
using NZWalks.API.Repositories.Interfaces;
using NZWalks.Domain.Dtos;

namespace NZWalks.API.Repositories.Implementations
{
    public class AuthRepository : IAuthRepository
    {

        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;
        public AuthRepository(
            UserManager<IdentityUser> userManager,
            ITokenRepository tokenRepository
            )
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }

        public async Task<string?> Login(LoginDto loginDto)
        {
            var user = await userManager.FindByEmailAsync(loginDto.Username);

            if (user != null)
            {
                var passwordIsCorrect = await userManager.CheckPasswordAsync(user, loginDto.Password);
                if (passwordIsCorrect)
                {
                    var roles = await userManager.GetRolesAsync(user);
                    if (roles != null)
                    {
                        var jwtToken = tokenRepository.CreateJWTToken(user, roles.ToList());
                        //var response = new LoginResponseDto
                        //{
                        //    JwtToken = jwtToken,
                        //};
                        return jwtToken;
                    }
                }
            }
            return null;
        }

        public async Task<bool> Register(RegisterDto registerDto)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerDto.Username,
                Email = registerDto.Username,
            };
            var identityResult = await userManager.CreateAsync(identityUser, registerDto.Password);
            if (identityResult.Succeeded)
            {
                if (registerDto.Roles != null && registerDto.Roles.Any())
                {
                    identityResult = await userManager.AddToRolesAsync(identityUser, registerDto.Roles);
                    if (identityResult.Succeeded)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
