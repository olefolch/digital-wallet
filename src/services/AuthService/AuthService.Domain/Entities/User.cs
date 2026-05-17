using AuthService.Domain.Exceptions;

namespace AuthService.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private User(string name, string email, string passwordHash)
    {
        Id = Guid.NewGuid();
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
        CreatedAt = DateTime.UtcNow;
    }

    public static User Create(string name, string email, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new DomainException("Name is required");
        if (string.IsNullOrWhiteSpace(email)) throw new DomainException("Email is required");
        if (string.IsNullOrWhiteSpace(passwordHash)) throw new DomainException("Password hash is required");

        return new User(name, email, passwordHash);
    }
}
