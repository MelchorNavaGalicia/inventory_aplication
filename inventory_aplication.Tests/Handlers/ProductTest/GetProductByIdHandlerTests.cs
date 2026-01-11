using inventory_aplication.Application.Common.DTOs.Product;
using inventory_aplication.Application.Common.Interfaces.Repositories;
using inventory_aplication.Application.Features.Product.Queries.GetProductById;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace inventory_aplication.Tests.Handlers.ProductTest
{
    public class GetProductByIdHandlerTests
    {
        private readonly Mock<IProductRepository> _repoMock;
        private readonly GetProductByIdHandler _handler;

        public GetProductByIdHandlerTests()
        {
            _repoMock = new Mock<IProductRepository>();
            _handler = new GetProductByIdHandler(_repoMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnFail_WhenProductNotFound()
        {
            _repoMock.Setup(r => r.GetByIdDtoAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync((ProductResponseDto)null);

            var query = new GetProductByIdQuery(1);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Equal("El producto no fue encontrado", result.Error);
        }

        [Fact]
        public async Task Handle_ShouldReturnProduct_WhenProductExists()
        {
            var productDto = new ProductResponseDto { Id = 2, Name = "Prod2" };
            _repoMock.Setup(r => r.GetByIdDtoAsync(2, It.IsAny<CancellationToken>())).ReturnsAsync(productDto);

            var query = new GetProductByIdQuery(2);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.True(result.Success);
            Assert.Equal(2, result.Data.Id);
            Assert.Equal("Prod2", result.Data.Name);
        }
    }

}
