using inventory_aplication.Application.Common.Interfaces.Repositories;
using inventory_aplication.Application.Features.Category.Commands.CreateCategory;
using inventory_application.Data.Entities;
using Moq;

namespace inventory_aplication.Tests.Handlers.CategoryTest
{
    public class CreateCategoryHandlerTests
    {
        private readonly Mock<ICategoryRepository> _categoryRepoMock;
        private readonly CreateCategoryHandler _handler;

        public CreateCategoryHandlerTests()
        {
            _categoryRepoMock = new Mock<ICategoryRepository>();
            _handler = new CreateCategoryHandler(_categoryRepoMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnFail_WhenCategoryAlreadyExists()
        {
            // Arrange
            _categoryRepoMock.Setup(x => x.ExistsByNameAsync("Electronics"))
                .ReturnsAsync(true);

            var command = new CreateCategoryCommand("Electronics", "Tech gadgets");

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("La categoria ya existe", result.Error);
            _categoryRepoMock.Verify(x => x.AddAsync(It.IsAny<Category>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnOk_WhenCategoryDoesNotExist()
        {
            // Arrange
            _categoryRepoMock.Setup(x => x.ExistsByNameAsync("Books"))
                .ReturnsAsync(false);

            var command = new CreateCategoryCommand("Books", "Reading material");

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Categoria creada correctamente", result.Data);
            _categoryRepoMock.Verify(x => x.AddAsync(It.Is<Category>(c => c.Name == "Books")), Times.Once);
        }
    }

}
