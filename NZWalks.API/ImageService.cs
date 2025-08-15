using NZWalks.API.Models.Domain;
using System;

namespace NZWalks.API
{
    public class ImageService
    {
        private readonly IWebHostEnvironment environment;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ImageService(
            IWebHostEnvironment environment,
            IHttpContextAccessor httpContextAccessor
        )
        {
            this.environment = environment;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> CreateImage(Image image, IFormFile formFile)
        {
            try
            {
                var localFilePath = Path.Combine(environment.ContentRootPath, "Images", $"{image.FileName}{image.FileExtension}");

                using var stream = new FileStream(localFilePath, FileMode.Create);
                await formFile.CopyToAsync(stream);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
