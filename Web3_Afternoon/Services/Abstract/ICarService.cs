using Web3_Afternoon.Entities;

namespace Web3_Afternoon.Services.Abstract
{
    public interface ICarService
    {
        IQueryable<Car> Get();
        Car? Get(int id);
        bool Delete(Car car);
        Car Update(Car car);
        Car Add(Car car);
        int GetCarAge(Car car);
    }
}
