using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web3_Afternoon.Services;

namespace Web3_Afternoon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestsController : ControllerBase
    {
        //private readonly ICalculateService _calculateService;
        //public TestsController(ICalculateService calculateService)
        //{
        //    _calculateService = calculateService;
        //    Console.WriteLine("Constructor called");
        //}

        //[HttpGet]
        //public IActionResult Get()
        //{
        //    var result = _calculateService.Calculate(10, 20);
        //    return Ok(result);
        //}

        private readonly ICalculateService _calculateService1;
        private readonly ICalculateService _calculateService2;

        public TestsController(ICalculateService calculateService1, ICalculateService calculateService2)
        {
            _calculateService1 = calculateService1;
            _calculateService2 = calculateService2;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var result1 = _calculateService1.Calculate(10, 20);
            var result2 = _calculateService2.Calculate(10, 20);
            return Ok(result1+result2);
        }
    }
}
