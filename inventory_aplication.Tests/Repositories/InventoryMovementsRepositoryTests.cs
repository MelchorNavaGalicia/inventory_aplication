using System;
using System.Collections.Generic;
using System.Text;
using inventory_aplication.Application.Common.DTOs.InventoryMovement;
using inventory_aplication.Application.Features.Common.Results;
using inventory_aplication.Infrastructure.Persistence;
using inventory_aplication.Infrastructure.Persistence.Repositories;
using inventory_application.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace inventory_aplication.Tests.Repositories
{

    public class InventoryMovementsRepositoryTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid())
                .Options;

            var context = new AppDbContext(options);

            // Seed categories
            context.Categories.Add(new Category { Id = 1, Name = "Electronics" ,Description="Electronic Test Category"});
            context.Categories.Add(new Category { Id = 2, Name = "Books", Description = "Books Test Category" });

            // Seed users
            context.Users.Add(new User { Id = 1, Name = "John Doe" , Username= "JohnDoeUser", Password ="TestPasw123"});
            context.Users.Add(new User { Id = 2, Name = "Jane Smith" , Username = "JaneSmithUser", Password = "PaswSecret" });

            // Seed products
            context.Products.Add(new Product { Id = 1, Name = "Laptop",Description="TestLaptop", Stock = 5, CategoryId = 1, IsActive = true });
            context.Products.Add(new Product { Id = 2, Name = "Book", Description = "TestBook", Stock = 10, CategoryId = 2, IsActive = true });

            // Seed inventory movements
            context.InventoryMovements.Add(new InventoryMovement { Id = 1, ProductId = 1, Quantity = 2, MovementType = "IN", UserId = 1, MovementDate = DateTime.UtcNow, IsActive = true });
            context.InventoryMovements.Add(new InventoryMovement { Id = 2, ProductId = 2, Quantity = 1, MovementType = "OUT", UserId = 2, MovementDate = DateTime.UtcNow, IsActive = true });

            context.SaveChanges();

            return context;
        }

        [Fact]
        public void ApplyMovement_IncreasesStock_WhenTypeIn()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new InventoryMovementsRepository(context);
            var product = context.Products.First(p => p.Id == 1);

            // Act
            repo.ApplyMovement(product, 3, "IN");

            // Assert
            Assert.Equal(8, product.Stock);
        }

        [Fact]
        public void ApplyMovement_DecreasesStock_WhenTypeOut()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new InventoryMovementsRepository(context);
            var product = context.Products.First(p => p.Id == 1);

            // Act
            repo.ApplyMovement(product, 2, "OUT");

            // Assert
            Assert.Equal(3, product.Stock);
        }

        [Fact]
        public void ApplyMovement_Throws_WhenQuantityZeroOrNegative()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new InventoryMovementsRepository(context);
            var product = context.Products.First(p => p.Id == 1);

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => repo.ApplyMovement(product, 0, "IN"));
            Assert.Equal("La cantidad debe ser mayor a cero", ex.Message);
        }

        [Fact]
        public void ApplyMovement_Throws_WhenStockInsufficient()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new InventoryMovementsRepository(context);
            var product = context.Products.First(p => p.Id == 1);

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => repo.ApplyMovement(product, 10, "OUT"));
            Assert.Equal("Stock insuficiente", ex.Message);
        }

        [Fact]
        public async Task AddAsync_AddsMovementSuccessfully()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new InventoryMovementsRepository(context);

            var movement = new InventoryMovement
            {
                ProductId = 1,
                Quantity = 5,
                MovementType = "IN",
                UserId = 1,
                MovementDate = DateTime.UtcNow,
                IsActive = true
            };

            // Act
            await repo.AddAsync(movement);
            var added = await context.InventoryMovements.FirstOrDefaultAsync(m => m.ProductId == 1 && m.Quantity == 5);

            // Assert
            Assert.NotNull(added);
            Assert.Equal("IN", added.MovementType);
        }

        [Fact]
        public async Task GetFilterPagedAsync_FiltersByProductName()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new InventoryMovementsRepository(context);

            // Act
            var result = await repo.GetFilterPagedAsync(productName: "Laptop", pageNumber: 1, pageSize: 10);

            // Assert
            Assert.Single(result.Items);
            Assert.Equal("Laptop", result.Items.First().ProductName);
        }

        [Fact]
        public async Task GetFilterPagedAsync_FiltersByUserName()
        {
            // Arrange
            var context = GetDbContext();
            var repo = new InventoryMovementsRepository(context);

            // Act
            var result = await repo.GetFilterPagedAsync(userName: "Jane", pageNumber: 1, pageSize: 10);

            // Assert
            Assert.Single(result.Items);
            Assert.Equal("Jane Smith", result.Items.First().MovementBy);
        }
    }

}
