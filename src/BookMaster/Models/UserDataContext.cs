using Microsoft.EntityFrameworkCore;

namespace LearnWebApi.Models
{
    public class UserDataContext : DbContext
    {
        public UserDataContext(DbContextOptions<UserDataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserData>().ToTable("UserData", "UserManagement");
        }
        public DbSet<UserData> Userlist { get; set; }
    }
}
