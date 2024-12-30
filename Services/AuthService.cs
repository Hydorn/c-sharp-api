using EntityFW.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EntityFW.Services
{
    public class AuthService
    {
        private readonly UserService _userService;
        private readonly IConfiguration _configuration;
        private readonly PasswordHasher<User> _passwordHasher;
        public AuthService(UserService userService, IConfiguration configuration)
        {

            this._userService = userService;
            this._configuration = configuration;
            this._passwordHasher = new PasswordHasher<User>();
        }
        public async Task<User> RegisterNewUser(UserRegisterDTO user)
        {

            try
            {
                User NewUser = new User();
                NewUser.Id = Guid.NewGuid();
                NewUser.Password = this._passwordHasher.HashPassword(NewUser, user.Password);
                NewUser.Name = user.Name;
                NewUser.Email = user.Email;
                NewUser.CreatedDate = DateTime.Now;
                NewUser.Role = user.Role?.ToLower();
                await this._userService.CreateUser(NewUser);
                return NewUser;
            }
            catch (Exception)
            {
                //loger
                throw;
            }

        }


        public async Task<string> LoginUser(LoginCredentialsDTO credentials)
        {
            User? logginUser = await this._userService.GetUserByEmail(credentials.Email);

            if (logginUser == null)
            {
                throw new Exception("Something went wrong");
            }

            PasswordVerificationResult IsCorrectPassword = this._passwordHasher.VerifyHashedPassword(logginUser, logginUser.Password, credentials.Password);

            if (IsCorrectPassword == PasswordVerificationResult.Success)
            {
                return this.CreateJWTAuthToken(logginUser);
            }
            else
            {
                throw new Exception("Unable to authenticate");
            }

        }

        public string CreateJWTAuthToken(User user)
        {
            string secretKey = this._configuration["JWT:SECRET"] ?? "";
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            if (secretKey == null)
            {
                throw new Exception("Unable to create token");
            }


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity([
                     new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Name, user.Name.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email.ToString()),
                    //#TODO - add roles
                    new Claim("Admin", false.ToString())
                     ]),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = credentials,
                Issuer = this._configuration["JWT:ISSUER"],
                Audience = this._configuration["JWT:AUDIENCE"]
            };

            var handler = new Microsoft.IdentityModel.JsonWebTokens.JsonWebTokenHandler();

            string token = handler.CreateToken(tokenDescriptor);

            return token;
        }
    }
}
