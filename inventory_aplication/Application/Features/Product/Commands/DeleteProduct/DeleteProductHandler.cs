using inventory_aplication.Application.Common.Interfaces.Repositories;
using inventory_aplication.Application.Features.Common.Results;
using inventory_aplication.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace inventory_aplication.Application.Features.Product.Commands.DeleteProduct
{
    public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, Result<string>>
    {
        private readonly IProductRepository _repository;
        public DeleteProductHandler(IProductRepository repository)
        {
            _repository = repository;
        }
        public async Task<Result<string>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _repository.GetByIdAsync(request.Id);
            if (product == null)
                return Result<string>.Fail(
                    "El producto no fue encontrado",
                    ErrorCodes.NotFound
                );
            await _repository.DeleteAsync(product);

            return Result<string>.Ok("Prodcuto eliminado correctamente");
        }
    }
}
