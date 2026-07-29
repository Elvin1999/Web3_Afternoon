using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web3_Afternoon.Dtos;
using Web3_Afternoon.Entities;
using Web3_Afternoon.Services.Abstract;

namespace Web3_Afternoon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly ICarService _carService;

        public CarsController(ICarService carService)
        {
            _carService = carService;
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
        public ActionResult<IEnumerable<CarDto>> Get()
        {
            var cars = _carService.Get()
                .Select(c => new CarDto { Id = c.Id, Model = c.Model, Vendor = c.Vendor });

            return Ok(cars);
        }

        [HttpGet("{id:int}")]
        public ActionResult<CarDto> Get(int id)
        {
            var car = _carService.Get(id);
            if (car == null) return NotFound();
            var result = new CarDto
            {
                Id = car.Id,
                Model = car.Model,
                Vendor = car.Vendor
            };
            return Ok(result);
        }

        [HttpGet("{id:int}/extend")]
        public ActionResult<CarExtendDto> GetExtend(int id)
        {
            var car = _carService.Get(id);
            if (car == null) return NotFound();
            var result = new CarExtendDto
            {
                Id = car.Id,
                Model = car.Model,
                Vendor = car.Vendor,
                Engine=car.Engine,
                Year=car.Year,
                Age=_carService.GetCarAge(car)
            };
            return Ok(result);
        }

        [HttpPost]
        public ActionResult<CarExtendDto> Post([FromBody] CarAddDto dto)
        {
            try
            {
                var car = new Car
                {
                    Engine = dto.Engine,
                    Model = dto.Model,
                    Vendor = dto.Vendor,
                    Year = dto.Year,
                };
                var createdCar = _carService.Add(car);
               
                var returnDto = new CarExtendDto
                {
                     Id=createdCar.Id,
                     Model=createdCar.Model,
                     Vendor=createdCar.Vendor,
                     Year=createdCar.Year,
                     Engine= createdCar.Engine,
                };

                return CreatedAtAction(nameof(Get),
                    new { id = returnDto.Id },
                    returnDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, [FromBody] Car car)
        {
            try
            {
                var exist = _carService.Get(id);

                if (exist == null) return NotFound();

                exist.Vendor = car.Vendor;
                exist.Model = car.Model;
                exist.Year = car.Year;

                var updated = _carService.Update(exist);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
