using inventory_aplication.Infrastructure.Persistence;
using inventory_aplication.Infrastructure.Persistence.Repositories;
using inventory_application.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace inventory_aplication.Tests.Repositories
{
    public class UserRepositoryTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task AddAsync_ShouldSaveUser()
        {
            var context = GetDbContext();
            var repo = new UserRepository(context);

            var user = new User { Id = 1, Name = "TestName", Username="TestUserName", Password = "TestPassword" };

            await repo.AddAsync(user);

            var savedUser = await context.Users.FindAsync(1);
            Assert.NotNull(savedUser);
            Assert.Equal("TestUserName", savedUser.Username);
        }
    }

}
