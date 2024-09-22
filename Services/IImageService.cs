using Constants.SourceType;
using Microsoft.AspNetCore.Http;
using RealEstateAPI.Models;
using System.Threading.Tasks;

namespace RealEstateAPI.Services
{
    public interface IImageService
    {
        Task<Image> UploadImageAsync(IFormFile file, SourceType sourceType, long sourceId);
        Task<Image> GetImageByIdAsync(long id);
        Task<IEnumerable<Image>> GetImagesBySourceAsync(SourceType sourceType, long sourceId);
    }
}
