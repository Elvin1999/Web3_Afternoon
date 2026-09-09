using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web3_Afternoon.Controllers;
using Web3_Afternoon.Dtos;
using Web3_Afternoon.Entities;
using Web3_Afternoon.Services.Abstract;

namespace WebApp.Tests.Controllers
{
    [TestFixture]
    public class CarsControllerTests
    {
        private Mock<ICarService> _carServiceMock = null!;
        private Mock<IMapper> _mapperMock = null!;
        private CarsController _controller = null!;

        [SetUp]
        public void SetUp()
        {
            _carServiceMock=new Mock<ICarService>();
            _mapperMock=new Mock<IMapper>();

            _controller = new CarsController(
                _carServiceMock.Object,
                _mapperMock.Object);

        }

        [Test]
        public async Task Get_Should_Return_Ok_When_Car_Exists()
        {

            // Arrange
            var car = new Car
            {
                Id = 1,
                Model = "X5",
                Vendor = "BMW",
                Engine = 3.0,
                Year = 2024
            };

            var carDto = new CarDto();

            _carServiceMock
                .Setup(x=>x.Get(1))
                .ReturnsAsync(car);

            _mapperMock
                .Setup(x => x.Map<CarDto>(car))
                .Returns(carDto);

            // Act
            var result = await _controller.Get(1);

            // Assert

            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());

            var okResult = (OkObjectResult)result.Result!;

            Assert.That(okResult.Value,Is.EqualTo(carDto));

        }

        [Test]
        public async Task Get_Should_Return_NotFound_When_Car_Does_Not_Exist()
        {
            // Arrange

            _carServiceMock
                .Setup(x => x.Get(999))
                .ReturnsAsync((Car?)null);

            // Act
            var result=await _controller.Get(999);

            // Assert
            Assert.That(
                result.Result,
                Is.TypeOf<NotFoundResult>());
        }


        [Test]
        public async Task Delete_Should_Call_Delete_Service_When_Car_Exists()
        {
            //Arrange
            var car = new Car
            {
                Id = 1,
                Model = "X5",
                Vendor = "BMW",
                Engine = 3.0,
                Year = 2022
            };

            _carServiceMock.
                Setup(x => x.Get(1))
                .ReturnsAsync(car);

            _carServiceMock.
                Setup(x => x.Delete(car))
                .ReturnsAsync(true);

            //Act

            await _controller.Delete(1);

            //Assert
            _carServiceMock.Verify(
                x => x.Delete(car),
                Times.Once);

        }

        [Test]
        public async Task Post_Should_Return_CreatedAtAction_When_Car_Is_Created()
        {
            // Arrange

            var dto = new CarAddDto();

            var car = new Car
            {
                Id = 1,
                Model = "X5",
                Vendor = "BMW",
                Engine = 3,
                Year = 2025
            };


            _mapperMock
                .Setup(x=>x.Map<Car>(dto))
                .Returns(car);

            _carServiceMock
                .Setup(x => x.Add(car))
                .ReturnsAsync(car);

            // Act

            var result = await _controller.Post(dto);

            // Assert

            Assert.That(result, Is.TypeOf<CreatedAtActionResult>());

            var createdResult = (CreatedAtActionResult)result;

            Assert.That(createdResult.ActionName,
                Is.EqualTo(nameof(CarsController.Get)));

            Assert.That(
                createdResult.RouteValues!["id"],
                Is.EqualTo(1));

            Assert.That(createdResult.Value,
                Is.EqualTo(car));


        }
    }
}
