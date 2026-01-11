using inventory_aplication.Infrastructure.Persistence;
using inventory_aplication.Infrastructure.Persistence.Repositories;
using inventory_application.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;
using inventory_aplication.Application.Common.DTOs.Product;
using inventory_aplication.Application.Features.Common.Results;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace inventory_aplication.Tests.Repositories
{

    public class ProductRepositoryTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_" + System.Guid.NewGuid())
                .Options;

            var context = new AppDbContext(options);

            // Seed data
            context.Categories.Add(new Category { Id = 1, Name = "Electronics", Description = "Electrónica" });
            context.Categories.Add(new Category { Id = 2, Name = "Books", Description = "Libros" });

            context.Products.AddRange(
                new Product { Id = 1, Name = "Laptop", Description = "Laptop Gamer", CategoryId = 1, Price = 1000, Stock = 5, IsActive = true },
                new Product { Id = 2, Name = "Book", Description = "Libro de C#", CategoryId = 2, Price = 30, Stock = 10, IsActive = true }
            );

            context.SaveChanges();

            return context;
        }

        [Fact]
        public async Task ExistsByNameAsync_ReturnsTrue_WhenProductExists()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new ProductRepository(context);

            // Act
            var exists = await repo.ExistsByNameAsync("Laptop");

            // Assert
            Assert.True(exists);
        }

        [Fact]
        public async Task ExistsByNameAsync_ReturnsFalse_WhenProductDoesNotExist()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new ProductRepository(context);

            // Act
            var exists = await repo.ExistsByNameAsync("NonExistingProduct");

            // Assert
            Assert.False(exists);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsProduct_WhenProductExists()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new ProductRepository(context);

            // Act
            var product = await repo.GetByIdAsync(1);

            // Assert
            Assert.NotNull(product);
            Assert.Equal("Laptop", product.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenProductDoesNotExist()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new ProductRepository(context);

            // Act
            var product = await repo.GetByIdAsync(999);

            // Assert
            Assert.Null(product);
        }

        [Fact]
        public async Task AddAsync_AddsProductSuccessfully()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new ProductRepository(context);

            var newProduct = new Product
            {
                Name = "Tablet",
                Description = "Tablet Android",
                CategoryId = 1,
                Price = 500,
                Stock = 3,
                IsActive = true
            };

            // Act
            await repo.AddAsync(newProduct);
            var added = await context.Products.FirstOrDefaultAsync(p => p.Name == "Tablet");

            // Assert
            Assert.NotNull(added);
            Assert.Equal("Tablet", added.Name);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesProductSuccessfully()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new ProductRepository(context);

            var product = await context.Products.FirstAsync(p => p.Id == 1);
            product.Price = 1200;

            // Act
            await repo.UpdateAsync(product);
            var updated = await context.Products.FirstAsync(p => p.Id == 1);

            // Assert
            Assert.Equal(1200, updated.Price);
            Assert.NotNull(updated.UpdatedAt);
        }

        [Fact]
        public async Task DeleteAsync_SetsProductInactive()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new ProductRepository(context);

            var product = await context.Products.FirstAsync(p => p.Id == 2);

            // Act
            await repo.DeleteAsync(product);
            var deleted = await context.Products.FirstAsync(p => p.Id == 2);

            // Assert
            Assert.False(deleted.IsActive);
        }

        [Fact]
        public async Task GetFilterPagedAsync_ReturnsFilteredProducts()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new ProductRepository(context);

            // Act
            var result = await repo.GetFilterPagedAsync(name: "Laptop", pageNumber: 1, pageSize: 10);

            // Assert
            Assert.Single(result.Items);
            Assert.Equal("Laptop", result.Items.First().Name);
        }
    }


}
