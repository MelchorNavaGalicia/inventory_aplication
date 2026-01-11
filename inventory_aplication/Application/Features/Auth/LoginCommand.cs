using inventory_aplication.Application.Features.Common.Results;
using MediatR;

namespace inventory_aplication.Application.Features.Auth
{
    public record LoginCommand(string Username, string Password) : IRequest<Result<string>>;
}
