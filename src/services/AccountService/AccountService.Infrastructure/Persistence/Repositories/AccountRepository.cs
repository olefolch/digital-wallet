using AccountService.Domain.Aggregates;
using AccountService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Infrastructure.Persistence.Repositories;

public class AccountRepository(AccountDbContext context) : IAccountRepository
{
    public async Task AddAsync(Account account)
    {
        await context.Accounts.AddAsync(account);
        await context.SaveChangesAsync();
    }

    public async Task<Account?> GetByIdAsync(Guid id)
    {
        return await context.Accounts.FindAsync(id);
    }

    public async Task<Account?> GetByOwnerIdAsync(Guid ownerId)
    {
        return await context.Accounts.FirstOrDefaultAsync(a => a.OwnerId == ownerId);
    }

    public async Task UpdateAsync(Account account)
    {
        context.Accounts.Update(account);
        await context.SaveChangesAsync();
    }
}