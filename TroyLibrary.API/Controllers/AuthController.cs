using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Extensions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TroyLibrary.Common.Models.Auth;
using TroyLibrary.Data.Models;
using TroyLibrary.Service.Interfaces;

namespace TroyLibrary.API.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<TroyLibraryUser> _userManager;
        private readonly SignInManager<TroyLibraryUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IAuthService _authService;

        public AuthController(UserManager<TroyLibraryUser> userManager, SignInManager<TroyLibraryUser> signInManager, IConfiguration configuration, IAuthService authService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _authService = authService;
        }

        [HttpPost("Register")]
        public async Task<RegisterResponse> Register([FromBody] RegisterRequest request)
        {
            return await this._authService.Register(request);
        }

        [HttpPost("Login")]
        public async Task<LoginResponse> Login([FromBody] LoginRequest request)
        {
            return await this._authService.Login(request);
        }
    }
}
