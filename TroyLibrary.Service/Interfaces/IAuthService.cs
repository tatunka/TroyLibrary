using TroyLibrary.Common.Models.Auth;
using TroyLibrary.Data.Models;

namespace TroyLibrary.Service.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponse> Login(LoginRequest request);
        Task<RegisterResponse> Register(RegisterRequest request);
        Task<string> GenerateJwtToken(TroyLibraryUser user);
    }
}
