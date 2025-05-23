using DemoAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace DemoAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)                 // 'ctor' then enter tab
        {

        }

        public DbSet<StudentEntity> StudentRegister { get; set; }
        public DbSet<LocalUser> LocalUsers { get; set; } 
    }
}
