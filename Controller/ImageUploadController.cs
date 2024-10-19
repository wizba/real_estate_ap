using Constants.SourceType;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstateAPI.Services;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class ImageUploadController : ControllerBase
{
    private readonly IImageService _imageService;

    public ImageUploadController(IImageService imageService)
    {
        _imageService = imageService;
    }

   
    [HttpPost]
    public async Task<IActionResult> UploadImage(IFormFile file, SourceType sourceType, long sourceId)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file provided.");

        try
        {
            var image = await _imageService.UploadImageAsync(file, sourceType, sourceId);
            return Ok(new
            {
                Url = image.ImagePath,
                SourceType = image.SourceType,
                SourceId = image.SourceId
            });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // GET: api/ImageUpload/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetImageById(long id)
    {
        var image = await _imageService.GetImageByIdAsync(id);
        if (image == null)
            return NotFound();

        return Ok(image);
    }


    [HttpGet("source")]
    public async Task<IActionResult> GetImagesBySource(SourceType sourceType, long sourceId)
    {
        var images = await _imageService.GetImagesBySourceAsync(sourceType, sourceId);
        return Ok(images);
    }
}
