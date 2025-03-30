using static TroyLibrary.Common.Enums;

namespace TroyLibrary.Common.Models.Auth
{
    public class RegisterRequest
    {
        public Credentials Credentials { get; set; }
        public string? Email { get; set; }
        public Role Role { get; set; }
    }
}
