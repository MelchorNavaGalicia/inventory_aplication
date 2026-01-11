using inventory_aplication.Application.Features.Common.Results;
using MediatR;

namespace inventory_aplication.Application.Features.Category.Commands.UpdateCategory
{
    public record UpdateCategoryCommand(int Id, string? Name, string? Description) : IRequest<Result<string>>;
}
