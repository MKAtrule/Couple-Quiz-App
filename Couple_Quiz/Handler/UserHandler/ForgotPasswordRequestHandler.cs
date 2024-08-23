using Couple_Quiz.Interface.Repositories;
using MediatR;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using Couple_Quiz.DTO.Request.Query.User;

namespace Couple_Quiz.Handler.UserHandler
{
    public class ForgotPasswordRequestHandler : IRequestHandler<ForgotPasswordRequest, Unit>
    {
        private readonly IAuthRepository    authRepository;
        private readonly IConfiguration config;
        public ForgotPasswordRequestHandler(IAuthRepository authRepository, IConfiguration config)
        {
            this.authRepository = authRepository;
            this.config = config;
        }
        public async Task<Unit> Handle(ForgotPasswordRequest request, CancellationToken cancellationToken)
        {
            var user = await authRepository.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new Exception("No User Associated with this Email");
            }

            var otp = GenerateOtp();
            await authRepository.SaveResetPasswordOtpAsync(user, otp);

            await SendResetPasswordOtpEmail(user.Email, otp, user.Name);
            return Unit.Value;
            
        }
        private string GenerateOtp()
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                var randomNumber = new byte[4];
                rng.GetBytes(randomNumber);
                int value = BitConverter.ToInt32(randomNumber, 0) % 10000;
                return Math.Abs(value).ToString("D4");
            }

        }
        private async Task SendResetPasswordOtpEmail(string email, string otp, string name)
        {
            using (MailMessage ms = new MailMessage(config["SMTP:Username"], email))
            {
                ms.Subject = "Couple Quiz  - OTP Verification";
                ms.Body = $@"
        <html>
        <head>
            <style>
                body {{
                    font-family: Arial, sans-serif;
                    background-color: #f4f4f4;
                    color: #333;
                    padding: 20px;
                }}
                .container {{
                    max-width: 600px;
                    margin: 0 auto;
                    background-color: #fff;
                    padding: 20px;
                    border-radius: 8px;
                    box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                }}
                .header {{
                    text-align: center;
                    margin-bottom: 20px;
                }}
                .header h2 {{
                    color: #FC4468;
                }}
                .otp-container {{
                    text-align: center;
                    margin: 20px 0;
                }}
                .otp {{
                    display: inline-block;
                    padding: 10px 20px;
                    background-color: #FC4468;
                    color: #fff;
                    text-align: center;
                    border-radius: 5px;
                    font-size: 24px;
                    font-weight: bold;
                }}
                .footer {{
                    margin-top: 20px;
                    text-align: center;
                    font-size: 12px;
                    color: #aaa;
                }}
                .footer a {{
                    color: #007bff;
                    text-decoration: none;
                }}
                .footer a:hover {{
                    text-decoration: underline;
                }}
            </style>
        </head>
        <body>
            <div class='container'>
                <div class='header'>
                    <h2>Couple Quiz</h2>
                    <p>OTP Verification</p>
                </div>
                <p>Hi {name},</p>
                <p>You recently requested to verify your account for your Couple account. Use the OTP below to proceed:</p>
                <div class='otp-container'>
                    <p class='otp'>{otp}</p>
                </div>
                <p>This OTP is only valid for the next 2 minutes.</p>
                <p>If you did not request an OTP, please ignore this email or <a href='mailto:support@couplequiz.com'>contact support</a> if you have questions.</p>
                <div class='footer'>
                    <p>&copy; {DateTime.Now.Year} Invitation Card Maker. All rights reserved.</p>
                </div>
            </div>
        </body>
        </html>";
                ms.IsBodyHtml = true;
                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Host = config["SMTP:Host"];
                    smtp.EnableSsl = true;
                    NetworkCredential crd = new NetworkCredential(config["SMTP:Username"], config["SMTP:Password"]);
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = crd;
                    smtp.Port = int.Parse(config["SMTP:Port"]);
                    await smtp.SendMailAsync(ms);
                }
            }
        }

    }
}
