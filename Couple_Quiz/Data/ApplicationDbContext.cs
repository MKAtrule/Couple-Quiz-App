using Couple_Quiz.Models;
using Microsoft.EntityFrameworkCore;

namespace Couple_Quiz.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options) 
        {
            
        }
        public DbSet<User> User { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<Role> Role { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var adminRoleId = Guid.NewGuid();
            var userRoleId = Guid.NewGuid();

            modelBuilder.Entity<Role>().HasData(
                new Role { GlobalId = adminRoleId, RoleName = "Admin" },
                new Role { GlobalId = userRoleId, RoleName = "User" }
            );

            var adminUserId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    GlobalId = adminUserId,
                    Name = "admin",
                    Email = "admin@example.com",
                    Password = "admin@123",
                    Gender = "Male",
                    Active = true,
                    ProfileImage = "...",
                    CreatedAt = DateTime.UtcNow
                },
                new User
                {
                    GlobalId = userId,
                    Name = "user",
                    Email = "user@example.com",
                    Password = "user@123",
                    Active = true,
                    Gender = "Female",
                    ProfileImage = "...",
                    CreatedAt = DateTime.UtcNow
                }
            );


            modelBuilder.Entity<UserRole>().HasData(
                new UserRole
                {
                    GlobalId = Guid.NewGuid(),
                    UserId = adminUserId,
                    RoleId = adminRoleId
                },
                new UserRole
                {
                    GlobalId = Guid.NewGuid(),
                    UserId = userId,
                    RoleId = userRoleId
                }
            );
        }

    }
}
