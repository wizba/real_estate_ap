using RealEstateAPI.Models;

namespace real_estate_api.Auth.Authenitication
{
    public class AuthenticateResponse
    {
        public string Token { get; set; }


        public AuthenticateResponse(string token)
        {
            Token = token;
        }
    }
}
