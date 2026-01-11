using inventory_aplication.Application.Common.DTOs;
using inventory_aplication.Application.Features.Common.Results;
using MediatR;

namespace inventory_aplication.Application.Features.User.Queries.GetAllUsers
{
    public record GetAllUsersQuery(int PageNumber = 1, int PageSize = 10) : IRequest<Result<PagedResult<UserDto>>>;
}
