using Microsoft.AspNetCore.Identity;

namespace Web3_Afternoon.Entities
{
    public class ApplicationUser:IdentityUser
    {
        public string Fullname { get; set; }
    }
}
