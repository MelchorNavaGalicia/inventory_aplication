using Azure.Core;
using inventory_aplication.Application.Common.DTOs;
using inventory_aplication.Application.Common.DTOs.Product;
using inventory_aplication.Application.Common.Interfaces.Repositories;
using inventory_aplication.Application.Features.Common.Results;
using inventory_application.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace inventory_aplication.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _context.Users
                .AnyAsync(u => u.Name == name);
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetByAsync(string? name)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u =>
                    u.Username == name &&
                    u.IsActive == true
                );
            return user;
        }
        public async Task<PagedResult<UserDto>> GetAllDtoAsync(int pageNumber = 1, int pageSize = 10,CancellationToken cancellationToken = default)
        {
            var query = from u in _context.Users
                        where u.IsActive
                        select new UserDto
                        {
                            Id = u.Id,
                            Name = u.Name,
                            Username = u.Username
                        };
            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderBy(p => p.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<UserDto>(
                totalCount,
                pageNumber,
                pageSize,
                items
            );

        }
        public async Task<IReadOnlyList<User>> GetAllAsync()
        {
            return await _context.Users
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            user.UpdatedAt = DateTime.Now;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(User user)
        {
            user.IsActive = false;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
