using BookDataPump.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace BookDataPump.Models
{
    public class BookItemsDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        private readonly ParameterStore _parameterStore;
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
            
            //Task<string> usernameTask = _parameterStore.GetParameterValueAsync(_configuration.GetSection("usernameParameterPath").Value, false);
            //Task<string> passwordTask = _parameterStore.GetParameterValueAsync(_configuration.GetSection("passwordParameterPath").Value, false);

            /*string sqlServerDBUrl = _configuration["sqlServerDBUrl"];
            string sqlServerDBUsername = _configuration["sqlServerDBUsername"];
            string sqlServerDBPassword = _configuration["sqlServerDBPassword"];*/

            //optionsBuilder.UseNpgsql($"Server={sqlServerDBUrl};Database=Books;User Id={sqlServerDBUsername}Password={sqlServerDBPassword}");
        }

        public DbSet<BookItem> BookItems { get; set; }
    }
}
