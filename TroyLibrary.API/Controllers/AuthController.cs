using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Extensions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TroyLibrary.Common.Models.Auth;
using TroyLibrary.Data.Models;

namespace TroyLibrary.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<TroyLibraryUser> _userManager;
        private readonly SignInManager<TroyLibraryUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<TroyLibraryUser> userManager, SignInManager<TroyLibraryUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpPost("Register")]
        public async Task<RegisterResponse> Register([FromBody] RegisterRequest request)
        {
            var user = new TroyLibraryUser
            {
                UserName = request.Credentials.UserName,
                Email = request.Email,
            };

            var result = await _userManager.CreateAsync(user, request.Credentials.Password);

            if (result.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(user, request.Role.GetDisplayName());
                return new RegisterResponse
                {
                    Token = await GenerateJwtToken(user),
                };
            }

            return new RegisterResponse { Errors = result.Errors.ToList() };
        }

        [HttpPost("Login")]
        public async Task<LoginResponse> Login([FromBody] LoginRequest request)
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

        private async Task<string> GenerateJwtToken(TroyLibraryUser user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var roles = await this._userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id),
                new(JwtRegisteredClaimNames.Name, user.UserName),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: credentials
             );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
