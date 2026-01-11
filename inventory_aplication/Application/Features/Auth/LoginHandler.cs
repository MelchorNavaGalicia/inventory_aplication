using MediatR;
using Microsoft.EntityFrameworkCore;
using inventory_aplication.Application.Features.Common.Results;
using inventory_aplication.Infrastructure.Services.Auth;
using inventory_aplication.Infrastructure.Persistence;
using inventory_aplication.Application.Common.Interfaces.Auth;
using inventory_aplication.Application.Common.Interfaces.Repositories;

namespace inventory_aplication.Application.Features.Auth
{
    public class LoginHandler : IRequestHandler<LoginCommand, Result<string>>
    {
        private readonly IJwtService _jwt;
        private readonly IPasswordHasherService _hasher;
        private readonly IUserRepository _User;

        public LoginHandler(IJwtService jwt, IPasswordHasherService hasher, IUserRepository User)
        {
            _jwt = jwt;
            _hasher = hasher;
            _User = User;
        }

        public async Task<Result<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _User.GetByAsync(request.Username);
            if (user == null)
            {
                return Result<string>.Fail("El usuario o contraseña estan equivocados", ErrorCodes.InvalidCredentials);
            }
            if (!_hasher.VerifyPassword(request.Password, user.Password))
                return Result<string>.Fail("El usuario o contraseña estan equivocados", ErrorCodes.InvalidCredentials);
            var token = _jwt.GenerateToken(user);

            return Result<string>.Ok(token);
        }
    }
}
