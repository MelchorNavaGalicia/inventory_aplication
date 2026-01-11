using inventory_aplication.Application.Common.Interfaces.CurrentUser;
using inventory_aplication.Application.Common.Interfaces.Repositories;
using inventory_aplication.Application.Features.Common.Results;
using inventory_aplication.Infrastructure.Persistence;
using MediatR;

namespace inventory_aplication.Application.Features.InventoryMovement.Commands.CreateInventoryMovement
{
    public class CreateInventoryMovementHandler
    : IRequestHandler<CreateInventoryMovementCommand, Result<string>>
    {
        private readonly IInventoryMovementRepository _repository;
        private readonly IProductRepository _repositoryProduct;
        private readonly ICurrentUserService _currentUser;
        public CreateInventoryMovementHandler(AppDbContext context,IInventoryMovementRepository repository, IProductRepository repositoryProduct, ICurrentUserService currentUser)
        {
            _repository = repository;
            _repositoryProduct = repositoryProduct;
            _currentUser = currentUser;
        }
        public async Task<Result<string>> Handle(
            CreateInventoryMovementCommand request,
            CancellationToken cancellationToken)
        {
            var product = await _repositoryProduct.GetByIdAsync(request.ProductId);
            if (product == null)
            {
                return Result<string>.Fail("El producto no existe",ErrorCodes.NotFound);
            }
            try
            {
                _repository.ApplyMovement(
                    product,
                    request.Quantity,
                    request.MovementType
                );
            }
            catch (InvalidOperationException ex)
            {
                return Result<string>.Fail(
                    ex.Message,
                    ErrorCodes.InvalidOperation
                );
            }
            var inventoryMovement = new inventory_application.Data.Entities.InventoryMovement
            {
                ProductId = request.ProductId,
                UserId = _currentUser.UserId,
                Quantity = request.Quantity,
                MovementType = request.MovementType,
                MovementDate = DateTime.UtcNow,
                IsActive = true
            };
            await _repository.AddAsync(inventoryMovement);

            return Result<string>.Ok("Movimiento agregado correctamente");
        }
    }
}
