using Microsoft.EntityFrameworkCore;
using Web3_Afternoon.Entities;

namespace Web3_Afternoon.Data
{
    public class CarDBContext:DbContext
    {
        public CarDBContext(DbContextOptions<CarDBContext> options)
            :base(options)
        {
            
        }

        public DbSet<Car> Cars { get; set; }

    }
}
