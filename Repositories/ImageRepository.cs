using Constants.SourceType;
using Microsoft.EntityFrameworkCore;
using RealEstateAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstateAPI.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly RealEstateContext _context;

        public ImageRepository(RealEstateContext context)
        {
            _context = context;
        }

        public async Task<Image> AddImageAsync(Image image)
        {
            _context.Images.Add(image);
            await _context.SaveChangesAsync();
            return image;
        }

        public async Task<Image> GetImageByIdAsync(long id)
        {
            return await _context.Images.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<Image>> GetImagesBySourceAsync(SourceType sourceType, long sourceId)
        {
            return await _context.Images
                .Where(i => i.SourceType == sourceType && i.SourceId == sourceId)
                .ToListAsync();
        }
    }
}
