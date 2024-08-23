using Couple_Quiz.DTO.Response.User;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Couple_Quiz.DTO.Request.Command.User
{
    public class CreateUserRequest:IRequest<CreateUserReposne>
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        public IFormFile ProfileImage { get; set; }
        public int Age { get; set; }
        [Required]
        public string Gender { get; set; }
        //[JsonIgnore]
        //public DateTime CreatedAt { get; set; }
    }
}
