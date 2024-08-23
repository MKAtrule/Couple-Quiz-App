using Couple_Quiz.DTO.Request.Command.User;
using Couple_Quiz.DTO.Request.Command.Users;
using Couple_Quiz.DTO.Request.Query.User;
using MediatR;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace Couple_Quiz.Controllers
{
    public class AuthController : BaseController
    {
        private readonly ISender sender;
        public AuthController(ISender sender)
        {
            this.sender = sender;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] AuthUserRequest request)
        {


            try
            {
                var authResponse = await sender.Send(request);
                return new JsonResult(new
                {
                    success = true,
                    token = authResponse.JwtToken,
                    refreshToken = authResponse.RefreshToken,
                    Email = authResponse.Email,
                    UserName = authResponse.UserName,
                    message = "Login Successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromForm] CreateUserRequest request)
        {
            try
            {

                return new JsonResult(new
                {
                    success = true,
                    data = await sender.Send(request),
                    Message = "User Account created SuccessFully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Messsage = ex.Message });
            }
        }
        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {

            try
            {
                var authResponse = await sender.Send(request);
                return new JsonResult(new
                {
                    success = true,
                    token = authResponse.Token,
                    refreshToken = authResponse.RefreshToken
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] DTO.Request.Query.User.ForgotPasswordRequest request)
        {


            try
            {
                await sender.Send(request);
                return new JsonResult(new { success = true, message = "OTP sent to email successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("VerifyOtp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequest request)
        {


            try
            {

                return new
                    JsonResult(new
                    {
                        success = true,
                        data = await sender.Send(request),
                        message = "OTP Verify successfully"
                    });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPost("ConfirmPassword")]
        public async Task<IActionResult> ConfirmPassword([FromBody] ConfirmPasswordRequest request)
        {


            try
            {
                return new JsonResult
              (
                    new
                    {
                        success = true,
                        message = await sender.Send(request),

                    });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
