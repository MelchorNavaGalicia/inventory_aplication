using inventory_aplication.Infrastructure.Services.Auth;
using System;
using System.Collections.Generic;
using System.Text;

namespace inventory_aplication.Tests.Services
{
    public class PasswordHasherServiceTests
    {
        private readonly PasswordHasherService _hasher;

        public PasswordHasherServiceTests()
        {
            _hasher = new PasswordHasherService();
        }

        [Fact]
        public void HashPassword_ShouldReturnDifferentHashes_ForSamePassword()
        {
            var password = "Password123!";
            var hash1 = _hasher.HashPassword(password);
            var hash2 = _hasher.HashPassword(password);

            Assert.NotEqual(hash1, hash2);
        }

        [Fact]
        public void VerifyPassword_ShouldReturnTrue_ForCorrectPassword()
        {
            var password = "Password123!";
            var hash = _hasher.HashPassword(password);

            var result = _hasher.VerifyPassword(password, hash);

            Assert.True(result);
        }

        [Fact]
        public void VerifyPassword_ShouldReturnFalse_ForIncorrectPassword()
        {
            var password = "Password123!";
            var hash = _hasher.HashPassword(password);

            var result = _hasher.VerifyPassword("WrongPassword", hash);

            Assert.False(result);
        }

        [Fact]
        public void VerifyPassword_ShouldReturnFalse_ForMalformedHash()
        {
            var malformedHash = "invalid.hash.format";

            var result = _hasher.VerifyPassword("Password123!", malformedHash);

            Assert.False(result);
        }
    }

}
