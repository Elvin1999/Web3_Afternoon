using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web3_Afternoon.Entities;
using Web3_Afternoon.Repository.Abstract;
using Web3_Afternoon.Services.Concrete;

namespace WebApp.Tests.Services
{
    public class CarServiceTests
    {
        private Mock<ICarRepository> _carRepositoryMock = null!;
        private CarService _carService = null!;

        [SetUp]
        public void SetUp()
        {
            _carRepositoryMock = new Mock<ICarRepository>();
            _carService = new CarService(_carRepositoryMock.Object);
        }

        [Test]
        public async Task Get_Should_Return_Car_When_Car_Exists()
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

            _carRepositoryMock
                .Setup(x => x.Get(1))
                .ReturnsAsync(car);

            // Act

            var result = await _carService.Get(1);

            // Assert

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.Model, Is.EqualTo("X5"));
            Assert.That(result.Vendor, Is.EqualTo("BMW"));


        }

        [Test]
        public async Task Get_Should_Return_Null_When_Car_Does_Not_Exists()
        {
            // Arrange
            _carRepositoryMock
                .Setup(x => x.Get(999))
                .ReturnsAsync((Car?)null);

            // Act
            var result = await _carService.Get(999);

            //Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void Get_Should_Throw_When_Repository_Throws_Exception()
        {
            // Arrange
            _carRepositoryMock
                .Setup(x => x.Get(1))
                .ThrowsAsync(new Exception("Database Error"));

            //Act and Assert
            Assert.ThrowsAsync<Exception>(
                async () => await _carService.Get(1));
        }

        [Test]
        public async Task Get_Should_Return_All_Cars()
        {
            // Arrange
            var cars = new List<Car>
                {
                     new Car
                    {
                        Id = 1,
                        Model = "X5",
                        Vendor = "BMW",
                        Engine = 3.0,
                        Year = 2022
                    },
                    new Car
                    {
                        Id = 2,
                        Model = "A6",
                        Vendor = "Audi",
                        Engine = 2.0,
                        Year = 2021
                    }
                };

            _carRepositoryMock
                .Setup(x => x.Get())
                .ReturnsAsync(cars);

            // Act
            var result = await _carService.Get();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].Vendor, Is.EqualTo("BMW"));
            Assert.That(result[1].Vendor, Is.EqualTo("Audi"));
        }

        [Test]
        public async Task Add_Should_Call_Repository_And_SaveChanges()
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

            _carRepositoryMock
                .Setup(x=>x.Add(car))
                .ReturnsAsync(car);

            _carRepositoryMock
                .Setup(x => x.SaveChanges())
                .ReturnsAsync(true);

            // Act
            var result=await _carService.Add(car);

            //Assert
            Assert.That(result, Is.EqualTo(car));

            _carRepositoryMock.Verify(
                x => x.Add(car),
                Times.Once);

            _carRepositoryMock.Verify(
                x => x.SaveChanges(),
                Times.Once);

        }

        [Test]
        public async Task Delete_Should_Call_Delete_And_SaveChanges()
        {
            // Arrange
            var car = new Car
            {
                Id = 1,
                Model = "X5",
                Vendor = "BMW",
                Engine = 3.0,
                Year = 2022
            };

            _carRepositoryMock
                .Setup(x => x.Delete(car))
                .Returns(Task.CompletedTask);

            _carRepositoryMock
                .Setup(x => x.SaveChanges())
                .ReturnsAsync(true);

            // Act
            var result = await _carService.Delete(car);

            // Assert
            Assert.That(result, Is.True);

            _carRepositoryMock.Verify(
                x => x.Delete(car),
                Times.Once);

            _carRepositoryMock.Verify(
                x => x.SaveChanges(),
                Times.Once);

        }

        [TestCase(2020)]
        [TestCase(2022)]
        [TestCase(2040)]
        public void GetCarAge_Should_Not_Return_Negative_Age(int year)
        {
            // Arrange
            var car = new Car
            {
                Year = year
            };

            // Act
            var result = _carService.GetCarAge(car);

            //Assert

            Assert.That(result,Is.GreaterThanOrEqualTo(0));
        }

    }
}
