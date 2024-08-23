using Couple_Quiz.DTO.Request.Query.User;
using Couple_Quiz.DTO.Response.User;
using Couple_Quiz.Interface.Repositories;
using Couple_Quiz.Models;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Couple_Quiz.Handler.UserHandler
{
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenRequest, RefreshTokenResponse>
    {
        private readonly IAuthRepository authRepository;
        private readonly IUserRoleRepository userRoleRepository;
        private readonly IConfiguration config;

        public RefreshTokenHandler(IAuthRepository authRepository, IUserRoleRepository userRoleRepository, IConfiguration config)
        {
            this.authRepository = authRepository;
            this.userRoleRepository = userRoleRepository;
            this.config = config;
        }
        public async Task<RefreshTokenResponse> Handle(RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var principal = GetPrincipalFromExpiredToken(request.Token);
            var email = principal.FindFirstValue("Email");
            var user = await authRepository.GetUserByRefreshTokenAsync(request.RefreshToken);

            if (user == null || user.Email != email || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                throw new SecurityTokenException("Invalid refresh token");
            }

            var roles = await userRoleRepository.GetUserRolesAsync(user.GlobalId);
            var newToken = await GenerateToken(user, roles);
            var newRefreshToken = GenerateRefreshToken();
            await authRepository.SaveRefreshTokenAsync(user, newRefreshToken);

            return new RefreshTokenResponse
            {
                Token = newToken,
                RefreshToken = newRefreshToken
            };
        }
        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = false,
                ValidIssuer = config["JWT:Issuer"],
                ValidAudience = config["JWT:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Key"]))
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if (!(securityToken is JwtSecurityToken jwtSecurityToken) ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }
        private async Task<string> GenerateToken(User user, List<string> roles)
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
