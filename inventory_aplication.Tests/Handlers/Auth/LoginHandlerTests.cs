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
    public class LoginHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepo;
        private readonly Mock<IPasswordHasherService> _hasher;
        private readonly Mock<IJwtService> _jwt;
        private readonly LoginHandler _handler;

        public LoginHandlerTests()
        {
            _userRepo = new Mock<IUserRepository>();
            _hasher = new Mock<IPasswordHasherService>();
            _jwt = new Mock<IJwtService>();
            _handler = new LoginHandler(_jwt.Object, _hasher.Object, _userRepo.Object);
        }

        [Fact]
        public async Task Handle_ShouldFail_WhenUserNotFound()
        {
            _userRepo.Setup(r => r.GetByAsync("user")).ReturnsAsync((User)null);

            var command = new LoginCommand("user", "pass");
            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Equal(ErrorCodes.InvalidCredentials, result.Code);
            _hasher.Verify(h => h.VerifyPassword(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _jwt.Verify(j => j.GenerateToken(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldFail_WhenPasswordIncorrect()
        {
            var user = new User { Username = "user", Password = "hash" };
            _userRepo.Setup(r => r.GetByAsync("user")).ReturnsAsync(user);
            _hasher.Setup(h => h.VerifyPassword("wrong", "hash")).Returns(false);

            var command = new LoginCommand("user", "wrong");
            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Equal(ErrorCodes.InvalidCredentials, result.Code);
            _jwt.Verify(j => j.GenerateToken(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnToken_WhenCredentialsCorrect()
        {
            var user = new User { Username = "user", Password = "hash" };
            _userRepo.Setup(r => r.GetByAsync("user")).ReturnsAsync(user);
            _hasher.Setup(h => h.VerifyPassword("pass", "hash")).Returns(true);
            _jwt.Setup(j => j.GenerateToken(user)).Returns("token123");

            var command = new LoginCommand("user", "pass");
            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.Success);
            Assert.Equal("token123", result.Data);
        }
    }

}
