using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.Dto;
using NZWalks.API.Repositories.Interfaces;
using NZWalks.Core.Pagination;

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


        public async Task<PagedResult<Walk>> GetAllAsync(GetAllWalksQueryParameters query)
        {
            var walksQueryable = dbContext.Walks
                .Include(x => x.Difficulty)
                .Include(x => x.Region)
                .AsQueryable();

            //Filtering
            if (string.IsNullOrWhiteSpace(query.FilterOn) == false && string.IsNullOrWhiteSpace(query.FilterQuery) == false)
            {
                walksQueryable = query.FilterOn.ToLower() switch
                {
                    "name" => walksQueryable.Where(x => x.Name.Contains(query.FilterQuery)),
                    "region" => walksQueryable.Where(x => x.Region.Name.Contains(query.FilterQuery)),
                    _ => walksQueryable
                };
            }

            //Sorting
            if(string.IsNullOrWhiteSpace(query.SortBy) == false)
            {
                walksQueryable = query.SortBy.ToLower() switch
                {
                    "name" => query.IsAscending
                                ? walksQueryable.OrderBy(x => x.Name)
                                : walksQueryable.OrderByDescending(x => x.Name),
                    "length" => query.IsAscending
                                ? walksQueryable.OrderBy(x => x.LengthInKm)
                                : walksQueryable.OrderByDescending(x => x.LengthInKm),
                    _ => walksQueryable
                };
            }

            //Pagination
            var totalCount = await walksQueryable.CountAsync();
            var skipResults = (query.PageNumber - 1) * query.PageSize;
            var items = await walksQueryable
                .Skip(skipResults)
                .Take(query.PageSize)
                .ToListAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)query.PageSize);

            return new PagedResult<Walk>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
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
