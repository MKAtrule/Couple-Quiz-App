using Couple_Quiz.DTO.Response.User;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Couple_Quiz.DTO.Request.Command.User
{
    public class AuthUserRequest:IRequest<AuthUserResposne>
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
