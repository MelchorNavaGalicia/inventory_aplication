using Xunit;
using Moq;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using inventory_aplication.Controllers;
using inventory_aplication.Application.Common.DTOs;
using inventory_aplication.Application.Features.Common.Results;
using inventory_aplication.Application.Features.Auth;
using System.Threading;
using System.Threading.Tasks;

namespace inventory_aplication.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new AuthController(_mediatorMock.Object);
        }

        #region REGISTER

        [Fact]
        public async Task Register_ReturnsOk_WhenRegistrationSucceeds()
        {
            // Arrange
            var dto = new RegisterDto
            {
                Name = "John Doe",
                Username = "johndoe",
                Password = "Password123!"
            };

            var expectedResult = Result<string>.Ok("Usuario registrado correctamente");

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<RegisterCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.Register(dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsType<Result<string>>(okResult.Value);

            Assert.True(value.Success);
            Assert.Equal(expectedResult.Data, value.Data);
        }

        #endregion

        #region LOGIN

        [Fact]
        public async Task Login_ReturnsOk_WhenLoginSucceeds()
        {
            // Arrange
            var dto = new LoginDto
            {
                Username = "johndoe",
                Password = "Password123!"
            };

            var expectedResult = Result<string>.Ok("JWT_TOKEN_123"); // simula el token

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<LoginCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.Login(dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsType<Result<string>>(okResult.Value);

            Assert.True(value.Success);
            Assert.Equal(expectedResult.Data, value.Data);
        }

        [Fact]
        public async Task Login_ReturnsFail_WhenLoginNotSuccessful()
        {
            // Arrange
            var dto = new LoginDto
            {
                Username = "wronguser",
                Password = "WrongPassword!"
            };

            // Simula que el login falla
            var expectedResult = Result<string>.Fail("Credenciales inválidas", ErrorCodes.InvalidCredentials);

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<LoginCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.Login(dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsType<Result<string>>(okResult.Value);

            // Validamos que el resultado es fallido
            Assert.False(value.Success);
            Assert.Equal("INVALID_CREDENTIALS", value.Code);
            Assert.Null(value.Data); // normalmente no hay token cuando falla
        }

        #endregion
    }
}
   

