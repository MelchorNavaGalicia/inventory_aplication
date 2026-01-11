using inventory_aplication.Infrastructure.Services.CurrentUser;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace inventory_aplication.Tests.Services
{
    public class CurrentUserServiceTests
    {
        [Fact]
        public void UserId_ShouldReturnId_WhenClaimExists()
        {
            // Arrange
            var userId = 42;
            var claims = new List<Claim> { new Claim("id", userId.ToString()) };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var principal = new ClaimsPrincipal(identity);

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(x => x.User).Returns(principal);

            var accessorMock = new Mock<IHttpContextAccessor>();
            accessorMock.Setup(x => x.HttpContext).Returns(httpContextMock.Object);

            var service = new CurrentUserService(accessorMock.Object);

            // Act
            var result = service.UserId;

            // Assert
            Assert.Equal(userId, result);
        }

        [Fact]
        public void UserId_ShouldThrowUnauthorized_WhenClaimMissing()
        {
            // Arrange
            var principal = new ClaimsPrincipal(new ClaimsIdentity());
            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(x => x.User).Returns(principal);

            var accessorMock = new Mock<IHttpContextAccessor>();
            accessorMock.Setup(x => x.HttpContext).Returns(httpContextMock.Object);

            var service = new CurrentUserService(accessorMock.Object);

            // Act & Assert
            Assert.Throws<UnauthorizedAccessException>(() => service.UserId);
        }

        [Fact]
        public void UserId_ShouldThrowUnauthorized_WhenHttpContextIsNull()
        {
            var accessorMock = new Mock<IHttpContextAccessor>();
            accessorMock.Setup(x => x.HttpContext).Returns((HttpContext)null);

            var service = new CurrentUserService(accessorMock.Object);

            Assert.Throws<UnauthorizedAccessException>(() => service.UserId);
        }
    }

}
