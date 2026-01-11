using inventory_aplication.Application.Common.Interfaces.Repositories;
using inventory_aplication.Application.Features.Product.Commands.DeleteProduct;
using inventory_application.Data.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace inventory_aplication.Tests.Handlers.ProductTest
{
    public class DeleteProductHandlerTests
    {
        private readonly Mock<IProductRepository> _repoMock;
        private readonly DeleteProductHandler _handler;

        public DeleteProductHandlerTests()
        {
            _repoMock = new Mock<IProductRepository>();
            _handler = new DeleteProductHandler(_repoMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnFail_WhenProductNotFound()
        {
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Product)null);

            var command = new DeleteProductCommand(1);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Equal("El producto no fue encontrado", result.Error);
        }

        [Fact]
        public async Task Handle_ShouldReturnOk_WhenProductExists()
        {
            var product = new Product { Id = 2, Name = "Prod2" };
            _repoMock.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(product);

            var command = new DeleteProductCommand(2);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.Success);
            Assert.Equal("Prodcuto eliminado correctamente", result.Data);
            _repoMock.Verify(r => r.DeleteAsync(product), Times.Once);
        }
    }

}
