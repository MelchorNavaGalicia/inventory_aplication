using inventory_aplication.Application.Features.Common.Results;
using MediatR;

namespace inventory_aplication.Application.Features.Auth
{
    public record RegisterCommand(string Name,string Username, string Password) : IRequest<Result<string>>;
}
