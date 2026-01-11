using inventory_aplication.Application.Common.DTOs.InventoryMovement;
using inventory_aplication.Application.Features.Common.Results;
using inventory_application.Data.Entities;

namespace inventory_aplication.Application.Common.Interfaces.Repositories
{
    public interface IInventoryMovementRepository
    {
        void ApplyMovement(Product product, int quantity, string movementType);
        Task<PagedResult<InventoryMovementResponseDto>> GetFilterPagedAsync(int? productId = null, int? categoryId = null, string? productName = null, string? categoryName = null, string? userName = null, DateTime? startDate = null, DateTime? endDate = null, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);
        Task AddAsync(InventoryMovement inventoryMovement);
    }
}
