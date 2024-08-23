namespace Couple_Quiz.Models
{
    public class Role:BaseClass
    {
        public string RoleName { get; set; }
        public ICollection<UserRole> UserRole { get; set; }


    }
}
