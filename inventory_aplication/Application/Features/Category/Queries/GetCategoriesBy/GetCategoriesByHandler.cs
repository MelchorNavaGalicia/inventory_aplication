using inventory_aplication.Application.Common.DTOs.Category;
using inventory_aplication.Application.Common.Interfaces.Repositories;
using inventory_aplication.Application.Features.Common.Results;
using inventory_aplication.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace inventory_aplication.Application.Features.Category.Queries.GetCategoriesBy
{
    public class GetCategoriesByHandler
        : IRequestHandler<GetCategoriesByQuery, Result<PagedResult<CategoryResponseDto>>>
    {
        private readonly ICategoryRepository _category;
        public GetCategoriesByHandler(ICategoryRepository category)
        {
            _category = category;
        }
        public async Task<Result<PagedResult<CategoryResponseDto>>> Handle(
            GetCategoriesByQuery request,
            CancellationToken cancellationToken)
        {
            var categoriesPagedResult = await _category.GetFilterPagedAsync(request.PageNumber, request.PageSize, request.Name, cancellationToken);
            
            return Result<PagedResult<CategoryResponseDto>>.Ok(categoriesPagedResult);
        }
    }
}

