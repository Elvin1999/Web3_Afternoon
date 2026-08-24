
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Threading.Tasks;
using Web3_Afternoon.Data;
using Web3_Afternoon.Entities;
using Web3_Afternoon.Formatters;
using Web3_Afternoon.Middlewares;
using Web3_Afternoon.Repository.Abstract;
using Web3_Afternoon.Repository.Concrete;
using Web3_Afternoon.Services;
using Web3_Afternoon.Services.Abstract;
using Web3_Afternoon.Services.Concrete;

namespace Web3_Afternoon
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers(options =>
            {
                options.OutputFormatters.Add(new CarVCardOutputFormatter());
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApplication3 API", Version = "v1" });

                // Swagger UI-a təhlükəsizlik növünü (JWT) tanıtmada istifadə olunur
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Zəhmət olmasa bura yalnız tokeninizi daxil edin (Başına Bearer yazmayın)."
                });

                // Bütün endpoint-lərə kilid (Authorize) ikonunun əlavə edilməsi
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
            });


            var connection = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<CarDBContext>(options => options.UseSqlServer(connection));

            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.LicenseKey = builder.Configuration["AutoMapper:LicenseKey"]!;
            }, typeof(Program).Assembly);


            //Identity
            builder.Services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<CarDBContext>();


            //Jwt Authentication

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],

                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
                        )
                };
            });

            builder.Services.AddAuthorization();
            //builder.Services.AddSingleton<ICalculateService, CalculateService>();
            //builder.Services.AddScoped<ICalculateService, CalculateService>();
            builder.Services.AddTransient<ICalculateService, CalculateService>();

            builder.Services.AddScoped<ICarRepository, CarRepository>();
            builder.Services.AddScoped<ICarService, CarService>();
            builder.Services.AddScoped<IAuthService,AuthService>();



            var app = builder.Build();


            using (var scope=app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                string[] roles =
                {
                    "User",
                    "Admin",
                    "Manager"
                };

                foreach (var role in roles)
                {
                    if(!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                var userManager=scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                var adminEmail = "admin1@test.com";

                var admin=await userManager.FindByEmailAsync(adminEmail);

                if (admin == null)
                {
                    admin = new ApplicationUser
                    {
                        Fullname = "System Admin",
                        UserName = adminEmail,
                        Email = adminEmail,
                        EmailConfirmed = true
                    };

                    await userManager.CreateAsync(admin,"Admin_123");

                    await userManager.AddToRoleAsync(admin, "Admin");

                }

            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseHttpsRedirection();

            app.UseMiddleware<RequestLoggingMiddleware>();


            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
