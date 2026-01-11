using Azure.Core;
using inventory_aplication.Application.Common.DTOs.Product;
using inventory_aplication.Application.Common.Interfaces.Repositories;
using inventory_aplication.Application.Features.Common.Results;
using inventory_application.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace inventory_aplication.Infrastructure.Persistence.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _context.Products
                .AnyAsync(p => p.Name == name && p.IsActive);
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<ProductResponseDto?> GetByIdDtoAsync(int id, CancellationToken cancellationToken = default)
        {
            return await (
                from p in _context.Products
                join c in _context.Categories on p.CategoryId equals c.Id
                where p.IsActive && p.Id == id
                select new ProductResponseDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    CategoryId = p.CategoryId,
                    CategoryName = c.Name,
                    Price = p.Price,
                    Stock = p.Stock
                }
            ).FirstOrDefaultAsync(cancellationToken);
        }
        public async Task<PagedResult<ProductResponseDto>> GetFilterPagedAsync(int? categoryId = null,string? name = null,string? categoryName = null,int pageNumber = 1,int pageSize = 10,CancellationToken cancellationToken = default)
        {
            var query = from p in _context.Products
                        join c in _context.Categories on p.CategoryId equals c.Id
                        where p.IsActive
                        select new ProductResponseDto
                        {
                            Id = p.Id,
                            Name = p.Name,
                            Description = p.Description,
                            CategoryId = c.Id,
                            CategoryName = c.Name,
                            Price = p.Price,
                            Stock = p.Stock
                        };
            if (categoryId.HasValue)
                query = query.Where(p => p.CategoryId == categoryId.Value);

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(p => p.Name.Contains(name));

            if (!string.IsNullOrWhiteSpace(categoryName))
                query = query.Where(p => p.CategoryName.Contains(categoryName)); // <-- nuevo filtro

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderBy(p => p.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<ProductResponseDto>(
                totalCount,
                pageNumber,
                pageSize,
                items
            );
        }

        public async Task<IReadOnlyList<Product>> GetAllAsync()
        {
            return await _context.Products
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            product.UpdatedAt = DateTime.Now;
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Product product)
        {
            product.IsActive = false;
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }
    }
}
