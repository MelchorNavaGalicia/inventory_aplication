using inventory_aplication.Application.Features.Common.Results;
using MediatR;

namespace inventory_aplication.Application.Features.InventoryMovement.Commands.CreateInventoryMovement
{
    public record CreateInventoryMovementCommand(int ProductId, int Quantity, string MovementType) : IRequest<Result<string>>;
}
