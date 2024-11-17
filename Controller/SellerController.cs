using Microsoft.AspNetCore.Mvc;
using real_estate_api.Services;
using real_estate_api.Shared;
using RealEstateAPI.Models;

namespace RealEstateAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SellerController : ControllerBase
    {
        private readonly IUserService _sellerService;

        public SellerController(IUserService sellerService)
        {
            _sellerService = sellerService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Seller>>> GetSellers()
        {
            var sellers = await _sellerService.GetAllUsersAsync();
            return Ok(sellers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Seller>> GetSeller(long id)
        {
            var seller = await _sellerService.GetUserByIdAsync(id);
            if (seller == null)
            {
                return NotFound();
            }
            return Ok(seller);
        }

        [HttpPost]
        public async Task<ActionResult> CreateSeller(Seller seller)
        {
           
            // encrypt password
            seller.Password = Protect.EncryptPassword(seller.Password);
            
            await _sellerService.AddUserAsync(seller);
            return CreatedAtAction(nameof(GetSeller), new { id = seller.Id }, seller);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSeller(long id, Seller seller)
        {
            if (id != seller.Id)
            {
                return BadRequest();
            }

            await _sellerService.UpdateUserAsync(seller);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSeller(long id)
        {
            await _sellerService.DeleteUserAsync(id);
            return NoContent();
        }
    }
}
