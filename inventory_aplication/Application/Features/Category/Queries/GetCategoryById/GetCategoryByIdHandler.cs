using inventory_aplication.Application.Common.DTOs.Category;
using inventory_aplication.Application.Common.Interfaces.Repositories;
using inventory_aplication.Application.Features.Common.Results;
using MediatR;

namespace inventory_aplication.Application.Features.Category.Queries.GetCategoryById
{
    public class GetCategoryByIdHandler
    : IRequestHandler<GetCategoryByIdQuery, Result<CategoryResponseDto>>
    {
        private readonly ICategoryRepository _category;
        public GetCategoryByIdHandler(ICategoryRepository category)
        {
            _category = category;
        }

        public async Task<Result<CategoryResponseDto>> Handle(
            GetCategoryByIdQuery request,
            CancellationToken cancellationToken)
        {
            var categoryResult = await _category.GetByIdAsync(request.Id);
            if (categoryResult == null)
            {
                return Result<CategoryResponseDto>.Fail(
                    "La categoría no fue encontrada",
                    ErrorCodes.NotFound
                );
            }
            CategoryResponseDto category = new CategoryResponseDto
            {
                Id = categoryResult.Id,
                Name = categoryResult.Name,
                Description = categoryResult.Description
            };

            return Result<CategoryResponseDto>.Ok(category);
        }
    }
}
