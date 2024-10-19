using Constants.SourceType;
using RealEstateAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealEstateAPI.Repositories
{
    public interface IImageRepository
    {
        Task<Image> AddImageAsync(Image image);
        Task<Image> GetImageByIdAsync(long id);
        Task<IEnumerable<Image>> GetImagesBySourceAsync(SourceType sourceType, long sourceId);
    }
}
