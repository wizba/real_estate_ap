using RealEstateAPI.Models;

namespace real_estate_api.Auth.Authenitication
{
    public interface IUserAuth
    {
        Task<AuthenticateResponse?> Authenticate(AuthenticateRequest model);
        Task<IEnumerable<Person>> GetAll();
        Task<Person?> GetById(int id);
        Task<Person?> AddAndUpdateUser(Person userObj);
    }
}
