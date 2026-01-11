using inventory_aplication.Application.Common.Interfaces.Repositories;
using inventory_aplication.Application.Features.Category.Queries.GetCategoryById;
using inventory_application.Data.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace inventory_aplication.Tests.Handlers.CategoryTest
{
    public class GetCategoryByIdHandlerTests
    {
        private readonly Mock<ICategoryRepository> _categoryRepoMock;
        private readonly GetCategoryByIdHandler _handler;

        public GetCategoryByIdHandlerTests()
        {
            _categoryRepoMock = new Mock<ICategoryRepository>();
            _handler = new GetCategoryByIdHandler(_categoryRepoMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnFail_WhenCategoryNotFound()
        {
            _categoryRepoMock.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync((Category)null);

            var query = new GetCategoryByIdQuery(1);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Equal("La categoría no fue encontrada", result.Error);
        }

        [Fact]
        public async Task Handle_ShouldReturnCategory_WhenCategoryExists()
        {
            var category = new Category { Id = 2, Name = "Cat2", Description = "Desc2" };
            _categoryRepoMock.Setup(x => x.GetByIdAsync(2))
                .ReturnsAsync(category);

            var query = new GetCategoryByIdQuery(2);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.True(result.Success);
            Assert.Equal(2, result.Data.Id);
            Assert.Equal("Cat2", result.Data.Name);
            Assert.Equal("Desc2", result.Data.Description);
        }
    }

}
