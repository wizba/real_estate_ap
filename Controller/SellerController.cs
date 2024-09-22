using Microsoft.AspNetCore.Mvc;
using RealEstateAPI.Models;
using RealEstateAPI.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealEstateAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SellerController : ControllerBase
    {
        private readonly ISellerService _sellerService;

        public SellerController(ISellerService sellerService)
        {
            _sellerService = sellerService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Seller>>> GetSellers()
        {
            var sellers = await _sellerService.GetAllSellersAsync();
            return Ok(sellers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Seller>> GetSeller(long id)
        {
            var seller = await _sellerService.GetSellerByIdAsync(id);
            if (seller == null)
            {
                return NotFound();
            }
            return Ok(seller);
        }

        [HttpPost]
        public async Task<ActionResult> CreateSeller(Seller seller)
        {
            await _sellerService.AddSellerAsync(seller);
            return CreatedAtAction(nameof(GetSeller), new { id = seller.Id }, seller);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSeller(long id, Seller seller)
        {
            if (id != seller.Id)
            {
                return BadRequest();
            }

            await _sellerService.UpdateSellerAsync(seller);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSeller(long id)
        {
            await _sellerService.DeleteSellerAsync(id);
            return NoContent();
        }
    }
}
