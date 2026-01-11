using inventory_aplication.Application.Common.DTOs.Category;
using inventory_aplication.Application.Common.Interfaces.Repositories;
using inventory_aplication.Application.Features.Common.Results;
using inventory_application.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace inventory_aplication.Infrastructure.Persistence.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _context.Categories
                .AnyAsync(c => c.Name == name && c.IsActive);
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id && c.IsActive);
        }

        public async Task<Category?> GetByAsync(int id, string name)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id && c.IsActive);
        }

        public async Task<IReadOnlyList<Category>> GetAllAsync()
        {
            return await _context.Categories
                .Where(c => c.IsActive)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<PagedResult<CategoryResponseDto>> GetFilterPagedAsync(int pageNumber,int pageSize,string? nameFilter = null,CancellationToken cancellationToken = default)
        {
            var query = _context.Categories
                .Where(c => c.IsActive);
            if (!string.IsNullOrWhiteSpace(nameFilter))
            {
                query = query.Where(c => c.Name.Contains(nameFilter));
            }
            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .OrderBy(c => c.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new CategoryResponseDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description
                })
                .ToListAsync(cancellationToken);
            return new PagedResult<CategoryResponseDto>(
                totalCount,
                pageNumber,
                pageSize,
                items
            );
        }

        public async Task AddAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Category category)
        {
            category.UpdatedAt = DateTime.UtcNow;
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Category category)
        {
            category.IsActive = false;
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }
    }
}
