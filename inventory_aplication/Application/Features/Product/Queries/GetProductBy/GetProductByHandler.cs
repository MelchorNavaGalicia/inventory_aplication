using inventory_aplication.Application.Common.DTOs.Product;
using inventory_aplication.Application.Common.Interfaces.Repositories;
using inventory_aplication.Application.Features.Common.Results;
using inventory_aplication.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace inventory_aplication.Application.Features.Product.Queries.GetProductBy
{
    public class GetProductByHandler
        : IRequestHandler<GetProductByQuery, Result<PagedResult<ProductResponseDto>>>
    {
        private readonly IProductRepository _repository;
        public GetProductByHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<PagedResult<ProductResponseDto>>> Handle(GetProductByQuery request,CancellationToken cancellationToken)
        {
            var productPagedResult = await _repository.GetFilterPagedAsync(request.CategoryId,request.Name,request.CategoryName, request.PageNumber, request.PageSize, cancellationToken);

            return Result<PagedResult<ProductResponseDto>>.Ok(productPagedResult);
        }

    }
}
