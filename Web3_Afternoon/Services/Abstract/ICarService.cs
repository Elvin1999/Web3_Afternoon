using Web3_Afternoon.Entities;
using Web3_Afternoon.Models;

namespace Web3_Afternoon.Services.Abstract
{
    public interface ICarService
    {
        Task<List<Car>> Get();
        Task<PagedResult<Car>> GetAll(int page, int pageSize);
        Task<Car?> Get(int id);
        Task<bool> Delete(Car car);
        Task<Car> Update(Car car);
        Task<Car> Add(Car car);
        int GetCarAge(Car car);
    }
}
