using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Web3_Afternoon.Data;
using Web3_Afternoon.Entities;
using Web3_Afternoon.Models;
using Web3_Afternoon.Repository.Abstract;

namespace Web3_Afternoon.Repository.Concrete
{
    public class CarRepository : ICarRepository
    {
        private readonly CarDBContext _context;

        public CarRepository(CarDBContext context)
        {
            _context = context;
        }

        public async Task<Car> Add(Car car)
        {
            var createdCar = (await _context.Cars.AddAsync(car)).Entity;
            return createdCar;
        }

        public async Task Delete(Car car)
        {
            await Task.Run(() =>
            {
                _context.Cars.Remove(car);
            });
        }

        public async Task<List<Car>> Get()
        {
            return await _context.Cars.ToListAsync();
        }

        public async Task<Car?> Get(int id)
        {
            return await _context.Cars.SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<PagedResult<Car>> GetAll(int page, int pageSize)
        {
            var query = _context.Cars;

            var totalCount = await query.CountAsync();

            var cars = await query
                .OrderBy(i => i.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Car>
            {
                items=cars,
                Page = page,
                TotalCount = totalCount,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalCount/(double)pageSize)
            };
                
                
        }

        public async Task<bool> SaveChanges()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<Car> Update(Car car)
        {
            var updatedCar = await Task.Run(() =>
            {
                return _context.Cars.Update(car).Entity;
            });
            return updatedCar;
        }
    }
}
