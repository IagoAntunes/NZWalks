using Azure.Core;
using Microsoft.AspNetCore.Identity;
using NZWalks.API.Models.Dto;
using NZWalks.API.Repositories.Interfaces;

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

        public async Task<string?> Login(LoginRequestDto request)
        {
            var user = await userManager.FindByEmailAsync(request.Username);

            if (user != null)
            {
                var passwordIsCorrect = await userManager.CheckPasswordAsync(user, request.Password);
                if (passwordIsCorrect)
                {
                    var roles = await userManager.GetRolesAsync(user);
                    if (roles != null)
                    {
                        var jwtToken = tokenRepository.CreateJWTToken(user, roles.ToList());
                        var response = new LoginResponseDto
                        {
                            JwtToken = jwtToken,
                        };
                        return jwtToken;
                    }
                }
            }
            return null;
        }

        public async Task<bool> Register(RegisterRequestDto request)
        {
            var identityUser = new IdentityUser
            {
                UserName = request.Username,
                Email = request.Username,
            };
            var identityResult = await userManager.CreateAsync(identityUser, request.Password);
            if (identityResult.Succeeded)
            {
                if (request.Roles != null && request.Roles.Any())
                {
                    identityResult = await userManager.AddToRolesAsync(identityUser, request.Roles);
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
