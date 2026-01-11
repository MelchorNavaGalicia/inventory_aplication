using inventory_aplication.Application.Common.DTOs.InventoryMovement;
using inventory_aplication.Application.Features.Common.Results;
using MediatR;

namespace inventory_aplication.Application.Features.InventoryMovement.Queries.GetAllInventoryMovemets
{
    public record GetAllInventoryMovementsQuery(int PageNumber = 1, int PageSize = 10) : IRequest<Result<PagedResult<InventoryMovementResponseDto>>>;
}
