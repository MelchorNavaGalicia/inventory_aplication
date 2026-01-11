using inventory_aplication.Application.Common.DTOs.Category;
using inventory_aplication.Application.Features.Common.Results;
using inventory_application.Data.Entities;

namespace inventory_aplication.Application.Common.Interfaces.Repositories
{
    public interface ICategoryRepository
    {
        Task<Category?> GetByIdAsync(int id);
        Task<Category?> GetByAsync(int id, string name);
        Task<PagedResult<CategoryResponseDto>> GetFilterPagedAsync(int pageNumber, int pageSize, string? nameFilter = null, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Category>> GetAllAsync();
        Task AddAsync(Category category);
        Task UpdateAsync(Category category);
        Task DeleteAsync(Category category);
        Task<bool> ExistsByNameAsync(string name);
    }
}
