using Couple_Quiz.DTO.Request.Command.User;
using Couple_Quiz.Models;

namespace Couple_Quiz.Interface.Repositories
{
    public interface IAuthRepository : IBaseRepository<User>
    {
        Task<User> FindUserAsync(AuthUserRequest request);
        Task<User> GetByIdAsync(Guid id);
        Task SaveRefreshTokenAsync(User user, string refreshToken);
        Task<User> GetUserByRefreshTokenAsync(string refreshToken);
        Task SaveResetPasswordOtpAsync(User user, string otp);
        Task<User> GetUserByOtpAsync(string email, string otp);
        Task<User> FindByEmailAsync(string email);
        Task<User> UpdatePassword(string email, string password);


    }
}
