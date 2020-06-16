using BookDataPump.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace BookDataPump.Models
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
