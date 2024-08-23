using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Couple_Quiz.DTO.Request.Query.User
{
    public class ForgotPasswordRequest : IRequest<Unit>
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
