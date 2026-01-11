using inventory_aplication.Application.Features.Common.Results;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace inventory_aplication.Application.Features.Product.Commands.CreateProduct
{
    public record CreateProductCommand(string Name, string Description, int CategoryId, int InitialStock, decimal Price) : IRequest<Result<string>>;
}
