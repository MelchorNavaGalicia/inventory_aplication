using inventory_application.Data.Entities;

namespace inventory_aplication.Application.Common.Interfaces.Auth
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
