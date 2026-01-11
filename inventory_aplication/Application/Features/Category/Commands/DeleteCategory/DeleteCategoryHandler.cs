using inventory_aplication.Application.Common.Interfaces.Repositories;
using inventory_aplication.Application.Features.Common.Results;
using inventory_aplication.Infrastructure.Persistence;
using inventory_application.Data.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace inventory_aplication.Application.Features.Category.Commands.DeleteCategory
{
    public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, Result<string>>
    {
        private readonly ICategoryRepository _category;
        public DeleteCategoryHandler(ICategoryRepository category)
        {
            _category = category;
        }
        public async Task<Result<string>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _category.GetByIdAsync(request.Id);
            if (category == null)
                return Result<string>.Fail(
                    "La categoría no fue encontrada",
                    ErrorCodes.NotFound
                );
            await _category.DeleteAsync(category);

            return Result<string>.Ok("Categoría eliminada correctamente");
        }
    }
}
