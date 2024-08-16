using AEBackendCodeChallenge.Controllers;
using AEBackendCodeChallenge.Models;
using AEBackendCodeChallenge.Models.Dto;
using AEBackendCodeChallenge.Services;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AEBackendCodeChallenge.Test.TestController
{
    public class UsersControllerTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly Mock<ILogger<UsersController>> _mockLogger;
        private readonly UsersController _usersController;

        public UsersControllerTests()
        {
            _mockUserService = new Mock<IUserService>();
            _mockLogger = new Mock<ILogger<UsersController>>();
            _usersController = new UsersController(_mockUserService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAllUsers_Returns_OKResult_With_ListOfUsers()
        {
            //Arrange
            var user = new List<UserWithShipsDto>
            {
                new UserWithShipsDto { UserId = 1, UserName = "TestName1" },
                new UserWithShipsDto { UserId = 2, UserName = "TestName2" }
            };
            //test the user service to load all user
            _mockUserService.Setup(s => s.GetAllUsersAsync()).ReturnsAsync(user);

            //Act
            var result = await _usersController.GetUsers();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<User>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task CreateUser_Returns_CreatedResult_With_NewUser()
        {
            //Arrange
            var user = new UserWithShipsDto { UserId = 1, UserName = "TestNameAddUSer", Role = "Admin", Ships = [] };
            _mockUserService.Setup(s=> s.AddUserAsync(It.IsAny<AddUserDto>())).ReturnsAsync(user);
            
            //Act
            var userDto = new AddUserDto { UserId = 1, UserName = "TestNameAddUSer", Role = "Admin", Ships = [] };
            var result = await _usersController.CreateUser(userDto);

            //Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnValue = Assert.IsType<User>(createdAtActionResult.Value);
            Assert.Equal(user.UserId, returnValue.Id);
        }

        [Fact]
        public async Task UpdateUserShips_Returns_OKResult_With_UpdatedUsers()
        {
            //Arrange
            AssignShipToUserDto assignShipToUser = new AssignShipToUserDto();
            assignShipToUser.UsersId = 1;
            assignShipToUser.ShipIds = new List<int> { 1,2 };

            var user = new User { Id = 1, Name = "TestUpdateName1" };
            var returnUserWithDto = new UserWithShipsDto();
            _mockUserService.Setup(s=>s.UpdateUserShipsAsync(assignShipToUser)).ReturnsAsync(returnUserWithDto);

            //Act
            AssignShipToUserDto assignShipToUserAct = new AssignShipToUserDto();
            assignShipToUserAct.UsersId = 1;
            assignShipToUserAct.ShipIds = new List<int> { 1, 2 };



            var result = await _usersController.AssignUserToShips(assignShipToUserAct);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<UserWithShipsDto>(okResult.Value);
            Assert.Equal(user.Id, returnValue.UserId);
            Assert.Equal(user.Name, returnValue.UserName);
        }
    }
}
