using AccountService.Domain.Enums;
using AccountService.Domain.Exceptions;
using AccountService.Domain.ValueObjects;

namespace AccountService.Domain.Aggregates;

public class Account
{
    public Guid Id { get; private set; }
    public Guid OwnerId { get; private set; }
    public Money Balance { get; private set; } = null!;
    public AccountStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Account(Guid ownerId, string currency)
    {
        Id = Guid.NewGuid();
        OwnerId = ownerId;
        Balance = Money.Create(0, currency);
        Status = AccountStatus.Active;
        CreatedAt = DateTime.UtcNow;
    }

    public static Account Create(Guid ownerId, string currency)
    {
        if (ownerId == Guid.Empty)
            throw new DomainException("OwnerId is required");

        return new Account(ownerId, currency);
    }

    public void Deposit(decimal amount)
    {
        if (amount <= 0)
            throw new DomainException("Deposit amount must be greater than zero.");

        if (Status != AccountStatus.Active)
            throw new DomainException("Cannot deposit to an inactive/suspended account.");

        Balance = Money.Create(Balance.Amount + amount, Balance.Currency);
    }

    public void Withdraw(decimal amount)
    {
        if (amount <= 0)
            throw new DomainException("Withdrawal amount must be greater than zero.");

        if (Status != AccountStatus.Active)
            throw new DomainException("Cannot withdraw from an inactive/suspended account.");

        if (Balance.Amount < amount)
            throw new DomainException("Insufficient funds for withdrawal.");

        Balance = Money.Create(Balance.Amount - amount, Balance.Currency);
    }
}