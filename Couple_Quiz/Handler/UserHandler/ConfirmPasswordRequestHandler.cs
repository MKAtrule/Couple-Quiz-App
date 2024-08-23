using Couple_Quiz.DTO.Request.Command.Users;
using Couple_Quiz.Interface.Repositories;
using MediatR;

namespace Couple_Quiz.Handler.UserHandler
{
    public class ConfirmPasswordRequestHandler : IRequestHandler<ConfirmPasswordRequest, string>
    {
        private readonly IAuthRepository authRepository;
        public ConfirmPasswordRequestHandler(IAuthRepository authRepository)
        {
            this.authRepository = authRepository;
        }
        public async Task<string> Handle(ConfirmPasswordRequest request, CancellationToken cancellationToken)
        {
            var updatedUser = await authRepository.UpdatePassword(request.Email, request.NewPassword);
            if (updatedUser != null)
            {
                return "Updated Successfuly";
            }
            else
            {
                throw new Exception("User with that email not found");
            }
        }
    }
}
