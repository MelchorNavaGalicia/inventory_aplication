using inventory_aplication.Application.Features.Common.Results;
using inventory_aplication.Application.Features.User.Commands.DeleteUser;
using inventory_aplication.Application.Features.User.Queries.GetAllUsers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace inventory_aplication.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UserController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetAllUsers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var Query = new GetAllUsersQuery(pageNumber, pageSize);
            var result = await _mediator.Send(Query);
            return Ok(result);
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var command = new DeleteUserCommand(id);
            var result =  await _mediator.Send(command);
            return Ok(result);
        }

    }

    
}
