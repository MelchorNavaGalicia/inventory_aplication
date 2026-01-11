using inventory_aplication.Application.Common.Interfaces.Repositories;
using inventory_aplication.Application.Features.Common.Results;
using MediatR;

namespace inventory_aplication.Application.Features.User.Commands.DeleteUser
{

    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, Result<string>>
    {
        private readonly IUserRepository _repository;
        public DeleteUserHandler(IUserRepository repository)
        {
            _repository = repository;
        }
        public async Task<Result<string>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _repository.GetByIdAsync(request.id);
            if (user == null) {
                return Result<string>.Fail("El usuario no fue encontrado", ErrorCodes.NoFound);
            }
            await _repository.DeleteAsync(user);

            return Result<string>.Ok("El usuario fue eliminado correctamente");
        }
    }
}
