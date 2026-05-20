using AuthService.Domain.Entities;
using AuthService.Domain.Exceptions;

namespace AuthService.Domain.Tests;

public class UserTests
{
    [Fact]
    public void Create_WithValidData_ShouldReturnUserWithCorrectProperties()
    {
        // Arrange
        var name = "Test";
        var email = "email@test.com";
        var passwordHash = "hash123";

        // Act
        var user = User.Create(name, email, passwordHash);

        // Assert
        Assert.Equal(name, user.Name);
        Assert.Equal(email, user.Email);
    }

    [Fact]
    public void Create_WithEmptyName_ShouldThrowDomainException()
    {
        // Arrange
        var email = "email@test.com";
        var passwordHash = "hash123";

        // Act & Assert
        Assert.Throws<DomainException>(() => User.Create("", email, passwordHash));
    }

    [Fact]
    public void Create_WithEmptyEmail_ShouldThrowDomainException()
    {
        // Arrange
        var name = "Test";
        var passwordHash = "hash123";

        // Act & Assert
        Assert.Throws<DomainException>(() => User.Create(name, "", passwordHash));
    }

    [Fact]
    public void Create_WithEmptyPasswordHash_ShouldThrowDomainException()
    {
        // Arrange
        var name = "Test";
        var email = "email@test.com";

        // Act & Assert
        Assert.Throws<DomainException>(() => User.Create(name, email, ""));
    }
}
