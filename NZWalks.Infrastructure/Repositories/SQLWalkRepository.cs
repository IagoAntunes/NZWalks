using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.Dto;
using NZWalks.API.Repositories.Interfaces;
using NZWalks.Core.Pagination;
using NZWalks.Domain.Dtos;

namespace NZWalks.API.Repositories.Implementations
{
    public class SQLWalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext dbContext;

        public SQLWalkRepository(
            NZWalksDbContext dbContext    
        )
        {
            this.dbContext = dbContext;
        }

        public async Task<Walk> CreateAsync(Walk walk)
        {
            await dbContext.Walks.AddAsync(walk);
            await dbContext.SaveChangesAsync();
            return walk;
        }


        public async Task<PagedResult<Walk>> GetAllAsync(GetAllWalksFilter filter)
        {
            var walksQueryable = dbContext.Walks
                .Include(x => x.Difficulty)
                .Include(x => x.Region)
                .AsQueryable();

            //Filtering
            if (string.IsNullOrWhiteSpace(filter.FilterOn) == false && string.IsNullOrWhiteSpace(filter.FilterQuery) == false)
            {
                walksQueryable = filter.FilterOn.ToLower() switch
                {
                    "name" => walksQueryable.Where(x => x.Name.Contains(filter.FilterQuery)),
                    "region" => walksQueryable.Where(x => x.Region.Name.Contains(filter.FilterQuery)),
                    _ => walksQueryable
                };
            }

            //Sorting
            if(string.IsNullOrWhiteSpace(filter.SortBy) == false)
            {
                walksQueryable = filter.SortBy.ToLower() switch
                {
                    "name" => filter.IsAscending
                                ? walksQueryable.OrderBy(x => x.Name)
                                : walksQueryable.OrderByDescending(x => x.Name),
                    "length" => filter.IsAscending
                                ? walksQueryable.OrderBy(x => x.LengthInKm)
                                : walksQueryable.OrderByDescending(x => x.LengthInKm),
                    _ => walksQueryable
                };
            }

            //Pagination
            var totalCount = await walksQueryable.CountAsync();
            var skipResults = (filter.PageNumber - 1) * filter.PageSize;
            var items = await walksQueryable
                .Skip(skipResults)
                .Take(filter.PageSize)
                .ToListAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)filter.PageSize);

            return new PagedResult<Walk>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalPages = totalPages,
            };

            //var walks = await dbContext.Walks
            //    .Include(x => x.Difficulty)
            //    .Include(Y => Y.Region)
            //    .ToListAsync();
            //var walks = await dbContext.Walks
            //    .Include("Difficulty")
            //    .Include("Region")
            //    .ToListAsync();
        }

        public async Task<Walk?> GetByIdAsync(Guid id)
        {
            var walk = await dbContext.Walks
                .Include("Difficulty")
                .Include("Region")
                .FirstOrDefaultAsync(x => x.Id == id);
                
            return walk;
        }

        public async Task<Walk?> UpdateWalkAsync(Guid id, Walk walk)
        {
            var walkToUpdate = await dbContext.Walks.FindAsync(id);
            if(walkToUpdate == null)
            {
                return null;
            }
            walkToUpdate.Name = walk.Name;
            walkToUpdate.Description = walk.Description;
            walkToUpdate.LengthInKm = walk.LengthInKm;
            walkToUpdate.WalkImageUrl = walk.WalkImageUrl;
            walkToUpdate.Region = walk.Region;
            walkToUpdate.Difficulty = walk.Difficulty;

            await dbContext.SaveChangesAsync();

            return walkToUpdate;
        }

        public async Task<Walk?> DeleteByIdAsync(Guid id)
        {
            var walkToDelete = await dbContext.Walks
                .Include("Difficulty")
                .Include("Region")
                .FirstOrDefaultAsync(x => x.Id == id);

            dbContext.Walks.Remove(walkToDelete);
            await dbContext.SaveChangesAsync();

            return walkToDelete;
        }
    }
}
