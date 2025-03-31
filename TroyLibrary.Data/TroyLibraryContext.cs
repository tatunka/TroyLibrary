using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TroyLibrary.Data.Models;

namespace TroyLibrary.Data
{
    public class TroyLibraryContext : IdentityDbContext<TroyLibraryUser>
    {
        private readonly IConfiguration _configuration;

        public TroyLibraryContext(DbContextOptions<TroyLibraryContext> options, IConfiguration configuration)
            : base(options)
        {
            this._configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options) =>
           options.UseSqlServer(this._configuration["ConnectionStrings:TroyLibraryContext"]);

        public DbSet<TroyLibraryUser> TroyLibraryUsers { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TroyLibraryUser>()
                .ToTable("TroyLibraryUser");
            modelBuilder.Entity<Book>()
                .ToTable("Book");
            modelBuilder.Entity<Category>()
                .ToTable("Category");
            modelBuilder.Entity<Review>()
                .ToTable("Review")
                .HasOne(r => r.TroyLibraryUser)
                .WithMany(u => u.Reviews)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
