using Couple_Quiz.Common.Interface.Auth;
using Couple_Quiz.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Couple_Quiz.Common.Service.FileService
{
    public class AuthHelper : IAuthHelper
    {
        private readonly IConfiguration config;

        public AuthHelper(IConfiguration config)
        {
            this.config = config;
        }

        public async Task<string> GenerateToken(User user, List<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim("Email", user.Email)
            };
            roles.ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role)));

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

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public string GenerateOtp()
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                var randomNumber = new byte[4];
                rng.GetBytes(randomNumber);
                int value = BitConverter.ToInt32(randomNumber, 0) % 10000;
                return Math.Abs(value).ToString("D4");
            }
        }

        public async Task SendResetPasswordOtpEmail(string email, string otp, string name)
        {
            using (var ms = new MailMessage(config["SMTP:Username"], email))
            {
                ms.Subject = "Couple Quiz - OTP Verification";
                ms.Body = $@"
                    <html>
                    <head><style> /* Styles here */ </style></head>
                    <body> /* Body content here */ </body>
                    </html>";
                ms.IsBodyHtml = true;

                using (var smtp = new SmtpClient(config["SMTP:Host"]))
                {
                    smtp.EnableSsl = true;
                    var crd = new NetworkCredential(config["SMTP:Username"], config["SMTP:Password"]);
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = crd;
                    smtp.Port = int.Parse(config["SMTP:Port"]);
                    await smtp.SendMailAsync(ms);
                }
            }
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
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
    }
}

