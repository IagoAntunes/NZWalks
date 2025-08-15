using NZWalks.API.Models.Dto;

namespace NZWalks.API.Repositories.Interfaces
{
    public interface IAuthRepository
    {

        Task<bool> Register(RegisterRequestDto request);
        Task<string?> Login(LoginRequestDto request);

    }
}
