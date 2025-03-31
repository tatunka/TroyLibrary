
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TroyLibrary.Data;
using TroyLibrary.Data.Models;
using TroyLibrary.Data.Repos;
using TroyLibrary.Data.Repos.Interfaces;
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
            var requirements = builder.Configuration.GetSection("PasswordRequirements"); 
            builder.Services.AddIdentity<TroyLibraryUser, IdentityRole>(o =>
            {
                o.Password.RequireDigit = requirements.GetValue<bool>("RequireDigit");
                o.Password.RequireLowercase = requirements.GetValue<bool>("RequireLowercase");
                o.Password.RequireUppercase = requirements.GetValue<bool>("RequireUppercase");
                o.Password.RequireNonAlphanumeric = requirements.GetValue<bool>("RequireNonAlphaNumeric");
                o.Password.RequiredLength = requirements.GetValue<int>("RequiredLength");
            })
                .AddEntityFrameworkStores<TroyLibraryContext>()
                .AddDefaultTokenProviders();

            // JWT Authentication
            var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]);
            builder.Services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.MapInboundClaims = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            builder.Services.AddAuthorization();

            // Add Services
            builder.Services.AddScoped<IBookService, BookService>();
            builder.Services.AddScoped<IReviewService, ReviewService>();
            builder.Services.AddScoped<ILookupService, LookupService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IBookRepo, BookRepo>();
            builder.Services.AddScoped<IReviewRepo, ReviewRepo>();
            builder.Services.AddScoped<ILookupRepo, LookupRepo>();

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

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
