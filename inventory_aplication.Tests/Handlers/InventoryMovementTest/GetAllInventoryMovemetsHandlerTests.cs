using inventory_aplication.Application.Common.DTOs.InventoryMovement;
using inventory_aplication.Application.Common.Interfaces.Repositories;
using inventory_aplication.Application.Features.Common.Results;
using inventory_aplication.Application.Features.InventoryMovement.Queries.GetAllInventoryMovemets;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace inventory_aplication.Tests.Handlers.InventoryMovementTest
{
    public class GetAllInventoryMovemetsHandlerTests
    {
        private readonly Mock<IInventoryMovementRepository> _repoMock;
        private readonly GetAllInventoryMovemetsHandler _handler;

        public GetAllInventoryMovemetsHandlerTests()
        {
            _repoMock = new Mock<IInventoryMovementRepository>();
            _handler = new GetAllInventoryMovemetsHandler(_repoMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnPagedResult()
        {
            var pagedResult = new PagedResult<InventoryMovementResponseDto>(
                totalItems: 1,
                pageNumber: 1,
                pageSize: 10,
                items: new List<InventoryMovementResponseDto>
                {
                new InventoryMovementResponseDto { Id = 1, ProductName = "Prod1", Quantity = 5 }
                }
            );

            _repoMock.Setup(r => r.GetFilterPagedAsync(
                null, null, null, null, null, null, null, 1, 10, It.IsAny<CancellationToken>()))
                .ReturnsAsync(pagedResult);

            var query = new GetAllInventoryMovementsQuery(1, 10);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.True(result.Success);
            Assert.Equal(pagedResult, result.Data);
        }
    }

}
