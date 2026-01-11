using inventory_aplication.Application.Common.Interfaces.Auth;
using inventory_aplication.Application.Common.Interfaces.Repositories;
using inventory_aplication.Application.Features.Auth;
using inventory_aplication.Application.Features.Common.Results;
using inventory_application.Data.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace inventory_aplication.Tests.Handlers.Auth
{
    public class RegisterHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepo;
        private readonly Mock<IPasswordHasherService> _hasher;
        private readonly Mock<IJwtService> _jwt;
        private readonly RegisterHandler _handler;

        public RegisterHandlerTests()
        {
            _userRepo = new Mock<IUserRepository>();
            _hasher = new Mock<IPasswordHasherService>();
            _jwt = new Mock<IJwtService>();
            _handler = new RegisterHandler(_jwt.Object, _hasher.Object, _userRepo.Object);
        }

        [Fact]
        public async Task Handle_ShouldFail_WhenUsernameExists()
        {
            _userRepo.Setup(r => r.ExistsByNameAsync("user")).ReturnsAsync(true);

            var command = new RegisterCommand("User Name", "user", "pass");
            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Equal(ErrorCodes.ExistingUser, result.Code);
            _hasher.Verify(h => h.HashPassword(It.IsAny<string>()), Times.Never);
            _jwt.Verify(j => j.GenerateToken(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnToken_WhenUserCreated()
        {
            _userRepo.Setup(r => r.ExistsByNameAsync("user")).ReturnsAsync(false);
            _hasher.Setup(h => h.HashPassword("pass")).Returns("hash123");
            _jwt.Setup(j => j.GenerateToken(It.IsAny<User>())).Returns("token123");

            User? addedUser = null;
            _userRepo.Setup(r => r.AddAsync(It.IsAny<User>()))
                     .Callback<User>(u => addedUser = u)
                     .Returns(Task.CompletedTask);

            var command = new RegisterCommand("User Name", "user", "pass");
            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.Success);
            Assert.Equal("token123", result.Data);
            Assert.NotNull(addedUser);
            Assert.Equal("User Name", addedUser.Name);
            Assert.Equal("user", addedUser.Username);
            Assert.Equal("hash123", addedUser.Password);
        }
    }

}
