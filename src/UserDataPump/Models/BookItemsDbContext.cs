using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace UserDataPump.Models
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
        public DbSet<BookVolumeInfo> BookVolumeInfos { get; set; }
        public DbSet<BookAccessInfo> BookAccessInfos { get; set; }
        public DbSet<BookAccessInfoListPrice> BookAccessInfoListPrices { get; set; }
        public DbSet<BookSearchInfo> BookSearchInfos { get; set; }
        public DbSet<BookSaleInfo> BookSaleInfos { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<OfferListPrice> OfferListPrices { get; set; }
        public DbSet<Epub> Epubs { get; set; }
        public DbSet<ImageLink> ImageLinks { get; set; }
        public DbSet<IndustryIdentifier> IndustryIdentifiers { get; set; }
        public DbSet<ReadingMode> ReadingModes { get; set; }

    }
}
