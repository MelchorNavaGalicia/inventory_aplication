using inventory_aplication.Application.Common.DTOs.InventoryMovement;
using inventory_aplication.Application.Features.Common.Results;
using MediatR;

namespace inventory_aplication.Application.Features.InventoryMovement.Queries.GetInventoryMovementsBy
{
    public record GetInventoryMovementsByQuery(
    int? ProductId = null,
    string? ProductName = null,
    int? CategoryId = null,
    string? UserName = null,
    string? CategoryName = null,
    DateTime? FromDate = null,
    DateTime? ToDate = null,
    int PageNumber = 1,
    int PageSize = 20
    ) : IRequest<Result<PagedResult<InventoryMovementResponseDto>>>;
}
