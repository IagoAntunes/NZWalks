using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Repositories.Interfaces;

namespace NZWalks.API.Repositories.Implementations
{
    public class LocalImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment environment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly NZWalksDbContext dbContext;

        public LocalImageRepository(
            IWebHostEnvironment environment,
            IHttpContextAccessor httpContextAccessor,
            NZWalksDbContext dbContext
            )
        {
            this.environment = environment;
            this.httpContextAccessor = httpContextAccessor;
            this.dbContext = dbContext;
        }

        public IHttpContextAccessor HttpContextAccessor { get; }
        public NZWalksDbContext DbContext { get; }

        public async Task<Image> Upload(Image image)
        {
            var localFilePath = Path.Combine(environment.ContentRootPath,"Images", $"{image.FileName}{image.FileExtension}");

            //Upload Image to Local Path
            using var stream = new FileStream(localFilePath, FileMode.Create);
            await image.File.CopyToAsync(stream);

            var urlFilePath = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}{httpContextAccessor.HttpContext.Request.PathBase}/Images/{image.FileName}{image.FileExtension}";
            image.FilePath = urlFilePath;

            await dbContext.Images.AddAsync(image);
            await dbContext.SaveChangesAsync();

            return image;
        }
    }
}
