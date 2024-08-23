namespace Couple_Quiz.DTO.Response.User
{
    public class AuthUserResposne
    {
        public string JwtToken { get; set; }
        public string RefreshToken { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
