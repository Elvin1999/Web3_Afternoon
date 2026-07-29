using Web3_Afternoon.Entities;
using Web3_Afternoon.Repository.Abstract;
using Web3_Afternoon.Services.Abstract;

namespace Web3_Afternoon.Services.Concrete
{
    public class CarService : ICarService
    {
        private readonly ICarRepository _carRepository;

        public CarService(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }

        public Car Add(Car car)
        {
            var result=_carRepository.Add(car);
            _carRepository.SaveChanges();
            return result;
        }

        public bool Delete(Car car)
        {
            _carRepository.Delete(car);
            return _carRepository.SaveChanges();
        }

        public IQueryable<Car> Get()
        {
            return _carRepository.Get();
        }

        public Car? Get(int id)
        {
            return _carRepository.Get(id);
        }

        public int GetCarAge(Car car)
        {
            var difference = DateTime.Now.Year - car.Year;
            return difference > 0 ? difference : 0;
        }

        public Car Update(Car car)
        {
            var result=_carRepository.Update(car);
            _carRepository.SaveChanges();
            return result;
        }
    }
}
