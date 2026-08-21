using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web3_Afternoon.Dtos;
using Web3_Afternoon.Entities;
using Web3_Afternoon.Models;
using Web3_Afternoon.Services.Abstract;

namespace Web3_Afternoon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CarsController : ControllerBase
    {
        private readonly ICarService _carService;
        private readonly IMapper _mapper;

        public CarsController(ICarService carService, IMapper mapper)
        {
            _carService = carService;
            _mapper = mapper;
        }

        //[HttpGet]
        //public ActionResult<IEnumerable<Car>> Get()
        //{
        //    var cars = _carService.Get();
        //    return Ok(cars);
        //}

        //[HttpGet("{id:int}")]
        //public ActionResult<Car> Get(int id)
        //{
        //    var car = _carService.Get(id);
        //    if (car == null) return NotFound();
        //    return Ok(car);
        //}

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CarDto>>> Get()
        {
            //var cars = _carService.Get()
            //    .Select(c => new CarDto { Id = c.Id, Model = c.Model, Vendor = c.Vendor });
            var cars = await _carService.Get();
            var returnDto = _mapper.Map<IEnumerable<CarDto>>(cars);

            return Ok(returnDto);
        }

        [HttpGet("partial")]
        public async Task<ActionResult<PagedResult<Car>>> GetAll(int page=1,int pageSize = 10)
        {
            var carsFromService = await _carService.GetAll(page, pageSize);
            return Ok(carsFromService);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CarDto>> Get(int id)
        {
            var car = await _carService.Get(id);
            if (car == null) return NotFound();
            //var result = new CarDto
            //{
            //    Id = car.Id,
            //    Model = car.Model,
            //    Vendor = car.Vendor
            //};
            var result = _mapper.Map<CarDto>(car);
            return Ok(result);
        }

        [HttpGet("{id:int}/extend")]
        public async Task<ActionResult<CarExtendDto>> GetExtend(int id)
        {
            var car = await _carService.Get(id);
            if (car == null) return NotFound();
            //var result = new CarExtendDto
            //{
            //    Id = car.Id,
            //    Model = car.Model,
            //    Vendor = car.Vendor,
            //    Engine=car.Engine,
            //    Year=car.Year,
            //    Age=_carService.GetCarAge(car)
            //};

            var result = _mapper.Map<CarExtendDto>(car);

            return Ok(result);
        }

        [HttpGet("extend")]
        public async Task<ActionResult<IEnumerable<CarExtendDto>>> GetExtendAll()
        {
            var cars = await _carService.Get();

            var result = _mapper.Map<IEnumerable<CarExtendDto>>(cars);

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<CarExtendDto>> Post([FromBody] CarAddDto dto)
        {
            //try
            //{
                //var car = new Car
                //{
                //    Engine = dto.Engine,
                //    Model = dto.Model,
                //    Vendor = dto.Vendor,
                //    Year = dto.Year,
                //};

                var car = _mapper.Map<Car>(dto);

                var createdCar = await _carService.Add(car);

                //var returnDto = new CarExtendDto
                //{
                //     Id=createdCar.Id,
                //     Model=createdCar.Model,
                //     Vendor=createdCar.Vendor,
                //     Year=createdCar.Year,
                //     Engine= createdCar.Engine,
                //};

                var returnDto = _mapper.Map<CarExtendDto>(createdCar);

                return CreatedAtAction(nameof(Get),
                    new { id = returnDto.Id },
                    returnDto);
            //}
            //catch (Exception ex)
            //{
            //    return BadRequest(ex.Message);
            //}
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] Car car)
        {
            try
            {
                var exist = await _carService.Get(id);

                if (exist == null) return NotFound();

                exist.Vendor = car.Vendor;
                exist.Model = car.Model;
                exist.Year = car.Year;

                var updated = await _carService.Update(exist);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
