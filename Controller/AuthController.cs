using Microsoft.AspNetCore.Mvc;
using real_estate_api.Auth.Authenitication;

namespace real_estate_api.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
       private readonly IUserAuth _userAuthService;

       public AuthController(IUserAuth _userAuthService)
        {
            this._userAuthService = _userAuthService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthenticateResponse>> Login([FromBody] AuthenticateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _userAuthService.Authenticate(request);

            if (response == null)
            {
                return Unauthorized(new { message = "Username or password is incorrect" });
            }

            return Ok(response);
        }

    }
}
