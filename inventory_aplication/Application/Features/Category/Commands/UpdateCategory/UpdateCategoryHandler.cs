using inventory_aplication.Application.Common.Interfaces.Repositories;
using inventory_aplication.Application.Features.Common.Results;
using MediatR;

namespace inventory_aplication.Application.Features.Category.Commands.UpdateCategory
{
    public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, Result<string>>
    {
        private readonly ICategoryRepository _category;
        public UpdateCategoryHandler(ICategoryRepository category)
        {
            _category = category;
        }
        public async Task<Result<string>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _category.GetByIdAsync(request.Id);
            if (category == null)
                return Result<string>.Fail(
                    "La categoría no fue encontrada",
                    ErrorCodes.NotFound
                );

            if (request.Name != null)
                category.Name = request.Name;
            if (request.Description != null)
                category.Description = request.Description;
            await _category.UpdateAsync(category);

            return Result<string>.Ok("Categoría actualizada correctamente");
        }
    }
}
