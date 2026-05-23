using AccountService.Domain.Aggregates;

namespace AccountService.Domain.Interfaces;

public interface IAccountRepository
{
    Task<Account?> GetByIdAsync(Guid id);
    Task<Account?> GetByOwnerIdAsync(Guid ownerId);
    Task AddAsync(Account account);
    Task UpdateAsync(Account account);
}