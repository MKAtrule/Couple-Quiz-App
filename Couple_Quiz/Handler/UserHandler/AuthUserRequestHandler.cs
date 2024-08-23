using Couple_Quiz.Common.Interface.Auth;
using Couple_Quiz.DTO.Request.Command.User;
using Couple_Quiz.DTO.Response.User;
using Couple_Quiz.Interface.Repositories;
using Couple_Quiz.Models;
using Couple_Quiz.Repositories;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Couple_Quiz.Handler.UserHandler
{
    public class AuthUserRequestHandler : IRequestHandler<AuthUserRequest, AuthUserResposne>
    {
        private readonly IAuthRepository authRepository;
        private readonly IUserRoleRepository userRoleRepository;
        private readonly IAuthHelper authHelper;
        public AuthUserRequestHandler(IAuthRepository authRepository, IUserRoleRepository roleRepository, IAuthHelper authHelper)
        {
            this.authRepository = authRepository;
            this.userRoleRepository = roleRepository;
            this.authHelper = authHelper;
        }
        public async Task<AuthUserResposne> Handle(AuthUserRequest request, CancellationToken cancellationToken)
        {
            var user = await authRepository.FindUserAsync(request);
            if (user != null)
            {
                var roles = await userRoleRepository.GetUserRolesAsync(user.GlobalId);

                var token = await authHelper.GenerateToken(user, roles);
                var refreshToken = authHelper.GenerateRefreshToken();
                await authRepository.SaveRefreshTokenAsync(user, refreshToken);

                return new AuthUserResposne
                {
                    JwtToken = token,
                    RefreshToken = refreshToken,
                    Email = user.Email,
                    UserName = user.Name,
                };
            }
            else
            {
                throw new Exception("User not found");
            }
        }
      

    }
}
