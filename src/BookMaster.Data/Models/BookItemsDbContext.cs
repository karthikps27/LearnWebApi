using Microsoft.EntityFrameworkCore;

namespace BookMaster.Data.Models
{
    public class BookItemsDbContext : DbContext
    {
        public BookItemsDbContext(DbContextOptions<BookItemsDbContext> options) : base(options) 
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<BookItem> BookItems { get; set; }
    }
}
