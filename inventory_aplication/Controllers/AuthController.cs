using inventory_aplication.Application.Common.DTOs;
using inventory_aplication.Application.Features.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace inventory_aplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthController(IMediator mediator) => _mediator = mediator;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var command = new RegisterCommand(dto.Name,dto.Username, dto.Password);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var command = new LoginCommand(dto.Username, dto.Password);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

    }

}
