using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Constants.SourceType;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RealEstateAPI.Models;
using RealEstateAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealEstateAPI.Services
{
    public class ImageService : IImageService
    {
        private readonly Cloudinary _cloudinary;
        private readonly RealEstateContext _context;

        public ImageService(Cloudinary cloudinary, RealEstateContext context)
        {
            _cloudinary = cloudinary;
            _context = context;
        }

        public async Task<Image> UploadImageAsync(IFormFile file, SourceType sourceType, long sourceId)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Invalid file.");

            // Upload the file to Cloudinary
            var uploadResult = new ImageUploadResult();

            using (var stream = file.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Crop("fill").Gravity("face").Width(500).Height(500)
                };

                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }

            if (uploadResult.Error != null)
                throw new Exception(uploadResult.Error.Message);

            // Create an Image entity
            var image = new Image
            {
                Name = file.FileName,
                ImagePath = uploadResult.SecureUrl.ToString(),
                SourceType = sourceType,
                SourceId = sourceId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Save image to the database
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
