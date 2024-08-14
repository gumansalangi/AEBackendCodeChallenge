using AEBackendCodeChallenge.Controllers;
using AEBackendCodeChallenge.Models;
using AEBackendCodeChallenge.Services;
using AEBackendCodeChallenge.Models.Queryable;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AEBackendCodeChallenge.Test.TestController
{
    public class ShipsContollerTests
    {
        private readonly Mock<IShipService> _mockShipService;
        private readonly Mock<ILogger<ShipsController>> _mockLogger;
        private readonly ShipsController _shipsController;

        public ShipsContollerTests()
        {
            _mockShipService = new Mock<IShipService>();
            _mockLogger = new Mock<ILogger<ShipsController>>();
            _shipsController = new ShipsController(_mockShipService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetShips_Returns_OKResult_With_ListOfShips()
        {
            //Arrange
            var ships = new List<Ship>
            {
                new Ship { Id = 1, Name="TestShip1", ShipId="TestShipId11"},
                new Ship {Id=1, Name="TestShip2", ShipId="TestShipId12"}
            };
            _mockShipService.Setup(s => s.GetAllShipsAsync()).ReturnsAsync(ships);

            //Act
            var result = await _shipsController.GetShips();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<Ship>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetUnassignedShips_Returns_OKResult_With_ListOfUnassignedShips()
        {
            //Arrange
            var unAssignedShips = new List<Ship>
            {
                new Ship {Id=1, Name="TestShipName1"}
            };
            _mockShipService.Setup(s => s.GetUnassignedShipsAsync()).ReturnsAsync(unAssignedShips);

            //Act
            var result = await _shipsController.GetUnassignedShips();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<Ship>>(okResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task CreateShip_Return_CreatedResult_With_NewShips()
        {
            //Arrange
            var ship = new Ship { Id = 1, Name = "TestAddShip1" };
            _mockShipService.Setup(s => s.AddShipAsync(It.IsAny<Ship>())).ReturnsAsync(ship);

            //Act
            var result = await _shipsController.CreateShip(ship);

            //Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnValue = Assert.IsType<Ship>(createdAtActionResult.Value);
            Assert.Equal(ship.Id, returnValue.Id);

        }

        [Fact]
        public async Task UpdateVelocity_Returns_OkResult_With_UpdatedShips()
        {
            //Arrange
            var ships = new Ship { Id = 1, ShipId = "RGSL001", Name = "TestNameUpdateVelocityShip1", Velocity = 13 };
            _mockShipService.Setup(s=> s.UpdateShipVelocityAsync(ships.ShipId, It.IsAny<double>()))
                .ReturnsAsync(ships);

            //Act
            var updateVelocityQuery = new UpdateVelocityQuery
            {
                ShipId = ships.ShipId,
                Velocity= ships.Velocity
            };


            var result = await _shipsController.UpdateVelocity(updateVelocityQuery);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Ship>(okResult.Value);
            Assert.Equal(ships.Id, returnValue.Id);
            Assert.Equal(28, returnValue.Velocity);
        }

        [Fact]
        public async Task GetClosestPort_Returns_OkResult_With_ClosestPortDetails()
        {
            //Arrange
            var ships = new Ship { Id = 1, ShipId= "RGSL001", Name = "TestNameUpdateVelocityShip1", Velocity = 13 };
            _mockShipService.Setup(s => s.GetShipByIdAsync(ships.Id))
                .ReturnsAsync(ships);


            //Act
            var result = await _shipsController.GetClosestPort(ships.ShipId);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            dynamic returnValue = okResult.Value;
            Assert.NotNull(returnValue.ClosestPort);
            Assert.True(returnValue.EstimatedArrivalTime != string.Empty);
        }
    }
}
