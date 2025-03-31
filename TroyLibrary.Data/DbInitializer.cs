using Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TroyLibrary.Data.Models;

namespace TroyLibrary.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(TroyLibraryContext context, RoleManager<IdentityRole> roleManager)
        { 
            context.Database.EnsureCreated();
            // Look for any users.
            if (context.Books.Any())
            {
                return;   // DB has been seeded
            }

            Randomizer.Seed = new Random(8675309);

            await roleManager.CreateAsync(new IdentityRole("Librarian"));
            await roleManager.CreateAsync(new IdentityRole("Customer"));

            await context.AddRangeAsync(
                [
                    new Category { Name = "Non-Fiction" },
                    new Category { Name = "Science Fiction" },
                    new Category { Name = "Fantasy" },
                    new Category { Name = "Mystery" },
                    new Category { Name = "Romance" },
                    new Category { Name = "Horror" },
                    new Category { Name = "Thriller" },
                    new Category { Name = "Biography" },
                    new Category { Name = "History" },
                    new Category { Name = "Self-Help" }
                ]);
            context.SaveChanges();

            await context.AddRangeAsync(
                new Faker<Book>()
                    .RuleFor(b => b.CategoryId, f => f.Random.Number(1, 10))
                    .RuleFor(b => b.Title, f => f.Commerce.ProductName())
                    .RuleFor(b => b.Author, f => f.Name.FullName())
                    .RuleFor(b => b.Description, f => f.Commerce.ProductDescription())
                    .RuleFor(b => b.CoverImage, f => f.Image.PicsumUrl())
                    .RuleFor(b => b.Publisher, f => f.Company.CompanyName())
                    .RuleFor(b => b.PublicationDate, f => f.Date.Past())
                    .RuleFor(b => b.ISBN, f => f.Commerce.Ean13())
                    .RuleFor(b => b.PageCount, f => f.Random.Number(100, 500))
                    .Generate(200)
                    );
            context.SaveChanges();
        }
    }
}
