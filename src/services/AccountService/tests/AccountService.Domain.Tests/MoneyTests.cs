using AccountService.Domain.Exceptions;
using AccountService.Domain.ValueObjects;

namespace AccountService.Domain.Tests;

public class MoneyTests
{
    [Fact]
    public void Create_WithValidData_ShouldReturnMoneyWithCorrectValues()
    {
        // Arrange
        var amount = 10.0m;
        var currency = "BRL";

        // Act
        var money = Money.Create(amount, currency);

        // Assert
        Assert.Equal(amount, money.Amount);
        Assert.Equal(currency, money.Currency);
    }

    [Fact]
    public void Create_WithNegativeAmount_ShouldThrowDomainException()
    {
        // Arrange
        var currency = "BRL";

        // Act & Assert
        Assert.Throws<DomainException>(() => Money.Create(-10.0m, currency));
    }

    [Fact]
    public void Create_WithEmptyCurrency_ShouldThrowDomainException()
    {
        // Arrange
        var amount = 10.0m;

        // Act & Assert
        Assert.Throws<DomainException>(() => Money.Create(amount, ""));
    }

    [Fact]
    public void Equals_WithSameValues_ShouldReturnTrue()
    {
        // Arrange
        var amount = 10.0m;
        var currency = "BRL";

        // Act
        var objA = Money.Create(amount, currency);
        var objB = Money.Create(amount, currency);

        var equals = objA.Equals(objB);

        // Assert
        Assert.True(equals);
    }
    
    [Fact]
    public void Equals_WithDifferentValues_ShouldReturnFalse()
    {
        // Arrange
        var amountA = 10.0m;
        var amountB = 20.0m;
        var currencyA = "BRL";
        var currencyB = "USD";

        // Act
        var objA = Money.Create(amountA, currencyA);
        var objB = Money.Create(amountB, currencyB);

        var equals = objA.Equals(objB);

        // Assert
        Assert.False(equals);
    }
}