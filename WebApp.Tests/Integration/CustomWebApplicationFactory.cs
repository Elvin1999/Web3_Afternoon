using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Web3_Afternoon.Data;
using Web3_Afternoon.Entities;

namespace WebApp.Tests.Integration
{
    public class CustomWebApplicationFactory
    : WebApplicationFactory<Program>
    {
        private SqliteConnection? _connection;

        protected override void ConfigureWebHost(
            IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {

                services.RemoveAll<
                    DbContextOptions<CarDBContext>>();


                // -----------------------------------------
                // 3. Create SQLite In-Memory database
                // -----------------------------------------

                _connection = new SqliteConnection(
                    "DataSource=:memory:");

                _connection.Open();


                services.AddDbContext<CarDBContext>(options =>
                {
                    options.UseSqlite(_connection);
                });


                // -----------------------------------------
                // 4. Create database
                // -----------------------------------------

                var serviceProvider =
                    services.BuildServiceProvider();

                using var scope =
                    serviceProvider.CreateScope();

                var db =
                    scope.ServiceProvider
                        .GetRequiredService<CarDBContext>();

                db.Database.EnsureCreated();


                // -----------------------------------------
                // 5. Seed test cars
                // -----------------------------------------

                SeedDatabase(db);
            });
        }


        private static void SeedDatabase(
            CarDBContext context)
        {
            if (context.Cars.Any())
            {
                return;
            }

            context.Cars.AddRange(

                new Car
                {
                    Id = 1,
                    Model = "X5",
                    Vendor = "BMW",
                    Engine = 3.0,
                    Year = 2022
                },

                new Car
                {
                    Id = 2,
                    Model = "A6",
                    Vendor = "Audi",
                    Engine = 2.0,
                    Year = 2021
                },

                new Car
                {
                    Id = 3,
                    Model = "C-Class",
                    Vendor = "Mercedes",
                    Engine = 2.0,
                    Year = 2023
                }

            );

            context.SaveChanges();
        }


        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                _connection?.Dispose();
            }
        }
    }
}
