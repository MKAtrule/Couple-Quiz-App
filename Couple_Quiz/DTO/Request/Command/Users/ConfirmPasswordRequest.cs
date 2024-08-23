using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Couple_Quiz.DTO.Request.Command.Users
{
    public class ConfirmPasswordRequest:IRequest<string>
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}
