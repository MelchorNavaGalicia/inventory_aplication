using inventory_aplication.Application.Features.Common.Results;
using MediatR;

namespace inventory_aplication.Application.Features.Product.Commands.DeleteProduct
{
    public record DeleteProductCommand(int Id) : IRequest<Result<string>>;
}
