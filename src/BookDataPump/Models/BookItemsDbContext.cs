using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BookDataPump.Models
{
    public class BookItemsDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public BookItemsDbContext(DbContextOptions<BookItemsDbContext> options, IConfiguration configuration) : base(options) {
            _configuration = configuration;
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_configuration.GetSection("DBConnectionString").Value);
        }

        public DbSet<BookItem> BookItems { get; set; }
    }
}
