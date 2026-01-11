using inventory_aplication.Application.Common.Interfaces.Repositories;
using inventory_aplication.Application.Features.Common.Results;
using inventory_aplication.Application.Features.Product.Commands.CreateProduct;
using inventory_application.Data.Entities;
using Moq;
using Xunit;

namespace inventory_aplication.Tests.Handlers.ProductTest
{
    public class CreateProductHandlerTests
    {
        private readonly Mock<IProductRepository> _repoMock;
        private readonly CreateProductHandler _handler;

        public CreateProductHandlerTests()
        {
            _repoMock = new Mock<IProductRepository>();
            _handler = new CreateProductHandler(null!, _repoMock.Object);
        }

        [Fact]
        public async Task Handle_ProductAlreadyExists_ShouldFail()
        {
            // Arrange
            var command = new CreateProductCommand(
                "Producto",
                "Desc",
                1,
                5,
                100
            );

            _repoMock
                .Setup(r => r.ExistsByNameAsync(command.Name))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(ErrorCodes.ExistingItem, result.Code);
        }

        [Fact]
        public async Task Handle_NewProduct_ShouldCreateProduct()
        {
            // Arrange
            var command = new CreateProductCommand(
                "Nuevo Producto",
                "Desc",
                1,
                10,
                50
            );

            _repoMock
                .Setup(r => r.ExistsByNameAsync(command.Name))
                .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Success);

            _repoMock.Verify(r =>
                r.AddAsync(It.Is<Product>(p =>
                    p.Name == command.Name &&
                    p.Stock == command.InitialStock &&
                    p.Price == command.Price
                )),
                Times.Once
            );
        }
    }
}
