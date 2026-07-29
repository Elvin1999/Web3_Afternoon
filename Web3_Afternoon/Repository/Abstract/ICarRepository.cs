using Web3_Afternoon.Entities;

namespace Web3_Afternoon.Repository.Abstract
{
    public interface ICarRepository
    {
        IQueryable<Car> Get();
        Car? Get(int id);
        void Delete(Car car);
        Car Update(Car car);
        Car Add(Car car);
        bool SaveChanges();
    }
}
