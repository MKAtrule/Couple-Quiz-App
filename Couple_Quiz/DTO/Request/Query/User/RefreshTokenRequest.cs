using Couple_Quiz.DTO.Response.User;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Couple_Quiz.DTO.Request.Query.User
{
    public class RefreshTokenRequest : IRequest<RefreshTokenResponse>
    {
        [Required]
        public string Token { get; set; }
        [Required]
        public string RefreshToken { get; set; }
    }
}
