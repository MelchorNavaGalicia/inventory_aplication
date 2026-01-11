using inventory_aplication.Application.Common.DTOs;
using inventory_aplication.Application.Common.Interfaces.Repositories;
using inventory_aplication.Application.Features.Common.Results;
using inventory_aplication.Infrastructure.Persistence;
using inventory_aplication.Infrastructure.Services.Auth;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace inventory_aplication.Application.Features.User.Queries.GetAllUsers
{
    public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, Result<PagedResult<UserDto>>>
    {
        private readonly IUserRepository _repository;
        public GetAllUsersHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<PagedResult<UserDto>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _repository.GetAllDtoAsync(request.PageNumber, request.PageSize, cancellationToken);
            return Result<PagedResult<UserDto>>.Ok(users);
        }
    }
}
