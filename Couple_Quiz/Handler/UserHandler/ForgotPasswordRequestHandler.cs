using Couple_Quiz.Interface.Repositories;
using MediatR;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using Couple_Quiz.DTO.Request.Query.User;
using Couple_Quiz.Common.Interface.Auth;

namespace Couple_Quiz.Handler.UserHandler
{
    public class ForgotPasswordRequestHandler : IRequestHandler<ForgotPasswordRequest, Unit>
    {
        private readonly IAuthRepository    authRepository;
        private readonly IAuthHelper authHelper;
        public ForgotPasswordRequestHandler(IAuthRepository authRepository, IConfiguration config, IAuthHelper authHelper)
        {
            this.authRepository = authRepository;
            this.authHelper = authHelper;
        }
        public async Task<Unit> Handle(ForgotPasswordRequest request, CancellationToken cancellationToken)
        {
            var user = await authRepository.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new Exception("No User Associated with this Email");
            }

            var otp = authHelper.GenerateOtp();
            await authRepository.SaveResetPasswordOtpAsync(user, otp);

            await authHelper.SendResetPasswordOtpEmail(user.Email, otp, user.Name);
            return Unit.Value;
            
        }
     

    }
}
