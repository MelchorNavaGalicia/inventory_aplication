using inventory_aplication.Application.Common.Interfaces.Auth;
using inventory_aplication.Application.Common.Interfaces.Repositories;
using inventory_aplication.Application.Features.Common.Results;
using inventory_aplication.Infrastructure.Persistence;
using inventory_aplication.Infrastructure.Services.Auth;
using inventory_application.Data.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace inventory_aplication.Application.Features.Auth
{

    public class RegisterHandler : IRequestHandler<RegisterCommand, Result<string>>
    {
        private readonly IJwtService _jwt;
        private readonly IPasswordHasherService _hasher;
        private readonly IUserRepository _user;

        public RegisterHandler(IJwtService jwt, IPasswordHasherService hasher, IUserRepository user)
        {
            _jwt = jwt;
            _hasher = hasher;
            _user = user;
        }

        public async Task<Result<string>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            // Validar que el username no exista
            if (await _user.ExistsByNameAsync(request.Username))
                return Result<string>.Fail("El usuario ya existe", ErrorCodes.ExistingUser);

            var user = new inventory_application.Data.Entities.User
            {
                Name = request.Name,
                Username = request.Username,
                Password = _hasher.HashPassword(request.Password)
            };
            await _user.AddAsync(user);
            var token = _jwt.GenerateToken(user);

            return Result<string>.Ok(token);
        }
    }
}
