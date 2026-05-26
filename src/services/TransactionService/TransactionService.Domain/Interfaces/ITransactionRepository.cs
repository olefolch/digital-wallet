using TransactionService.Domain.Aggregates;

namespace TransactionService.Domain.Interfaces;

public interface ITransactionRepository
{
    Task<Transaction?> GetByIdAsync(Guid id);
    Task<Transaction?> GetByIdempotencyKeyAsync(string key);
    Task AddAsync(Transaction transaction);
    Task UpdateAsync(Transaction transaction);
}