using AutoMapper;
using Web3_Afternoon.Dtos;
using Web3_Afternoon.Entities;

namespace Web3_Afternoon.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Car, CarDto>();
            //CreateMap<Car, CarDto>().ReverseMap();
            CreateMap<CarAddDto, Car>();

            //CreateMap<Car, CarExtendDto>()
            //    .AfterMap((src, dest) =>
            //    {
            //        var difference = DateTime.Now.Year - src.Year;
            //        dest.Age = difference > 0 ? difference : 0;
            //    });

            CreateMap<Car, CarExtendDto>()
                .ForMember(d => d.Age,
                opt => opt.MapFrom<CarAgeResolver>());
        }
    }
}
