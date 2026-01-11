using System;
using System.Collections.Generic;
using System.Text;
using inventory_aplication.Application.Common.DTOs.InventoryMovement;
using inventory_aplication.Application.Features.Common.Results;
using inventory_aplication.Application.Features.InventoryMovement.Commands.CreateInventoryMovement;
using inventory_aplication.Application.Features.InventoryMovement.Queries.GetAllInventoryMovemets;
using inventory_aplication.Application.Features.InventoryMovement.Queries.GetInventoryMovementsBy;
using inventory_aplication.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace inventory_aplication.Tests.Controllers
{
    public class InventoryMovementControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly InventoryMovementController _controller;

        public InventoryMovementControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new InventoryMovementController(_mediatorMock.Object);
        }

        #region GET All

        [Fact]
        public async Task GetAsync_ReturnsOk_WithPagedInventoryMovements()
        {
            // Arrange
            var expectedResult = Result<PagedResult<InventoryMovementResponseDto>>.Ok(
                new PagedResult<InventoryMovementResponseDto>(
                    totalItems: 1,
                    pageNumber: 1,
                    pageSize: 10,
                    items: new List<InventoryMovementResponseDto>
                    {
                    new InventoryMovementResponseDto
                    {
                        Id = 1,
                        ProductId = 1,
                        Quantity = 10,
                        MovementType = "IN",
                        MovementBy = "TestUsr",
                        MovementDate = System.DateTime.Now
                    }
                    }
                )
            );

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetAllInventoryMovementsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.GetAsync(1, 10);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsType<Result<PagedResult<InventoryMovementResponseDto>>>(okResult.Value);

            Assert.True(value.Success);
            Assert.Single(value.Data.Items);
            Assert.Equal(1, value.Data.Items[0].ProductId);
        }

        #endregion

        #region GET Filter

        [Fact]
        public async Task GetByFilter_ReturnsOk_WithFilteredInventoryMovements()
        {
            // Arrange
            var query = new GetInventoryMovementsByQuery(ProductId: 1);

            var expectedResult = Result<PagedResult<InventoryMovementResponseDto>>.Ok(
                new PagedResult<InventoryMovementResponseDto>(
                    totalItems: 1,
                    pageNumber: 1,
                    pageSize: 10,
                    items: new List<InventoryMovementResponseDto>
                    {
                    new InventoryMovementResponseDto
                    {
                        Id = 1,
                        ProductId = 1,
                        Quantity = 5,
                        MovementType = "OUT",
                        MovementBy = "TestUsr",
                        MovementDate = System.DateTime.Now
                    }
                    }
                )
            );

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetInventoryMovementsByQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.Get(query);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsType<Result<PagedResult<InventoryMovementResponseDto>>>(okResult.Value);

            Assert.True(value.Success);
            Assert.Single(value.Data.Items);
            Assert.Equal("OUT", value.Data.Items[0].MovementType);
        }

        #endregion

        #region POST - Create Inventory Movement

        [Fact]
        public async Task PostAsync_ReturnsOk_WhenInventoryMovementCreated()
        {
            // Arrange
            var dto = new InventoryMovementDto
            {
                ProductId = 1,
                Quantity = 10,
                MovementType = "IN"
            };

            var expectedResult = Result<string>.Ok("Movimiento de inventario creado correctamente");

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CreateInventoryMovementCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.PostAsync(dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsType<Result<string>>(okResult.Value);

            Assert.True(value.Success);
            Assert.Equal("Movimiento de inventario creado correctamente", value.Data);
        }

        #endregion
    }

}
