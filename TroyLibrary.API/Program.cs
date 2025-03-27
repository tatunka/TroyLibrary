
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TroyLibrary.Data;
using TroyLibrary.Data.Models;
using TroyLibrary.Data.Repos;
using TroyLibrary.Repo.Interfaces;
using TroyLibrary.Service;
using TroyLibrary.Service.Interfaces;

namespace TroyLibrary.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // set up cors
            var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowConfiguredOrigins",
                    policy => policy.WithOrigins(allowedOrigins)
                                    .AllowAnyMethod()
                                    .AllowAnyHeader());
            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<TroyLibraryContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("TroyLibraryContext"),
                b => b.MigrationsAssembly("TroyLibrary.Data")));

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

            // Add Services
            builder.Services.AddScoped<IBookService, BookService>();
            builder.Services.AddScoped<IBookRepo, BookRepo>();

            var app = builder.Build();

            app.UseCors("AllowConfiguredOrigins");

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

                using (var scope = app.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    var context = services.GetRequiredService<TroyLibraryContext>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    await DbInitializer.InitializeAsync(context, roleManager);
                }
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
