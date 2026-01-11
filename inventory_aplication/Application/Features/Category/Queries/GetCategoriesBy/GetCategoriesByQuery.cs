using inventory_aplication.Application.Common.DTOs.Category;
using inventory_aplication.Application.Features.Common.Results;
using MediatR;

namespace inventory_aplication.Application.Features.Category.Queries.GetCategoriesBy
{
    public record GetCategoriesByQuery(string? Name = null, int PageNumber = 1, int PageSize = 10) : IRequest<Result<PagedResult<CategoryResponseDto>>>;
}
