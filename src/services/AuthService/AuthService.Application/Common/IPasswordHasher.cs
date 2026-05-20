namespace AuthService.Application.Common;

public interface IPasswordHasher
{
    string Hash(string password);
}