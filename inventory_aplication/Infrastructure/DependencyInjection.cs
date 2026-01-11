using inventory_aplication.Application.Common.Interfaces.Auth;
using inventory_aplication.Application.Common.Interfaces.CurrentUser;
using inventory_aplication.Application.Common.Interfaces.Repositories;
using inventory_aplication.Infrastructure.Persistence;
using inventory_aplication.Infrastructure.Persistence.Repositories;
using inventory_aplication.Infrastructure.Services.Auth;
using inventory_aplication.Infrastructure.Services.CurrentUser;
using Microsoft.EntityFrameworkCore;

namespace inventory_aplication.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IInventoryMovementRepository, InventoryMovementsRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddSingleton<IPasswordHasherService, PasswordHasherService>();

            return services;
        }
    }
}
