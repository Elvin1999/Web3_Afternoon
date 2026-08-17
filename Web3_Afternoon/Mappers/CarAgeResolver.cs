using AutoMapper;
using Web3_Afternoon.Dtos;
using Web3_Afternoon.Entities;
using Web3_Afternoon.Services.Abstract;

namespace Web3_Afternoon.Mappers
{
    public class CarAgeResolver : IValueResolver<Car, CarExtendDto, int>
    {
        private readonly ICarService _carService;

        public CarAgeResolver(ICarService carService)
        {
            _carService = carService;
        }

        public int Resolve(Car source, CarExtendDto destination, int destMember, ResolutionContext context)
        {
            return _carService.GetCarAge(source);
        }
    }
}
