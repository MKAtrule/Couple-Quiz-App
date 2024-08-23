using Couple_Quiz.Data;
using Couple_Quiz.DTO.Request.Command.User;
using Couple_Quiz.Interface.Repositories;
using Couple_Quiz.Models;
using Microsoft.EntityFrameworkCore;

namespace Couple_Quiz.Repositories
{
    public class AuthRepository : BaseRepository<User>, IAuthRepository
    {
        private readonly ApplicationDbContext _context;
        public AuthRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;

        }

        
        public async Task<User> GetByIdAsync(Guid id)
        {
            return await _context.User.
                FirstOrDefaultAsync(us => us.GlobalId == id);
        }
        public async Task SaveRefreshTokenAsync(User user, string refreshToken)
        {
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            _context.Update(user);
            await _context.SaveChangesAsync();
        }
        public async Task<User> GetUserByRefreshTokenAsync(string refreshToken)
        {
            return await _context.User.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
        }
        public async Task SaveResetPasswordOtpAsync(User user, string otp)
        {
            user.ResetPasswordOtp = otp;
            user.ResetPasswordOtpExpiryTime = DateTime.UtcNow.AddMinutes(2);
            _context.Update(user);
            await _context.SaveChangesAsync();
        }
        public async Task<User> GetUserByOtpAsync(string email, string otp)
        {
            return await _context.User.FirstOrDefaultAsync(u => u.Email == email && u.ResetPasswordOtp == otp && u.ResetPasswordOtpExpiryTime > DateTime.UtcNow);
        }
        public async Task<User> FindByEmailAsync(string email)
        {
            return await _context.User.FirstOrDefaultAsync(us => us.Email == email);
        }
        public async Task<User> UpdatePassword(string email, string password)
        {
            var user = await _context.User.FirstOrDefaultAsync(us => us.Email == email);
            if (user != null)
            {
                user.Password = password;
                _context.User.Update(user);
                await _context.SaveChangesAsync();
                return user;

            }
            else
            {
                throw new Exception("User not found");
            }
        }
        public async Task<User> FindUserAsync(AuthUserRequest request)
        {
            return await _context.User
                                 .FirstOrDefaultAsync(us => us.Email == request.Email && us.Password == request.Password);
        }


    }
}
