using Web3_Afternoon.Data;
using Web3_Afternoon.Entities;
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

        public Car Add(Car car)
        {
            var createdCar = _context.Cars.Add(car).Entity;
            return createdCar;
        }

        public void Delete(Car car)
        {
            _context.Cars.Remove(car);
        }

        public IQueryable<Car> Get()
        {
            return _context.Cars;
        }

        public Car? Get(int id)
        {
            return _context.Cars.SingleOrDefault(c => c.Id == id);
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() > 0;
        }

        public Car Update(Car car)
        {
            var updatedCar=_context.Cars.Update(car).Entity;
            return updatedCar;
        }
    }
}
