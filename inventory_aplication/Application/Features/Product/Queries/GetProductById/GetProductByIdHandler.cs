using inventory_aplication.Application.Common.DTOs.Product;
using inventory_aplication.Application.Common.Interfaces.Repositories;
using inventory_aplication.Application.Features.Common.Results;
using MediatR;

namespace inventory_aplication.Application.Features.Product.Queries.GetProductById
{
    public class GetProductByIdHandler
    : IRequestHandler<GetProductByIdQuery, Result<ProductResponseDto>>
    {
        private readonly IProductRepository _repository;
        public GetProductByIdHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<ProductResponseDto>> Handle(GetProductByIdQuery request,CancellationToken cancellationToken)
        {
            var product = await _repository.GetByIdDtoAsync(request.Id);
            if (product == null)
            {
                return Result<ProductResponseDto>.Fail(
                    "El producto no fue encontrado",
                    ErrorCodes.NotFound
                );
            }

            return Result<ProductResponseDto>.Ok(product);
        }
    }
}
