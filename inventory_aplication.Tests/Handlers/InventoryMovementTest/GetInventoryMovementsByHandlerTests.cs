using inventory_aplication.Application.Common.DTOs.InventoryMovement;
using inventory_aplication.Application.Common.Interfaces.Repositories;
using inventory_aplication.Application.Features.Common.Results;
using inventory_aplication.Application.Features.InventoryMovement.Queries.GetInventoryMovementsBy;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace inventory_aplication.Tests.Handlers.InventoryMovementTest
{
    public class GetInventoryMovementsByHandlerTests
    {
        private readonly Mock<IInventoryMovementRepository> _repoMock;
        private readonly GetInventoryMovementsByHandler _handler;

        public GetInventoryMovementsByHandlerTests()
        {
            _repoMock = new Mock<IInventoryMovementRepository>();
            _handler = new GetInventoryMovementsByHandler(_repoMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnPagedResultWithFilters()
        {
            var pagedResult = new PagedResult<InventoryMovementResponseDto>(
                totalItems: 1,
                pageNumber: 1,
                pageSize: 10,
                items: new List<InventoryMovementResponseDto>
                {
                new InventoryMovementResponseDto { Id = 1, ProductName = "FilteredProd", Quantity = 3 }
                }
            );

            _repoMock.Setup(r => r.GetFilterPagedAsync(
                1, 2, "FilteredProd", "Cat1", "User1", null, null, 1, 10, It.IsAny<CancellationToken>()))
                .ReturnsAsync(pagedResult);

            var query = new GetInventoryMovementsByQuery(
                ProductId: 1,
                CategoryId: 2,
                ProductName: "FilteredProd",
                CategoryName: "Cat1",
                UserName: "User1",
                FromDate: null,
                ToDate: null,
                PageNumber: 1,
                PageSize: 10
            );

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.True(result.Success);
            Assert.Equal(pagedResult, result.Data);
        }
    }

}
