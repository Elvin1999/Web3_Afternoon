using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Web3_Afternoon.Entities;

namespace Web3_Afternoon.Data
{
    public class CarDBContext:IdentityDbContext<ApplicationUser>
    {
        public CarDBContext(DbContextOptions<CarDBContext> options)
            :base(options)
        {
            
        }

        public DbSet<Car> Cars { get; set; }

    }
}
