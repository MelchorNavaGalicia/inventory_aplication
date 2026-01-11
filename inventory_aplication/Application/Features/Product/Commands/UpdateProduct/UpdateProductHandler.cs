using inventory_aplication.Application.Common.Interfaces.Repositories;
using inventory_aplication.Application.Features.Common.Results;
using MediatR;

namespace inventory_aplication.Application.Features.Product.Commands.UpdateProduct
{
    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, Result<string>>
    {
        private readonly IProductRepository _repository;
        public UpdateProductHandler(IProductRepository repository)
        {
            _repository = repository;
        }
        public async Task<Result<string>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _repository.GetByIdAsync(request.Id);
            if (product == null)
                return Result<string>.Fail("Producto no encontrado", ErrorCodes.NotFound);

            var dto = request.Dto;
            if (dto.Name != null)
                product.Name = dto.Name;
            if (dto.Description != null)
                product.Description = dto.Description;
            if (dto.CategoryId.HasValue)
                product.CategoryId = dto.CategoryId.Value;
            if (dto.Price.HasValue)
                product.Price = dto.Price.Value;
            product.UpdatedAt = DateTime.UtcNow;
            await _repository.UpdateAsync(product);

            return Result<string>.Ok("Producto actualizado correctamente");
        }

    }
}
