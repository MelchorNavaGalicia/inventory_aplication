using inventory_aplication.Application.Common.DTOs;
using inventory_aplication.Application.Features.Common.Results;
using inventory_application.Data.Entities;

namespace inventory_aplication.Application.Common.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByAsync(string? name);
        Task<PagedResult<UserDto>> GetAllDtoAsync(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<User>> GetAllAsync();
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(User user);
        Task<bool> ExistsByNameAsync(string name);
    }
}
