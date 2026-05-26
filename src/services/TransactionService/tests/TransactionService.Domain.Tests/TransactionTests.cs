using TransactionService.Domain.Aggregates;
using TransactionService.Domain.Enums;
using TransactionService.Domain.Exceptions;

namespace TransactionService.Domain.Tests;

public class TransactionTests
{
    [Fact]
    public void CreateDeposit_WithValidData_ShouldReturnTransactionWithCorrectProperties()
    {
        // Arrange
        var sourceAccountId = Guid.NewGuid();
        var amount = 10.0m;
        var currency = "BRL";
        var idempotencyKey = Guid.NewGuid().ToString();

        // Act
        var deposit = Transaction.CreateDeposit(sourceAccountId, amount, currency, idempotencyKey);

        // Assert
        Assert.Equal(sourceAccountId, deposit.SourceAccountId);
        Assert.Equal(amount, deposit.Amount);
        Assert.Equal(currency, deposit.Currency);
        Assert.Equal(idempotencyKey, deposit.IdempotencyKey);
    }

    [Fact]
    public void CreateDeposit_WithZeroAmount_ShouldThrowDomainException()
    {
        // Arrange
        var sourceAccountId = Guid.NewGuid();
        var currency = "BRL";
        var idempotencyKey = Guid.NewGuid().ToString();

        // Act & Assert
        Assert.Throws<DomainException>(() =>
            Transaction.CreateDeposit(sourceAccountId, 0.0m, currency, idempotencyKey));
    }

    [Fact]
    public void CreateDeposit_WithEmptyIdempotencyKey_ShouldThrowDomainException()
    {
        // Arrange
        var sourceAccountId = Guid.NewGuid();
        var amount = 10.0m;
        var currency = "BRL";

        // Act & Assert
        Assert.Throws<DomainException>(() =>
            Transaction.CreateDeposit(sourceAccountId, amount, currency, ""));
    }

    [Fact]
    public void CreateDeposit_WithEmptySourceAccountId_ShouldThrowDomainException()
    {
        // Arrange
        var amount = 10.0m;
        var currency = "BRL";
        var idempotencyKey = Guid.NewGuid().ToString();

        // Act & Assert
        Assert.Throws<DomainException>(() =>
            Transaction.CreateDeposit(Guid.Empty, amount, currency, idempotencyKey));
    }

    [Fact]
    public void CreateWithdrawal_WithValidData_ShouldReturnTransactionWithCorrectProperties()
    {
        // Arrange
        var sourceAccountId = Guid.NewGuid();
        var amount = 10.0m;
        var currency = "BRL";
        var idempotencyKey = Guid.NewGuid().ToString();

        // Act
        var withdrawal = Transaction.CreateWithdrawal(sourceAccountId, amount, currency, idempotencyKey);

        // Assert
        Assert.Equal(sourceAccountId, withdrawal.SourceAccountId);
        Assert.Equal(amount, withdrawal.Amount);
        Assert.Equal(currency, withdrawal.Currency);
        Assert.Equal(idempotencyKey, withdrawal.IdempotencyKey);
    }

    [Fact]
    public void CreateTransfer_WithValidData_ShouldReturnTransactionWithCorrectProperties()
    {
        // Arrange
        var sourceAccountId = Guid.NewGuid();
        var destinationAccountId = Guid.NewGuid();
        var amount = 10.0m;
        var currency = "BRL";
        var idempotencyKey = Guid.NewGuid().ToString();

        // Act
        var transfer = Transaction.CreateTransfer(sourceAccountId, destinationAccountId, amount, currency, idempotencyKey);

        // Assert
        Assert.Equal(sourceAccountId, transfer.SourceAccountId);
        Assert.Equal(destinationAccountId, transfer.DestinationAccountId);
        Assert.Equal(amount, transfer.Amount);
        Assert.Equal(currency, transfer.Currency);
        Assert.Equal(idempotencyKey, transfer.IdempotencyKey);
    }

    [Fact]
    public void CreateTransfer_WithSameSourceAndDestination_ShouldThrowDomainException()
    {
        // Arrange
        var sourceAccountId = Guid.NewGuid();
        var amount = 10.0m;
        var currency = "BRL";
        var idempotencyKey = Guid.NewGuid().ToString();

        // Act & Assert
        Assert.Throws<DomainException>(() =>
            Transaction.CreateTransfer(sourceAccountId, sourceAccountId, amount, currency, idempotencyKey));
    }

    [Fact]
    public void Complete_ShouldChangeStatusToCompleted()
    {
        // Arrange
        var transaction = Transaction.CreateDeposit(Guid.NewGuid(), 10m, "BRL", Guid.NewGuid().ToString());

        // Act
        transaction.Complete();

        // Assert
        Assert.Equal(TransactionStatus.Completed, transaction.Status);
    }

    [Fact]
    public void Fail_ShouldChangeStatusToFailed()
    {
        // Arrange
        var transaction = Transaction.CreateDeposit(Guid.NewGuid(), 10m, "BRL", Guid.NewGuid().ToString());

        // Act
        transaction.Fail();

        // Assert
        Assert.Equal(TransactionStatus.Failed, transaction.Status);
    }
}