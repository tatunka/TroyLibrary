using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TroyLibrary.Data.Models;

namespace TroyLibrary.Data
{
    public class TroyLibraryContext : IdentityDbContext<TroyLibraryUser>
    {
        public TroyLibraryContext(DbContextOptions<TroyLibraryContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer("Server=localhost;Database=TroyLibrary;Trusted_Connection=True;TrustServerCertificate=True");

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
                .ToTable("Book")
                .Property(b => b.InStock)
                .HasDefaultValue(true);
            modelBuilder.Entity<Category>()
                .ToTable("Category");
            modelBuilder.Entity<Review>()
                .ToTable("Review")
                .HasOne(r => r.TroyLibraryUser)
                .WithMany(u => u.Reviews)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
