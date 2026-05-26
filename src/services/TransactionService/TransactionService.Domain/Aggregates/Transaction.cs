using TransactionService.Domain.Enums;
using TransactionService.Domain.Exceptions;

namespace TransactionService.Domain.Aggregates;

public class Transaction
{
    public Guid Id { get; private set; }
    public Guid SourceAccountId { get; private set; }
    public Guid? DestinationAccountId { get; private set; }
    public decimal Amount { get; private set; }
    public string Currency { get; private set; } = null!;
    public TransactionType Type { get; private set; }
    public TransactionStatus Status { get; private set; }
    public string IdempotencyKey { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }

    private Transaction() { }

    public static Transaction CreateDeposit(Guid sourceAccountId, decimal amount, string currency, string idempotencyKey)
    {
        if (amount <= 0)
            throw new DomainException("Deposit amount must be greater than zero.");

        if (string.IsNullOrEmpty(idempotencyKey))
            throw new DomainException("IdempotencyKey is required.");

        if (string.IsNullOrWhiteSpace(currency))
            throw new DomainException("Currency is required.");

        if (sourceAccountId.Equals(Guid.Empty))
            throw new DomainException("SourceAccountId is required.");

        return new Transaction
        {
            Id = Guid.NewGuid(),
            SourceAccountId = sourceAccountId,
            Amount = amount,
            Currency = currency,
            IdempotencyKey = idempotencyKey,
            Type = TransactionType.Deposit,
            Status = TransactionStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };
    }

    public static Transaction CreateWithdrawal(Guid sourceAccountId, decimal amount, string currency, string idempotencyKey)
    {
        if (amount <= 0)
            throw new DomainException("Withdrawal amount must be greater than zero.");

        if (string.IsNullOrEmpty(idempotencyKey))
            throw new DomainException("IdempotencyKey is required.");

        if (string.IsNullOrWhiteSpace(currency))
            throw new DomainException("Currency is required.");

        if (sourceAccountId.Equals(Guid.Empty))
            throw new DomainException("SourceAccountId is required.");

        return new Transaction
        {
            Id = Guid.NewGuid(),
            SourceAccountId = sourceAccountId,
            Amount = amount,
            Currency = currency,
            IdempotencyKey = idempotencyKey,
            Type = TransactionType.Withdrawal,
            Status = TransactionStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };
    }

    public static Transaction CreateTransfer(Guid sourceAccountId, Guid destinationAccountId, decimal amount, string currency, string idempotencyKey)
    {
        if (amount <= 0)
            throw new DomainException("Transfer amount must be greater than zero.");

        if (string.IsNullOrEmpty(idempotencyKey))
            throw new DomainException("IdempotencyKey is required.");

        if (string.IsNullOrWhiteSpace(currency))
            throw new DomainException("Currency is required.");

        if (sourceAccountId.Equals(Guid.Empty))
            throw new DomainException("SourceAccountId is required.");

        if (destinationAccountId.Equals(Guid.Empty))
            throw new DomainException("DestinationAccountId is required.");

        if (sourceAccountId.Equals(destinationAccountId))
            throw new DomainException("SourceAccountId must be different from DestinationAccountId.");

        return new Transaction
        {
            Id = Guid.NewGuid(),
            SourceAccountId = sourceAccountId,
            DestinationAccountId = destinationAccountId,
            Amount = amount,
            Currency = currency,
            IdempotencyKey = idempotencyKey,
            Type = TransactionType.Transfer,
            Status = TransactionStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Complete() => Status = TransactionStatus.Completed;
    public void Fail() => Status = TransactionStatus.Failed;
}