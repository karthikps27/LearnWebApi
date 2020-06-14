﻿using BookDataPump.Framework;
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
        public BookItemsDbContext(DbContextOptions<BookItemsDbContext> options, IConfiguration configuration, ParameterStore parameterStore) : base(options) {
            _configuration = configuration;
            _parameterStore = parameterStore;
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseNpgsql(_configuration.GetSection("DBConnectionString").Value);

            Task<string> usernameTask = _parameterStore.GetParameterValueAsync(_configuration.GetSection("usernameParameterPath").Value, false);
            Task<string> passwordTask = _parameterStore.GetParameterValueAsync(_configuration.GetSection("passwordParameterPath").Value, false);

            string DBServerUrl = _configuration["DBServerUrl"];
            string DBUsername = _configuration["DBUsername"];
            string DBPassword = _configuration["DBPassword"];

            optionsBuilder.UseNpgsql($"Server={DBServerUrl};Database=Books;User Id={DBUsername}Password={DBPassword}");
        }

        public DbSet<BookItem> BookItems { get; set; }
    }
}
