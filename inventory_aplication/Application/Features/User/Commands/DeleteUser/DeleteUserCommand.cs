using inventory_aplication.Application.Features.Common.Results;
using MediatR;

namespace inventory_aplication.Application.Features.User.Commands.DeleteUser
{
    public record DeleteUserCommand(int id) : IRequest<Result<string>>;
}
