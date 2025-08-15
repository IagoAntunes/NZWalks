using NZWalks.API.Models.Domain;
using NZWalks.API.Models.Dto;
using NZWalks.Core.Pagination;
using NZWalks.Domain.Dtos;

namespace NZWalks.API.Repositories.Interfaces
{
    public interface IWalkRepository
    {
        Task<Walk> CreateAsync(Walk walk);
        Task<PagedResult<Walk>> GetAllAsync(GetAllWalksFilter filter);
        Task<Walk?> GetByIdAsync(Guid id);
        Task<Walk?> UpdateWalkAsync(Guid id,Walk walk);
        Task<Walk?> DeleteByIdAsync(Guid id);
    }
}
