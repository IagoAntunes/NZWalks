using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Repositories.Interfaces;

namespace NZWalks.API.Repositories.Implementations
{
    public class LocalImageRepository : IImageRepository
    {
        private readonly NZWalksDbContext dbContext;

        public LocalImageRepository(
            NZWalksDbContext dbContext
        )
        {
            this.dbContext = dbContext;
        }

        public NZWalksDbContext DbContext { get; }

        public async Task<Image> Upload(Image image)
        {
            await dbContext.Images.AddAsync(image);
            await dbContext.SaveChangesAsync();

            return image;
        }
    }
}
