using Couple_Quiz.Models;
using System.Security.Claims;

namespace Couple_Quiz.Common.Interface.Auth
{
    public interface IAuthHelper
    {
        Task<string> GenerateToken(User user, List<string> roles);
        string GenerateRefreshToken();
        string GenerateOtp();
        Task SendResetPasswordOtpEmail(string email, string otp, string name);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
