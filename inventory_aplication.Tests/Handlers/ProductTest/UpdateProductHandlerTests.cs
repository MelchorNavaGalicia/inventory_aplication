using inventory_aplication.Application.Common.DTOs.Product;
using inventory_aplication.Application.Common.Interfaces.Repositories;
using inventory_aplication.Application.Features.Product.Commands.UpdateProduct;
using inventory_application.Data.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace inventory_aplication.Tests.Handlers.ProductTest
{
    public class UpdateProductHandlerTests
    {
        private readonly Mock<IProductRepository> _repoMock;
        private readonly UpdateProductHandler _handler;

        public UpdateProductHandlerTests()
        {
            _repoMock = new Mock<IProductRepository>();
            _handler = new UpdateProductHandler(_repoMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnFail_WhenProductNotFound()
        {
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Product)null);

            var command = new UpdateProductCommand(1, new ProductUpdateDto());

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Equal("Producto no encontrado", result.Error);
        }

        [Fact]
        public async Task Handle_ShouldReturnOk_WhenProductExists()
        {
            var product = new Product { Id = 2, Name = "Old", Description = "OldDesc", Stock = 5, Price = 50, CategoryId = 1 };
            _repoMock.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(product);

            var dto = new ProductUpdateDto { Name = "New", Description = "NewDesc", CategoryId = 2, Price = 100 };
            var command = new UpdateProductCommand(2, dto);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.Success);
            Assert.Equal("Producto actualizado correctamente", result.Data);
            _repoMock.Verify(r => r.UpdateAsync(It.Is<Product>(p =>
                p.Name == "New" &&
                p.Description == "NewDesc" &&
                p.CategoryId == 2 &&
                p.Price == 100)), Times.Once);
        }
    }

}
