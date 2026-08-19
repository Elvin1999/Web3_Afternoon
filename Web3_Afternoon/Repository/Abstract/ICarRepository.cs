using Web3_Afternoon.Entities;
using Web3_Afternoon.Models;

namespace Web3_Afternoon.Repository.Abstract
{
    public interface ICarRepository
    {
        Task<List<Car>> Get();
        Task<Car?> Get(int id);
        Task Delete(Car car);
        Task<Car> Update(Car car);
        Task<Car> Add(Car car);
        Task<bool> SaveChanges();
        Task<PagedResult<Car>> GetAll(int page, int pageSize);
    }
}
