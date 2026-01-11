using Xunit;
using Moq;
using Microsoft.Extensions.Configuration;
using inventory_application.Data.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace inventory_aplication.Tests.Services
{
    public class JwtServiceTests
    {
        [Fact]
        public void GenerateToken_ReturnsValidJwtToken_WithCorrectClaims()
        {
            // Arrange
            var inMemorySettings = new Dictionary<string, string> {
            {"Jwt:Key", "SuperSecretTestKey1234567890ThisSuperSecretKeyIsSuperSecret"},
            {"Jwt:Issuer", "TestIssuer"},
            {"Jwt:Audience", "TestAudience"},
            {"Jwt:ExpireMinutes", "60"}
        };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            var service = new JwtService(configuration);

            var user = new User { Id = 1, Name = "John Doe" , Password="PwrdTest"};

            // Act
            var tokenString = service.GenerateToken(user);

            // Assert
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(tokenString);

            Assert.Equal("TestIssuer", token.Issuer);
            Assert.Equal("TestAudience", token.Audiences.First());

            var nameClaim = token.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
            var idClaim = token.Claims.FirstOrDefault(c => c.Type == "id")?.Value;

            Assert.Equal("John Doe", nameClaim);
            Assert.Equal("1", idClaim);

            Assert.True(token.ValidTo > DateTime.UtcNow);
        }
    }

}
