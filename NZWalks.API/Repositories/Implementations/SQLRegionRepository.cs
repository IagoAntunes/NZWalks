using Azure.Core;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Repositories.Interfaces;

namespace NZWalks.API.Repositories.Implementations
{
    public class SQLRegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext dbContext;
        public SQLRegionRepository(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Region>> GetAllAsync()
        {
            var regions = await dbContext.Regions.ToListAsync();
            return regions;
        }

        public async Task<Region?> GetById(Guid id)
        {
            var regionDomain = await dbContext.Regions.FindAsync(id);
            return regionDomain;
        }

        public async Task<Region?> CreateAsync(Region region)
        {
            await dbContext.Regions.AddAsync(region);
            await dbContext.SaveChangesAsync();

            return region;
        }

        public async Task<Region?> UpdateAsync(Guid id, Region region)
        {
            //Check if region exists
            var existingRegion = await dbContext.Regions.FindAsync(id);
            if (region == null)
            {
                return null;
            }

            // Map DTO to Domain model
            existingRegion.Name = region.Name;
            existingRegion.Code = region.Code;
            existingRegion.RegionImageUrl = region.RegionImageUrl;

            dbContext.Regions.Update(existingRegion);
            await dbContext.SaveChangesAsync();

            return existingRegion;
        }

        public async Task<Region?> DeleteAsync(Guid id)
        {
            var region = await dbContext.Regions.FindAsync(id);

            if (region == null)
            {
                return null;
            }
            dbContext.Regions.Remove(region);
            await dbContext.SaveChangesAsync();

            return region;
        }
    }
}
