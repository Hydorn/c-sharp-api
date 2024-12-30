using EntityFW.Models;
using EntityFW.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EntityFW.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        public AuthController(AuthService authService)
        {
            this._authService = authService;
        }

        [HttpPost("Register")]
        public async Task<ActionResult> Register([FromBody] UserRegisterDTO user)
        {
            try
            {
                await this._authService.RegisterNewUser(user);
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong trying to register the new user");
                throw;

            }
        }


        [HttpPost("Login")]
        public async Task<ActionResult<LoginResponseDTO>> Login([FromBody] LoginCredentialsDTO credentials)
        {
            try
            {
                string token = await this._authService.LoginUser(credentials);
                LoginResponseDTO response = new LoginResponseDTO();
                response.AuthToken = token;
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong trying to log in");
                throw;

            }
        }
    }
}
