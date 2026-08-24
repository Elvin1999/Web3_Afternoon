using System.Security.Claims;
using Web3_Afternoon.Entities;

namespace Web3_Afternoon.Services.Abstract
{
    public interface IAuthService
    {
        Task<string> GenerateToken(ApplicationUser user);
        string GenerateRefreshToken();
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}
