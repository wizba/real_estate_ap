using RealEstateAPI.Models;

namespace real_estate_api.Services
{
    public interface IUserService
    {
        Task<IEnumerable<Person>> GetAllUsersAsync();
        Task<Person> GetUserByIdAsync(long id);
        Task AddUserAsync(Person client);
        Task UpdateUserAsync(Person client);
        Task DeleteUserAsync(long id);
    }
}
