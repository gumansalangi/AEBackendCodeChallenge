using AEBackendCodeChallenge.Controllers;
using AEBackendCodeChallenge.Models;
using AEBackendCodeChallenge.Services;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AEBackendCodeChallenge.Models.Dto;

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
            List<ShipWithUsersDto> ships = new List<ShipWithUsersDto>();
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
            List<ShipWithUsersDto> unAssignedShips = new List<ShipWithUsersDto>();
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
            var ship = new ShipWithUsersDto { ShipId = 1, Name = "TestAddShip1" };
            _mockShipService.Setup(s => s.AddShipAsync(It.IsAny<AddShipDto>())).ReturnsAsync(ship);

            //Act
            var shipDto = new AddShipDto { ShipId = "1", Name = "TestAddShip1" };
            var result = await _shipsController.CreateShip(shipDto);

            //Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnValue = Assert.IsType<Ship>(createdAtActionResult.Value);
            Assert.Equal(ship.Name, returnValue.Name);

        }

        [Fact]
        public async Task UpdateVelocity_Returns_OkResult_With_UpdatedShips()
        {
            //Arrange
            var ships = new ShipWithUsersDto { ShipId = 1, ShipCode = "RGSL001", Name = "TestNameUpdateVelocityShip1", Velocity = 28 };
            _mockShipService.Setup(s => s.UpdateShipVelocityAsync(ships.ShipId, It.IsAny<double>()))
                .ReturnsAsync(ships);

            //Act
            var updateVelocityQuery = new UpdateShipVelocityDto
            {
                ShipId = ships.ShipId,
                Velocity = ships.Velocity
            };


            var result = await _shipsController.UpdateVelocity(updateVelocityQuery);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<Ship>(okResult.Value);
            Assert.Equal(ships.ShipId, returnValue.Id);
            Assert.Equal(28, returnValue.Velocity);
        }

        [Fact]
        public async Task GetClosestPort_Returns_OkResult_With_ClosestPortDetails()
        {
            //Arrange
            var ships = new Ship { Id = 1, ShipCode = "RGSL001", Name = "TestNameUpdateVelocityShip1", Velocity = 13 };
            _mockShipService.Setup(s => s.GetShipByIdAsync(ships.Id))
                .ReturnsAsync(ships);


            //Act
            var getClosestPortDto = new GetClosestPortDto { id = 1 };
            var result = await _shipsController.GetClosestPort(getClosestPortDto);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            dynamic returnValue = okResult.Value;
            Assert.NotNull(returnValue.ClosestPort);
            Assert.True(returnValue.EstimatedArrivalTime != string.Empty);
        }
    }
}
