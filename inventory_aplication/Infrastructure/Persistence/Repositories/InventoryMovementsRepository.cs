using inventory_aplication.Application.Common.DTOs.InventoryMovement;
using inventory_aplication.Application.Common.Interfaces.Repositories;
using inventory_aplication.Application.Features.Common.Results;
using inventory_application.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace inventory_aplication.Infrastructure.Persistence.Repositories
{
    public class InventoryMovementsRepository : IInventoryMovementRepository
    {
        private readonly AppDbContext _context;
        public InventoryMovementsRepository(AppDbContext context)
        {
            _context = context;
        }
        public void ApplyMovement(Product product, int quantity, string movementType)
        {
            if (quantity <= 0)
                throw new InvalidOperationException("La cantidad debe ser mayor a cero");

            if (movementType == "OUT" && product.Stock < quantity)
                throw new InvalidOperationException("Stock insuficiente");

            if (movementType == "IN")
                product.Stock += quantity;
            else if (movementType == "OUT")
                product.Stock -= quantity;
        }
        public async Task<PagedResult<InventoryMovementResponseDto>> GetFilterPagedAsync(int? productId = null,int? categoryId = null,string? productName = null,string? categoryName = null,string? userName = null,DateTime? startDate = null,DateTime? endDate = null,int pageNumber = 1,int pageSize = 10,CancellationToken cancellationToken = default)
        {
            var query = from m in _context.InventoryMovements
                        join p in _context.Products on m.ProductId equals p.Id
                        join c in _context.Categories on p.CategoryId equals c.Id
                        join u in _context.Users on m.UserId equals u.Id
                        where m.IsActive
                        select new InventoryMovementResponseDto
                        {
                            Id = m.Id,
                            MovementType = m.MovementType,
                            Quantity = m.Quantity,
                            ProductId = p.Id,
                            ProductName = p.Name,
                            CategoryId = c.Id,
                            CategoryName = c.Name,
                            UserId = u.Id,
                            MovementBy = u.Name,
                            MovementDate = m.MovementDate
                        };

            if (productId.HasValue)
                query = query.Where(x => x.ProductId == productId.Value);

            if (categoryId.HasValue)
                query = query.Where(x => x.CategoryId == categoryId.Value);

            if (!string.IsNullOrWhiteSpace(productName))
                query = query.Where(x => x.ProductName.Contains(productName));

            if (!string.IsNullOrWhiteSpace(categoryName))
                query = query.Where(x => x.CategoryName.Contains(categoryName));

            if (!string.IsNullOrWhiteSpace(userName))
                query = query.Where(x => x.MovementBy.Contains(userName));

            if (startDate.HasValue)
                query = query.Where(x => x.MovementDate >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(x => x.MovementDate <= endDate.Value);

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .AsNoTracking()
                .OrderBy(x => x.MovementDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<InventoryMovementResponseDto>(
                totalCount,
                pageNumber,
                pageSize,
                items
            );
        }
        public async Task AddAsync(InventoryMovement inventoryMovement)
        {
            await _context.InventoryMovements.AddAsync(inventoryMovement);
            await _context.SaveChangesAsync();
        }
    }
}