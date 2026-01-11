using inventory_aplication.Application.Common.Interfaces.Repositories;
using inventory_aplication.Application.Features.Common.Results;
using inventory_aplication.Infrastructure.Persistence;
using MediatR;

namespace inventory_aplication.Application.Features.Product.Commands.CreateProduct
{
    public class CreateProductHandler
    : IRequestHandler<CreateProductCommand, Result<string>>
    {
        private readonly IProductRepository _repository;
        public CreateProductHandler(AppDbContext context, IProductRepository repository)
        {
            _repository = repository;
        }
        public async Task<Result<string>> Handle(
            CreateProductCommand request,
            CancellationToken cancellationToken)
        {
            if (await _repository.ExistsByNameAsync(request.Name))
                return Result<string>.Fail("El producto ya existe", ErrorCodes.ExistingItem);
            var product = new inventory_application.Data.Entities.Product
            {
                Name = request.Name,
                Description = request.Description,
                CategoryId = request.CategoryId,
                Stock = request.InitialStock,
                Price = request.Price 
            };
            await _repository.AddAsync(product);

            return Result<string>.Ok("Producto creado correctamente");
        }
    }
}
