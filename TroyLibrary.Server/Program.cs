
using TroyLibrary.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using TroyLibrary.Data.Models;
using TroyLibrary.Repo.Interfaces;
using TroyLibrary.Data.Repos;
using TroyLibrary.Service.Interfaces;
using TroyLibrary.Service;

namespace TroyLibrary.Server
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<TroyLibraryContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("TroyLibraryContext"),
                b => b.MigrationsAssembly("TroyLibrary.Data")));

            //builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            // Add Identity
            builder.Services.AddAuthorization();
            builder.Services.AddIdentityApiEndpoints<TroyLibraryUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<TroyLibraryContext>();

            builder.Services.AddIdentityCore<TroyLibraryUser>(o =>
            {
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 6;
            });

            builder.Services.AddScoped<IBookService, BookService>();
            builder.Services.AddScoped<IBookRepo, BookRepo>();

            var app = builder.Build();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<TroyLibraryContext>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                await DbInitializer.InitializeAsync(context, roleManager);
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.MapIdentityApi<IdentityUser>();

            app.MapControllers();

            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
