using real_estate_api.Services;
using RealEstateAPI.Models;
using RealEstateAPI.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealEstateAPI.Services
{
    public class SellerService : IUserService
    {
        private readonly ISellerRepository _clientRepository;

        public SellerService(ISellerRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<IEnumerable<Person>> GetAllUsersAsync()
    {
        return await _clientRepository.GetAllAsync();
    }

    public async Task<Person> GetUserByIdAsync(long id)
    {
        return await _clientRepository.GetByIdAsync(id);
    }

    public async Task AddUserAsync(Person client)
    {
        await _clientRepository.AddAsync((Seller)client);
    }

    public async Task UpdateUserAsync(Person client)
    {
        await _clientRepository.UpdateAsync((Seller)client);
    }

    public async Task DeleteUserAsync(long id)
    {
        await _clientRepository.DeleteAsync(id);
    }
}
}
