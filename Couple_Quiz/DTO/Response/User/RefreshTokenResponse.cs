using System.ComponentModel.DataAnnotations;

namespace Couple_Quiz.DTO.Response.User
{
    public class RefreshTokenResponse
    { 

    [Required]
    public string Token { get; set; }
    [Required]
    public string RefreshToken { get; set; }

    }
}
