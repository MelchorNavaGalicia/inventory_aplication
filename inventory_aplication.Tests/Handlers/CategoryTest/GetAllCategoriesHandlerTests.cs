using inventory_aplication.Application.Common.DTOs.Category;
using inventory_aplication.Application.Common.Interfaces.Repositories;
using inventory_aplication.Application.Features.Category.Queries.GetAllCategories;
using inventory_aplication.Application.Features.Common.Results;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace inventory_aplication.Tests.Handlers.CategoryTest
{
    public class GetAllCategoriesHandlerTests
    {
        private readonly Mock<ICategoryRepository> _categoryRepoMock;
        private readonly GetAllCategoriesHandler _handler;

        public GetAllCategoriesHandlerTests()
        {
            _categoryRepoMock = new Mock<ICategoryRepository>();
            _handler = new GetAllCategoriesHandler(_categoryRepoMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnPagedResult()
        {
            var pagedResult = new PagedResult<CategoryResponseDto>(
                totalItems: 1,
                pageNumber: 1,
                pageSize: 10,
                items: new List<CategoryResponseDto> { new CategoryResponseDto { Id = 1, Name = "Cat1" } }
            );

            _categoryRepoMock.Setup(x => x.GetFilterPagedAsync(1, 10, null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(pagedResult);

            var query = new GetAllCategoriesQuery(1, 10);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.True(result.Success);
            Assert.Equal(pagedResult, result.Data);
        }
    }

}
