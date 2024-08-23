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
        private readonly IConfiguration config;
        private readonly IUserRoleRepository userRoleRepository;

        public AuthUserRequestHandler(IAuthRepository authRepository, IConfiguration config, IUserRoleRepository roleRepository)
        {
            this.authRepository = authRepository;
            this.config = config;
            this.userRoleRepository = roleRepository;
        }
        public async Task<AuthUserResposne> Handle(AuthUserRequest request, CancellationToken cancellationToken)
        {
            var user = await authRepository.FindUserAsync(request);
            if (user != null)
            {
                var roles = await userRoleRepository.GetUserRolesAsync(user.GlobalId);

                var token = await GenerateToken(user, roles);
                var refreshToken = GenerateRefreshToken();
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
        private  async Task<string> GenerateToken(User user, List<string> roles)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim("Email", user.Email));
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }



            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: config["JWT:Issuer"],
                audience: config["JWT:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(20),
                signingCredentials: credentials);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenString = tokenHandler.WriteToken(token);

            return await Task.FromResult(tokenString);
        }
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

    }
}
