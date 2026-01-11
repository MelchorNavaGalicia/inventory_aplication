using inventory_aplication.Application.Common.DTOs.Product;
using inventory_aplication.Application.Common.Interfaces.Repositories;
using inventory_aplication.Application.Features.Common.Results;
using inventory_aplication.Application.Features.Product.Queries.GetProductBy;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace inventory_aplication.Tests.Handlers.ProductTest
{
    public class GetProductByHandlerTests
    {
        private readonly Mock<IProductRepository> _repoMock;
        private readonly GetProductByHandler _handler;

        public GetProductByHandlerTests()
        {
            _repoMock = new Mock<IProductRepository>();
            _handler = new GetProductByHandler(_repoMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnPagedResultWithFilter()
        {
            var pagedResult = new PagedResult<ProductResponseDto>(
                totalItems: 1,
                pageNumber: 1,
                pageSize: 10,
                items: new List<ProductResponseDto> { new ProductResponseDto { Id = 1, Name = "FilteredProd",CategoryName="Cat1" } }
            );

            _repoMock.Setup(r => r.GetFilterPagedAsync(null, "FilteredProd", "Cat1", 1, 10, It.IsAny<CancellationToken>()))
                .ReturnsAsync(pagedResult);

            var query = new GetProductByQuery("FilteredProd", null ,"Cat1", 1, 10);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.True(result.Success);
            Assert.Equal(pagedResult, result.Data);
        }
    }

}
