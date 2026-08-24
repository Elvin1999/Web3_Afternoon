using Microsoft.AspNetCore.Identity;

namespace Web3_Afternoon.Entities
{
    public class ApplicationUser:IdentityUser
    {
        public string Fullname { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpireTime { get; set; }
    }
}
