using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Couple_Quiz.DTO.Request.Command.Users
{
    public class VerifyOtpRequest:IRequest<string>
    {

        [Required]
        public string Email { get; set; }

        [Required]
        public string Otp { get; set; }
    }
}
