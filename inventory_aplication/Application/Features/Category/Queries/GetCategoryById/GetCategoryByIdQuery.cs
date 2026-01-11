using inventory_aplication.Application.Common.DTOs.Category;
using inventory_aplication.Application.Features.Common.Results;
using MediatR;

namespace inventory_aplication.Application.Features.Category.Queries.GetCategoryById
{
    public record GetCategoryByIdQuery(int Id) : IRequest<Result<CategoryResponseDto>>;
}
