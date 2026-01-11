using inventory_aplication.Application.Common.Interfaces.Repositories;
using inventory_aplication.Application.Features.Category.Commands.DeleteCategory;
using inventory_application.Data.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace inventory_aplication.Tests.Handlers.CategoryTest
{
    public class DeleteCategoryHandlerTests
    {
        private readonly Mock<ICategoryRepository> _categoryRepoMock;
        private readonly DeleteCategoryHandler _handler;

        public DeleteCategoryHandlerTests()
        {
            _categoryRepoMock = new Mock<ICategoryRepository>();
            _handler = new DeleteCategoryHandler(_categoryRepoMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnFail_WhenCategoryNotFound()
        {
            // Arrange
            _categoryRepoMock.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync((Category)null);

            var command = new DeleteCategoryCommand(1);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("La categoría no fue encontrada", result.Error);
            _categoryRepoMock.Verify(x => x.DeleteAsync(It.IsAny<Category>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnOk_WhenCategoryExists()
        {
            // Arrange
            var category = new Category { Id = 2, Name = "Books" };
            _categoryRepoMock.Setup(x => x.GetByIdAsync(2))
                .ReturnsAsync(category);

            var command = new DeleteCategoryCommand(2);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Categoría eliminada correctamente", result.Data);
            _categoryRepoMock.Verify(x => x.DeleteAsync(category), Times.Once);
        }
    }

}
