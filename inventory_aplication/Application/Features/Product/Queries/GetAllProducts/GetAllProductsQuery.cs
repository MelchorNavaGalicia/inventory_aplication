using inventory_aplication.Application.Common.DTOs.Product;
using inventory_aplication.Application.Features.Common.Results;
using MediatR;

namespace inventory_aplication.Application.Features.Product.Queries.GetAllProducts
{
    public record GetAllProductsQuery(int PageNumber = 1, int PageSize = 10) : IRequest<Result<PagedResult<ProductResponseDto>>>;
}
