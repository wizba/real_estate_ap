using real_estate_api.Services;
using RealEstateAPI.Models;
using RealEstateAPI.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealEstateAPI.Services
{
    public class SellerService : IUserService
    {
        private readonly IClientRepository _clientRepository;

    public SellerService(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<IEnumerable<Client>> GetAllUsersAsync()
    {
        return await _clientRepository.GetAllAsync();
    }

    public async Task<Client> GetUserByIdAsync(long id)
    {
        return await _clientRepository.GetByIdAsync(id);
    }

    public async Task AddUserAsync(Client client)
    {
        await _clientRepository.AddAsync(client);
    }

    public async Task UpdateUserAsync(Client client)
    {
        await _clientRepository.UpdateAsync(client);
    }

    public async Task DeleteUserAsync(long id)
    {
        await _clientRepository.DeleteAsync(id);
    }
}
}
