using RealEstateAPI.Models;

namespace real_estate_api.Services
{
    public interface IUserService
    {
        Task<IEnumerable<Client>> GetAllUsersAsync();
        Task<Client> GetUserByIdAsync(long id);
        Task AddUserAsync(Client client);
        Task UpdateUserAsync(Client client);
        Task DeleteUserAsync(long id);
    }
}
