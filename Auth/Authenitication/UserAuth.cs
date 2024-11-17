using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using real_estate_api.Config;
using RealEstateAPI.Models;
using RealEstateAPI.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace real_estate_api.Auth.Authenitication
{
    public class UserAuth : IUserAuth
    {
        private readonly AppSettings _appSettings;
        private readonly PersonRepository _personRepository;

        public UserAuth(IOptions<AppSettings> appSettings, PersonRepository personRepository)
        {
            _appSettings = appSettings.Value;
            _personRepository = personRepository;
        }
        public Task<Person?> AddAndUpdateUser(Person userObj)
        {
      
            throw new NotImplementedException();
        }

        public async Task<AuthenticateResponse?> Authenticate(AuthenticateRequest model)
        {
            var user = await _personRepository.GetByUserNameAndPassword(model.Username);

            if (model.Password != user.Password) { 
                return null;
            }

            if (user == null) { 
                return null;
            }

            var token = await generateJwtToken(user);

            return new AuthenticateResponse(token);
        }

        public Task<IEnumerable<Person>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Person?> GetById(int id)
        {
            throw new NotImplementedException();
        }

        private async Task<string> generateJwtToken(Person user)
        {
            //Generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = await Task.Run(() =>
            {

                var key = Encoding.UTF8.GetBytes(_appSettings.Secret);
                
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] { 
                        new Claim("id", user.Id.ToString()),
                        new Claim("email",user.Email),
                        new Claim("role",user.Role)
                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                return tokenHandler.CreateToken(tokenDescriptor);
            });

            return tokenHandler.WriteToken(token);
        }
    }
}
