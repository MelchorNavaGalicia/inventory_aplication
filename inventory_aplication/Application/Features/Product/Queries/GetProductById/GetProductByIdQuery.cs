using inventory_aplication.Application.Common.DTOs.Product;
using inventory_aplication.Application.Features.Common.Results;
using MediatR;

namespace inventory_aplication.Application.Features.Product.Queries.GetProductById
{
    public record GetProductByIdQuery(int Id) : IRequest<Result<ProductResponseDto>>;
}
