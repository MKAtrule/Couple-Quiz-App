using Couple_Quiz.Models;

namespace Couple_Quiz.Interface.Repositories
{
    public interface IUserRoleRepository : IBaseRepository<UserRole>
    {
        Task<List<string>> GetUserRolesAsync(Guid userId);
    }
}
