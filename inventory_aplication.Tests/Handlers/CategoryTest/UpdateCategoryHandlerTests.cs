using inventory_aplication.Application.Common.Interfaces.Repositories;
using inventory_aplication.Application.Features.Category.Commands.UpdateCategory;
using inventory_application.Data.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace inventory_aplication.Tests.Handlers.CategoryTest
{
    public class UpdateCategoryHandlerTests
    {
        private readonly Mock<ICategoryRepository> _categoryRepoMock;
        private readonly UpdateCategoryHandler _handler;
        public UpdateCategoryHandlerTests()
        {
            _categoryRepoMock = new Mock<ICategoryRepository>();
            _handler = new UpdateCategoryHandler(_categoryRepoMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnFail_WhenCategoryNotFound()
        {
            _categoryRepoMock.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync((Category)null);

            var command = new UpdateCategoryCommand(1, "NewName", "NewDesc");

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Equal("La categoría no fue encontrada", result.Error);
            _categoryRepoMock.Verify(x => x.UpdateAsync(It.IsAny<Category>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnOk_WhenCategoryExists()
        {
            var category = new Category { Id = 2, Name = "OldName", Description = "OldDesc" };
            _categoryRepoMock.Setup(x => x.GetByIdAsync(2)).ReturnsAsync(category);

            var command = new UpdateCategoryCommand(2, "NewName", "NewDesc");

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.Success);
            Assert.Equal("Categoría actualizada correctamente", result.Data);
            _categoryRepoMock.Verify(x => x.UpdateAsync(It.Is<Category>(c => c.Name == "NewName" && c.Description == "NewDesc")), Times.Once);
        }
    }

}
