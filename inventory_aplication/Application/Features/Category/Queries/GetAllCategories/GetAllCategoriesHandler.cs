using inventory_aplication.Application.Common.DTOs.Category;
using inventory_aplication.Application.Common.Interfaces.Repositories;
using inventory_aplication.Application.Features.Common.Results;
using MediatR;

namespace inventory_aplication.Application.Features.Category.Queries.GetAllCategories
{
    public class GetAllCategoriesHandler : IRequestHandler<GetAllCategoriesQuery, Result<PagedResult<CategoryResponseDto>>>
    {
        private readonly ICategoryRepository _category;
        public GetAllCategoriesHandler(ICategoryRepository category)
        {
            _category = category;
        }

        public async Task<Result<PagedResult<CategoryResponseDto>>> Handle(GetAllCategoriesQuery request,CancellationToken cancellationToken)
        {
            var categoriesPagedResult = await _category.GetFilterPagedAsync(request.PageNumber, request.PageSize, null, cancellationToken);

            return Result<PagedResult<CategoryResponseDto>>.Ok(categoriesPagedResult);
        }
    }
}
