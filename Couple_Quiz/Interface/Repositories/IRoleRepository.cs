using Couple_Quiz.Models;

namespace Couple_Quiz.Interface.Repositories
{
    public interface IRoleRepository : IBaseRepository<Role>
    {
        Task<Role> GetById(Guid id);
        Task<Role> GetByName();

    }
}
