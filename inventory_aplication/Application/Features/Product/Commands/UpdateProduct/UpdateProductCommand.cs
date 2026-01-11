using inventory_aplication.Application.Common.DTOs.Product;
using inventory_aplication.Application.Features.Common.Results;
using MediatR;

namespace inventory_aplication.Application.Features.Product.Commands.UpdateProduct
{
    public record UpdateProductCommand(int Id, ProductUpdateDto Dto) : IRequest<Result<string>>;
}
