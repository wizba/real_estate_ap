using Microsoft.AspNetCore.Mvc;
using real_estate_api.ErrorHandling;
using real_estate_api.Services;
using RealEstateAPI.Models;
using RealEstateAPI.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealEstateAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> GetClients()
        {
            var clients = await _clientService.GetAllUsersAsync();
            return Ok(clients);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetClient(long id)
        {
            var client = await _clientService.GetUserByIdAsync(id);
            //if (client == null)
            //{
            //    throw new NotFoundException($"User with ID {id} not found.");
            //}
            return Ok(client);
        }

        [HttpPost]
        public async Task<ActionResult> CreateClient(Client client)
        {
            await _clientService.AddUserAsync(client);
            return CreatedAtAction(nameof(GetClient), new { id = client.Id }, client);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClient(long id, Client client)
        {
            if (id != client.Id)
            {
                return BadRequest();
            }

            await _clientService.UpdateUserAsync(client);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(long id)
        {
            await _clientService.DeleteUserAsync(id);
            return NoContent();
        }
    }
}
