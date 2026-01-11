using inventory_aplication.Application.Common.DTOs.Product;
using inventory_aplication.Application.Common.Interfaces.Repositories;
using inventory_aplication.Application.Features.Common.Results;
using MediatR;

namespace inventory_aplication.Application.Features.Product.Queries.GetAllProducts
{
    public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, Result<PagedResult<ProductResponseDto>>>
    {
        private readonly IProductRepository _repository;
        public GetAllProductsHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<PagedResult<ProductResponseDto>>> Handle(GetAllProductsQuery request,CancellationToken cancellationToken)
        {
            var productPagedResult = await _repository.GetFilterPagedAsync(null,null,null,request.PageNumber,request.PageSize,cancellationToken);

            return Result<PagedResult<ProductResponseDto>>.Ok(productPagedResult);
        }
    }
}
