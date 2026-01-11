using inventory_aplication.Application.Common.Interfaces.Repositories;
using inventory_aplication.Application.Features.User.Commands.DeleteUser;
using inventory_application.Data.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace inventory_aplication.Tests.Handlers.UserTest
{
    public class DeleteUserHandlerTests
    {
        private readonly Mock<IUserRepository> _repoMock;
        private readonly DeleteUserHandler _handler;

        public DeleteUserHandlerTests()
        {
            _repoMock = new Mock<IUserRepository>();
            _handler = new DeleteUserHandler(_repoMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnFail_WhenUserNotFound()
        {
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((User)null);

            var command = new DeleteUserCommand(1);
            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Equal("El usuario no fue encontrado", result.Error);
            _repoMock.Verify(r => r.DeleteAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnOk_WhenUserDeleted()
        {
            var user = new User { Id = 2, Name = "Juan" ,Password="PaswordSecret"};
            _repoMock.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(user);

            var command = new DeleteUserCommand(2);
            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.Success);
            Assert.Equal("El usuario fue eliminado correctamente", result.Data);
            _repoMock.Verify(r => r.DeleteAsync(user), Times.Once);
        }
    }

}
