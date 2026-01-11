using inventory_aplication.Application.Common.DTOs.Product;
using inventory_aplication.Application.Features.Common.Results;
using inventory_application.Data.Entities;

namespace inventory_aplication.Application.Common.Interfaces.Repositories
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(int id);
        Task<ProductResponseDto?> GetByIdDtoAsync(int id, CancellationToken cancellationToken = default);
        Task<PagedResult<ProductResponseDto>> GetFilterPagedAsync(int? categoryId = null, string? name = null, string? categoryName = null, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Product>> GetAllAsync();
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(Product product);
        Task<bool> ExistsByNameAsync(string name);
    }
}
