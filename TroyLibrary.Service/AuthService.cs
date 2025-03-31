using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TroyLibrary.Common.Models.Auth;
using TroyLibrary.Data.Models;
using TroyLibrary.Service.Interfaces;

namespace TroyLibrary.Service
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<TroyLibraryUser> _userManager;
        private readonly SignInManager<TroyLibraryUser> _signInManager;
        private readonly IConfiguration _config;

        public AuthService(UserManager<TroyLibraryUser> userManager, SignInManager<TroyLibraryUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = configuration;
        }

        public async Task<RegisterResponse> Register(RegisterRequest request)
        {
            var user = new TroyLibraryUser
            {
                UserName = request.Credentials.UserName,
                Email = request.Email,
            };

            var result = await _userManager.CreateAsync(user, request.Credentials.Password);

            if (result.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(user, request.Role.ToString());
                return new RegisterResponse
                {
                    Token = await GenerateJwtToken(user),
                };
            }

            return new RegisterResponse { Errors = result.Errors.ToList() };
        }

        public async Task<LoginResponse> Login(LoginRequest request)
        {
            var result = await _signInManager.PasswordSignInAsync(request.Credentials.UserName, request.Credentials.Password, false, false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(request.Credentials.UserName);
                var token = await GenerateJwtToken(user);
                return new LoginResponse
                {
                    Token = token,
                };
            }
            return new LoginResponse();
        }

        public async Task<string> GenerateJwtToken(TroyLibraryUser user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var roles = await this._userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id),
                new(JwtRegisteredClaimNames.Name, user.UserName),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));


            var minutesString = _config["Jwt:ExpirationInMinutes"];
            double minutes = 1440;
            if (!string.IsNullOrWhiteSpace(minutesString))
            {
                minutes = double.Parse(minutesString);
            }
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(minutes),
                signingCredentials: credentials
             );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
