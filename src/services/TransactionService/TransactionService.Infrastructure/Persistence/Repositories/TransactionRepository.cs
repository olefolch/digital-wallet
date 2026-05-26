using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using TransactionService.Domain.Aggregates;
using TransactionService.Domain.Interfaces;

namespace TransactionService.Infrastructure.Persistence.Repositories;

public class TransactionRepository(TransactionDbContext context, IDistributedCache cache) : ITransactionRepository
{
    public async Task AddAsync(Transaction transaction)
    {
        await context.Transactions.AddAsync(transaction);
        await context.SaveChangesAsync();
    }

    public async Task<Transaction?> GetByIdAsync(Guid id)
    {
        return await context.Transactions.FindAsync(id);
    }

    public async Task<Transaction?> GetByIdempotencyKeyAsync(string key)
    {
        var cachedId = await cache.GetStringAsync(key);
        if (cachedId is not null)
            return await context.Transactions.FindAsync(Guid.Parse(cachedId));

        var transaction = await context.Transactions
            .FirstOrDefaultAsync(t => t.IdempotencyKey == key);

        if (transaction is not null)
            await cache.SetStringAsync(key, transaction.Id.ToString(),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24)
                });

        return transaction;
    }

    public async Task UpdateAsync(Transaction transaction)
    {
        context.Transactions.Update(transaction);
        await context.SaveChangesAsync();
    }
}