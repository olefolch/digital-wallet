using AccountService.Domain.Aggregates;
using AccountService.Domain.Enums;
using AccountService.Domain.Exceptions;

namespace AccountService.Domain.Tests;

public class AccountTests
{
    [Fact]
    public void Create_WithValidData_ShouldReturnAccountWithCorrectProperties()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var currency = "BRL";
        var amount = 0.0m;
        var status = AccountStatus.Active;

        // Act
        var account = Account.Create(ownerId, currency);

        // Assert
        Assert.Equal(ownerId, account.OwnerId);
        Assert.Equal(amount, account.Balance.Amount);
        Assert.Equal(currency, account.Balance.Currency);
        Assert.Equal(status, account.Status);
    }

    [Fact]
    public void Create_WithEmptyOwnerId_ShouldThrowDomainException()
    {
        // Arrange
        var currency = "BRL";

        // Act & Assert
        Assert.Throws<DomainException>(() => Account.Create(Guid.Empty, currency));
    }

    [Fact]
    public void Deposit_WithValidAmount_ShouldIncreaseBalance()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var currency = "BRL";
        var amount = 10.0m;
        var account = Account.Create(ownerId, currency);

        // Act
        account.Deposit(amount);

        // Assert
        Assert.Equal(amount, account.Balance.Amount);
    }

    [Fact]
    public void Deposit_WithZeroAmount_ShouldThrowDomainException()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var currency = "BRL";
        var amount = 0.0m;
        var account = Account.Create(ownerId, currency);

        // Act & Assert
        Assert.Throws<DomainException>(() => account.Deposit(amount));
    }

    [Fact]
    public void Withdraw_WithSufficientBalance_ShouldDecreaseBalance()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var currency = "BRL";
        var amount = 10.0m;
        var account = Account.Create(ownerId, currency);
        account.Deposit(20.0m);

        // Act
        account.Withdraw(amount);

        // Assert
        Assert.Equal(amount, account.Balance.Amount);
    }

    [Fact]
    public void Withdraw_WithInsufficientBalance_ShouldThrowDomainException()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var currency = "BRL";
        var amount = 10.0m;
        var account = Account.Create(ownerId, currency);

        // Act & Assert
        Assert.Throws<DomainException>(() => account.Withdraw(amount));
    }

    [Fact]
    public void Withdraw_WithZeroAmount_ShouldThrowDomainException()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var currency = "BRL";
        var amount = 0.0m;
        var account = Account.Create(ownerId, currency);

        // Act & Assert
        Assert.Throws<DomainException>(() => account.Withdraw(amount));
    }
}