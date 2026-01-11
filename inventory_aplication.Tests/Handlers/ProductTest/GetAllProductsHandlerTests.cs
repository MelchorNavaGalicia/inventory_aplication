using inventory_aplication.Application.Common.DTOs.Product;
using inventory_aplication.Application.Common.Interfaces.Repositories;
using inventory_aplication.Application.Features.Common.Results;
using inventory_aplication.Application.Features.Product.Queries.GetAllProducts;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace inventory_aplication.Tests.Handlers.ProductTest
{
    public class GetAllProductsHandlerTests
    {
        private readonly Mock<IProductRepository> _repoMock;
        private readonly GetAllProductsHandler _handler;

        public GetAllProductsHandlerTests()
        {
            _repoMock = new Mock<IProductRepository>();
            _handler = new GetAllProductsHandler(_repoMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnPagedResult()
        {
            var pagedResult = new PagedResult<ProductResponseDto>(
                totalItems: 1,
                pageNumber: 1,
                pageSize: 10,
                items: new List<ProductResponseDto> { new ProductResponseDto { Id = 1, Name = "Prod1" } }
            );

            _repoMock.Setup(r => r.GetFilterPagedAsync(null, null, null, 1, 10, It.IsAny<CancellationToken>()))
                .ReturnsAsync(pagedResult);

            var query = new GetAllProductsQuery(1, 10);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.True(result.Success);
            Assert.Equal(pagedResult, result.Data);
        }
    }

}
