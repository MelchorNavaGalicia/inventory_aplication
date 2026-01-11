using inventory_aplication.Application.Common.DTOs.InventoryMovement;
using inventory_aplication.Application.Common.Interfaces.Repositories;
using inventory_aplication.Application.Features.Common.Results;
using MediatR;

namespace inventory_aplication.Application.Features.InventoryMovement.Queries.GetAllInventoryMovemets
{
    public class GetAllInventoryMovemetsHandler
    : IRequestHandler<GetAllInventoryMovementsQuery, Result<PagedResult<InventoryMovementResponseDto>>>
    {
        private readonly IInventoryMovementRepository _repository;
        public GetAllInventoryMovemetsHandler(IInventoryMovementRepository repository)
        {
            _repository = repository;
        }
        public async Task<Result<PagedResult<InventoryMovementResponseDto>>> Handle(
            GetAllInventoryMovementsQuery request,
            CancellationToken cancellationToken)
        {
            var categoriesPagedResult = await _repository.GetFilterPagedAsync(null,null,null,null,null,null,null,request.PageNumber,request.PageSize,cancellationToken);

            return Result<PagedResult<InventoryMovementResponseDto>>.Ok(categoriesPagedResult);
        }
    }
}
