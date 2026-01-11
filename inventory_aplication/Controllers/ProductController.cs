using inventory_aplication.Application.Common.DTOs.Product;
using inventory_aplication.Application.Features.Product.Commands.CreateProduct;
using inventory_aplication.Application.Features.Product.Commands.DeleteProduct;
using inventory_aplication.Application.Features.Product.Commands.UpdateProduct;
using inventory_aplication.Application.Features.Product.Queries.GetAllProducts;
using inventory_aplication.Application.Features.Product.Queries.GetProductBy;
using inventory_aplication.Application.Features.Product.Queries.GetProductById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace inventory_aplication.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProductController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetAllProductsQuery(pageNumber, pageSize);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Get(int Id)
        {
            var query = new GetProductByIdQuery(Id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetBy([FromQuery] GetProductByQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProductCreateDto dto)
        {
            var command = new CreateProductCommand(dto.Name,dto.Description,dto.CategoryId,dto.Stock,dto.Price);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Put([FromRoute]  int Id, [FromBody] ProductUpdateDto dto)
        {
            var command = new UpdateProductCommand(Id, dto);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            var command = new DeleteProductCommand(Id);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
