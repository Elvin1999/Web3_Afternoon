using Web3_Afternoon.Entities;
using Web3_Afternoon.Models;
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

        public async Task<Car> Add(Car car)
        {
            var result=await _carRepository.Add(car);
            await _carRepository.SaveChanges();
            return result;
        }

        public async Task<bool> Delete(Car car)
        {
            await _carRepository.Delete(car);
            return await _carRepository.SaveChanges();
        }

        public async Task<List<Car>> Get()
        {
            return await _carRepository.Get();
        }

        public async Task<Car?> Get(int id)
        {
            return await _carRepository.Get(id);
        }

        public async Task<PagedResult<Car>> GetAll(int page, int pageSize)
        {
            return await _carRepository.GetAll(page, pageSize);
        }

        public int GetCarAge(Car car)
        {
            var difference = DateTime.Now.Year - car.Year;
            return difference > 0 ? difference : 0;
        }

        public async Task<Car> Update(Car car)
        {
            var result=await _carRepository.Update(car);
            await _carRepository.SaveChanges();
            return result;
        }
    }
}
