using Couple_Quiz.Data;
using Couple_Quiz.Interface.Repositories;
using Couple_Quiz.Models;
using Microsoft.EntityFrameworkCore;

namespace Couple_Quiz.Repositories
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        private readonly ApplicationDbContext _context;
        public RoleRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Role> GetById(Guid id)
        {
            return await _context.Role
                                 .FirstOrDefaultAsync(r => r.GlobalId == id);
        }

        public async Task<Role> GetByName()
        {
            return await _context.Role
                                .FirstOrDefaultAsync(r => r.RoleName == "User");
        }
    }
}
