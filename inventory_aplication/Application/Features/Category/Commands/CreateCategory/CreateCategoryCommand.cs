using inventory_aplication.Application.Features.Common.Results;
using MediatR;

namespace inventory_aplication.Application.Features.Category.Commands.CreateCategory
{
    public record CreateCategoryCommand(string Name, string Description) : IRequest<Result<string>>;
}
