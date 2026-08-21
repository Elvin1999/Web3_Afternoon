using Web3_Afternoon.Entities;

namespace Web3_Afternoon.Services.Abstract
{
    public interface IAuthService
    {
        string GenerateToken(ApplicationUser user);
    }
}
