using inventory_aplication.Application.Features.Common.Results;
using MediatR;

namespace inventory_aplication.Application.Features.Category.Commands.DeleteCategory
{
    public record DeleteCategoryCommand(int Id) : IRequest<Result<string>>;
}
