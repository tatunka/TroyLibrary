using Microsoft.AspNetCore.Identity;

namespace TroyLibrary.Common.Models.Auth
{
    public class RegisterResponse
    {
        public string Token { get; set; }
        public List<IdentityError>? Errors{ get; set; }
    }
}
