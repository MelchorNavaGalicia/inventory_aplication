namespace inventory_aplication.Application.Common.Interfaces.Auth
{
    public interface IPasswordHasherService
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string storedHash);
    }
}
