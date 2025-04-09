using Microsoft.AspNetCore.Http;
namespace ECommerce.Core.Services
{
    public interface IImageManagementService
    {
        Task<List<string>> UploadImagesAsync(IFormFileCollection files, string src);
        Task DeleteImageAsync(string src);
    }
}
