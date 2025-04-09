using ECommerce.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;

namespace ECommerce.Infrastructure.Repositories.Services
{
    public class ImageManagementService : IImageManagementService
    {
        #region Fields
        private readonly IFileProvider _fileProvider;
        #endregion

        #region Constructors
        public ImageManagementService(IFileProvider fileProvider)
        {
            _fileProvider = fileProvider;
        }
        #endregion

        #region Methods Handling
        public async Task<List<string>> UploadImagesAsync(IFormFileCollection files, string src)
        {
            List<string> saveImagesSrc = new List<string>();
            var imagesDirectory = Path.Combine("wwwroot", "Images", src.Replace(" ", "")).Replace("\\", "/");
            if (!Directory.Exists(imagesDirectory))
                Directory.CreateDirectory(imagesDirectory);

            foreach (var file in files)
            {
                if (file.Length <= 0)
                    continue;
                var fileName = $"{Guid.NewGuid():N}{Path.GetExtension(file.FileName)}";
                var filePath = Path.Combine(imagesDirectory, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                saveImagesSrc.Add(Path.Combine("Images", src, fileName).Replace("\\", "/"));
            }

            return saveImagesSrc;
        }
        public Task DeleteImageAsync(string src)
        {
            var info = _fileProvider.GetFileInfo(src);
            if (info.Exists)
            {
                var filePath = info.PhysicalPath;
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            return Task.CompletedTask;
        }

        #endregion

    }
}
