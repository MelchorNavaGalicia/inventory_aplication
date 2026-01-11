using inventory_aplication.Application.Common.Interfaces.CurrentUser;
using inventory_aplication.Application.Common.Interfaces.Repositories;
using inventory_aplication.Application.Features.InventoryMovement.Commands.CreateInventoryMovement;
using inventory_application.Data.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace inventory_aplication.Tests.Handlers.InventoryMovementTest
{
    public class CreateInventoryMovementHandlerTests
    {
        private readonly Mock<IInventoryMovementRepository> _movementRepoMock;
        private readonly Mock<IProductRepository> _productRepoMock;
        private readonly Mock<ICurrentUserService> _currentUserMock;
        private readonly CreateInventoryMovementHandler _handler;

        public CreateInventoryMovementHandlerTests()
        {
            _movementRepoMock = new Mock<IInventoryMovementRepository>();
            _productRepoMock = new Mock<IProductRepository>();
            _currentUserMock = new Mock<ICurrentUserService>();
            _handler = new CreateInventoryMovementHandler(
                null!, // AppDbContext no usado en handler
                _movementRepoMock.Object,
                _productRepoMock.Object,
                _currentUserMock.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldReturnFail_WhenProductNotFound()
        {
            _productRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Product)null);

            var command = new CreateInventoryMovementCommand(1, 5, "IN");

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Equal("El producto no existe", result.Error);
        }

        [Fact]
        public async Task Handle_ShouldReturnFail_WhenStockInsufficient()
        {
            var product = new Product { Id = 2, Name = "Prod", Stock = 2 };
            _productRepoMock.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(product);
            _currentUserMock.Setup(c => c.UserId).Returns(10);

            // Simulamos que ApplyMovement lanza InvalidOperationException
            _movementRepoMock.Setup(m => m.ApplyMovement(product, 5, "OUT"))
                .Throws(new InvalidOperationException("Stock insuficiente"));

            var command = new CreateInventoryMovementCommand(2, 5, "OUT");

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.Success);
            Assert.Equal("Stock insuficiente", result.Error);
            _movementRepoMock.Verify(m => m.AddAsync(It.IsAny<InventoryMovement>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnOk_WhenMovementAppliedSuccessfully()
        {
            var product = new Product { Id = 3, Name = "Prod", Stock = 5 };
            _productRepoMock.Setup(r => r.GetByIdAsync(3)).ReturnsAsync(product);
            _currentUserMock.Setup(c => c.UserId).Returns(20);

            var command = new CreateInventoryMovementCommand(3, 5, "IN");

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.Success);
            Assert.Equal("Movimiento agregado correctamente", result.Data);
            _movementRepoMock.Verify(m => m.ApplyMovement(product, 5, "IN"), Times.Once);
            _movementRepoMock.Verify(m => m.AddAsync(It.Is<InventoryMovement>(im => im.ProductId == 3 && im.UserId == 20)), Times.Once);
        }
    }

}
