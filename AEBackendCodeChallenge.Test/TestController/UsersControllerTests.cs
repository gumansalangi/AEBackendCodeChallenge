using AEBackendCodeChallenge.Controllers;
using AEBackendCodeChallenge.Models;
using AEBackendCodeChallenge.Models.Queryable;
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
            var user = new List<User>
            {
                new User { Id = 1, Name = "TestName1" },
                new User { Id = 2, Name = "TestName2" }
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
            var user = new User { Id = 1, Name = "TestNameAddUSer" };
            _mockUserService.Setup(s=> s.AddUserAsync(It.IsAny<User>())).ReturnsAsync(user);

            //Act
            var result = await _usersController.CreateUser(user);

            //Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnValue = Assert.IsType<User>(createdAtActionResult.Value);
            Assert.Equal(user.Id, returnValue.Id);
        }

        [Fact]
        public async Task UpdateUserShips_Returns_OKResult_With_UpdatedUsers()
        {
            //Arrange
            var user = new User { Id = 1, Name = "TestUpdateName1" };
            _mockUserService.Setup(s=>s.UpdateUserShipsAsync(user.Id, "RGSL002")).ReturnsAsync(user);

            //Act
            var updateUserQuery = new UpdateUserQuery
            {
                UsersId = user.Id,
                ShipdId = "RGSL002"
            };

            var result = await _usersController.UpdateUserShips(updateUserQuery);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<User>(okResult.Value);
            Assert.Equal(user.Id, returnValue.Id);

        }
    }
}
