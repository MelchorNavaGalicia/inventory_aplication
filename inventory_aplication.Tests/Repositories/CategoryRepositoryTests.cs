using inventory_aplication.Application.Common.DTOs.Category;
using inventory_aplication.Application.Features.Common.Results;
using inventory_aplication.Infrastructure.Persistence;
using inventory_aplication.Infrastructure.Persistence.Repositories;
using inventory_application.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using System;
using System.Collections.Generic;
using System.Text;

namespace inventory_aplication.Tests.Repositories
{

    public class CategoryRepositoryTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "CategoryDb_" + Guid.NewGuid())
                .Options;

            var context = new AppDbContext(options);

            context.Categories.AddRange(
                new Category { Id = 1, Name = "Electronics", Description = "Electro stuff", IsActive = true },
                new Category { Id = 2, Name = "Books", Description = "Reading materials", IsActive = true },
                new Category { Id = 3, Name = "Inactive", Description = "Not active", IsActive = false }
            );

            context.SaveChanges();
            return context;
        }

        [Fact]
        public async Task ExistsByNameAsync_ReturnsTrue_WhenCategoryExists()
        {
            var context = GetDbContext();
            var repo = new CategoryRepository(context);

            var exists = await repo.ExistsByNameAsync("Electronics");

            Assert.True(exists);
        }

        [Fact]
        public async Task ExistsByNameAsync_ReturnsFalse_WhenCategoryDoesNotExist()
        {
            var context = GetDbContext();
            var repo = new CategoryRepository(context);

            var exists = await repo.ExistsByNameAsync("NonExistent");

            Assert.False(exists);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCategory_WhenExists()
        {
            var context = GetDbContext();
            var repo = new CategoryRepository(context);

            var category = await repo.GetByIdAsync(1);

            Assert.NotNull(category);
            Assert.Equal("Electronics", category!.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenInactiveOrNotExists()
        {
            var context = GetDbContext();
            var repo = new CategoryRepository(context);

            var category = await repo.GetByIdAsync(3); // Inactive
            Assert.Null(category);

            var category2 = await repo.GetByIdAsync(99); // Not exists
            Assert.Null(category2);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsOnlyActiveCategories()
        {
            var context = GetDbContext();
            var repo = new CategoryRepository(context);

            var categories = await repo.GetAllAsync();

            Assert.Equal(2, categories.Count);
            Assert.DoesNotContain(categories, c => c.Name == "Inactive");
        }

        [Fact]
        public async Task GetFilterPagedAsync_FiltersByName()
        {
            var context = GetDbContext();
            var repo = new CategoryRepository(context);

            var result = await repo.GetFilterPagedAsync(1, 10, nameFilter: "Book");

            Assert.Single(result.Items);
            Assert.Equal("Books", result.Items.First().Name);
        }

        [Fact]
        public async Task AddAsync_AddsCategorySuccessfully()
        {
            var context = GetDbContext();
            var repo = new CategoryRepository(context);

            var category = new Category { Name = "Toys", Description = "Fun toys", IsActive = true };

            await repo.AddAsync(category);

            var added = await context.Categories.FirstOrDefaultAsync(c => c.Name == "Toys");
            Assert.NotNull(added);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesCategorySuccessfully()
        {
            var context = GetDbContext();
            var repo = new CategoryRepository(context);

            var category = await context.Categories.FirstAsync(c => c.Id == 1);
            category.Description = "Updated Description";

            await repo.UpdateAsync(category);

            var updated = await context.Categories.FirstAsync(c => c.Id == 1);
            Assert.Equal("Updated Description", updated.Description);
        }

        [Fact]
        public async Task DeleteAsync_SetsIsActiveFalse()
        {
            var context = GetDbContext();
            var repo = new CategoryRepository(context);

            var category = await context.Categories.FirstAsync(c => c.Id == 1);

            await repo.DeleteAsync(category);

            var deleted = await context.Categories.FirstAsync(c => c.Id == 1);
            Assert.False(deleted.IsActive);
        }
    }

}
