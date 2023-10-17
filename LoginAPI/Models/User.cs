using System.Diagnostics.CodeAnalysis;

namespace LoginAPI.Models
{
    public class User
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? PasswordResetToken { get; set; }
        public DateTime? TokenGeneratedTime { get; set; }

    }
}
