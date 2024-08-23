using Couple_Quiz.DTO.Request.Command.Users;
using Couple_Quiz.Interface.Repositories;
using MediatR;

namespace Couple_Quiz.Handler.UserHandler
{
    public class VerifyOtpHandler : IRequestHandler<VerifyOtpRequest, string>
    {
        private readonly IAuthRepository authRepository;
        public VerifyOtpHandler(IAuthRepository authRepository)
        {
            this.authRepository = authRepository;
        }
        public async Task<string> Handle(VerifyOtpRequest request, CancellationToken cancellationToken)
        {
            var user = await authRepository.GetUserByOtpAsync(request.Email, request.Otp);
            if (user == null)
            {
                throw new Exception("Invalid or expired OTP");
            }

            user.ResetPasswordOtp = null;
            user.ResetPasswordOtpExpiryTime = null;

            await authRepository.Update(user);
            return user.Email;
        }

    }
}
