using Microsoft.EntityFrameworkCore;
using UserManagement.Models;

namespace UserManagement.Data
{
    public class UserManagementDbContext : DbContext
    {
        public UserManagementDbContext(DbContextOptions<UserManagementDbContext> options) 
            : base(options)
        {
            
        }

        public DbSet<User> tb_Users { get; set; }
    }
}