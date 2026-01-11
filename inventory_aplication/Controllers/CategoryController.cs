using inventory_aplication.Application.Common.DTOs.Category;
using inventory_aplication.Application.Features.Category.Commands.CreateCategory;
using inventory_aplication.Application.Features.Category.Commands.DeleteCategory;
using inventory_aplication.Application.Features.Category.Commands.UpdateCategory;
using inventory_aplication.Application.Features.Category.Queries.GetAllCategories;
using inventory_aplication.Application.Features.Category.Queries.GetCategoriesBy;
using inventory_aplication.Application.Features.Category.Queries.GetCategoryById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace inventory_aplication.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CategoryController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int pageNumber = 1,[FromQuery] int pageSize = 10)
        {
            var query = new GetAllCategoriesQuery(pageNumber, pageSize);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Get(int Id)
        {
            var command = new GetCategoryByIdQuery(Id);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetBy([FromQuery] GetCategoriesByQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        // POST api/<CategoryController>
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] CategoryCreateDto dto)
        {
            var command = new CreateCategoryCommand(dto.Name, dto.Description);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> PutAsync([FromRoute] int Id, [FromBody] CategoryUpdateDto dto)
        {
            var command = new UpdateCategoryCommand(Id,dto.Name,dto.Description);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        // DELETE api/<CategoryController>/5
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            var command = new DeleteCategoryCommand(Id);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
