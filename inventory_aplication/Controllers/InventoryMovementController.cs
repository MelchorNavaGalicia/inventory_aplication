using inventory_aplication.Application.Common.DTOs.InventoryMovement;
using inventory_aplication.Application.Features.InventoryMovement.Commands.CreateInventoryMovement;
using inventory_aplication.Application.Features.InventoryMovement.Queries.GetAllInventoryMovemets;
using inventory_aplication.Application.Features.InventoryMovement.Queries.GetInventoryMovementsBy;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace inventory_aplication.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryMovementController : ControllerBase
    {
        private readonly IMediator _mediator;
        public InventoryMovementController(IMediator mediator) => _mediator = mediator;

        
        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetAllInventoryMovementsQuery(pageNumber,pageSize);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("filter")]
        public async Task<IActionResult> Get([FromQuery] GetInventoryMovementsByQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] InventoryMovementDto dto)
        {
            var command = new CreateInventoryMovementCommand(dto.ProductId, dto.Quantity, dto.MovementType);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

    }
}
