
using Microsoft.EntityFrameworkCore;
using Web3_Afternoon.Data;
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
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers(options =>
            {
                options.OutputFormatters.Add(new CarVCardOutputFormatter());
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            var connection = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<CarDBContext>(options => options.UseSqlServer(connection));

            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.LicenseKey = builder.Configuration["AutoMapper:LicenseKey"]!;
            }, typeof(Program).Assembly);

            //builder.Services.AddSingleton<ICalculateService, CalculateService>();
            //builder.Services.AddScoped<ICalculateService, CalculateService>();
            builder.Services.AddTransient<ICalculateService, CalculateService>();

            builder.Services.AddScoped<ICarRepository, CarRepository>();
            builder.Services.AddScoped<ICarService, CarService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseHttpsRedirection();

            app.UseMiddleware<RequestLoggingMiddleware>();


            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
