using NZWalks.API.Models.Dto;
using NZWalks.Domain.Dtos;

namespace NZWalks.API.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task<bool> Register(RegisterDto registerDto);
        Task<string?> Login(LoginDto loginDto);
    }
}
