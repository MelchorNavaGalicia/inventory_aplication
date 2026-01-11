using Xunit;
using Moq;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using inventory_aplication.Controllers;
using inventory_aplication.Application.Features.Common.Results;
using inventory_aplication.Application.Features.User.Queries.GetAllUsers;
using inventory_aplication.Application.Features.User.Commands.DeleteUser;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Text;
using inventory_aplication.Application.Common.DTOs;

namespace inventory_aplication.Tests.Controllers
{

    public class UserControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new UserController(_mediatorMock.Object);
        }

        #region GET All Users

        [Fact]
        public async Task GetAllUsers_ReturnsOk_WithPagedUsers()
        {
            // Arrange
            var expectedResult = Result<PagedResult<UserDto>>.Ok(
                new PagedResult<UserDto>(
                    totalItems: 2,
                    pageNumber: 1,
                    pageSize: 10,
                    items: new List<UserDto>
                    {
                    new UserDto { Id = 1, Username = "johndoe", Name = "John Doe" },
                    new UserDto { Id = 2, Username = "janedoe", Name = "Jane Doe" }
                    }
                )
            );

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetAllUsersQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.GetAllUsers(1, 10);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsType<Result<PagedResult<UserDto>>>(okResult.Value);

            Assert.True(value.Success);
            Assert.Equal(2, value.Data.TotalItems);
            Assert.Equal("johndoe", value.Data.Items[0].Username);
        }

        #endregion

        #region DELETE User - Positive

        [Fact]
        public async Task DeleteUser_ReturnsOk_WhenUserDeleted()
        {
            // Arrange
            int userId = 1;

            var expectedResult = Result<string>.Ok("Usuario eliminado correctamente");

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<DeleteUserCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.DeleteUser(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsType<Result<string>>(okResult.Value);

            Assert.True(value.Success);
            Assert.Equal("Usuario eliminado correctamente", value.Data);
        }

        #endregion

        #region DELETE User - Negative

        [Fact]
        public async Task DeleteUser_ReturnsFail_WhenUserDoesNotExist()
        {
            // Arrange
            int userId = 99;

            var expectedResult = Result<string>.Fail("Usuario no encontrado",ErrorCodes.NoFound);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<DeleteUserCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.DeleteUser(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsType<Result<string>>(okResult.Value);

            Assert.False(value.Success);
            Assert.Equal(ErrorCodes.NoFound, value.Code);
        }

        #endregion
    }

}
