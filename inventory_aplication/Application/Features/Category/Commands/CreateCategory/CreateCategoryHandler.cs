using inventory_aplication.Application.Common.Interfaces.Repositories;
using inventory_aplication.Application.Features.Common.Results;
using inventory_aplication.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace inventory_aplication.Application.Features.Category.Commands.CreateCategory
{
    public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, Result<string>>
    {
        private readonly ICategoryRepository _category;
        public CreateCategoryHandler(ICategoryRepository category)
        {
            _category = category;
        }
        public async Task<Result<string>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            if (await _category.ExistsByNameAsync(request.Name))
                return Result<string>.Fail("La categoria ya existe", ErrorCodes.ExistingItem);
            var category = new inventory_application.Data.Entities.Category
            {
                Name = request.Name,
                Description = request.Description
            };
            await _category.AddAsync(category);

            return Result<string>.Ok("Categoria creada correctamente");
        }
    }
}
