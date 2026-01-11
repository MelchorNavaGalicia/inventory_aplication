using inventory_aplication.Application.Common.DTOs.InventoryMovement;
using inventory_aplication.Application.Common.Interfaces.Repositories;
using inventory_aplication.Application.Features.Common.Results;
using MediatR;

namespace inventory_aplication.Application.Features.InventoryMovement.Queries.GetInventoryMovementsBy
{
    public class GetInventoryMovementsByHandler: IRequestHandler<GetInventoryMovementsByQuery, Result<PagedResult<InventoryMovementResponseDto>>>
    {
        private readonly IInventoryMovementRepository _repository;

        public GetInventoryMovementsByHandler(IInventoryMovementRepository repository)
        {
            _repository = repository;
        }
        public async Task<Result<PagedResult<InventoryMovementResponseDto>>> Handle(
        GetInventoryMovementsByQuery request,
        CancellationToken cancellationToken)
        {
            var categoriesPagedResult = await _repository.GetFilterPagedAsync(request.ProductId,request.CategoryId,request.ProductName,request.CategoryName,request.UserName,request.FromDate,request.ToDate,request.PageNumber, request.PageSize, cancellationToken);

            return Result<PagedResult<InventoryMovementResponseDto>>.Ok(categoriesPagedResult);
        }
    }
}
