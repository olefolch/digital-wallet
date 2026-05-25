using AccountService.Domain.Exceptions;
using AccountService.Domain.Interfaces;
using MediatR;

namespace AccountService.Application.Features.Accounts.Queries.GetAccount;

public class GetAccountQueryHandler(IAccountRepository accountRepository) : IRequestHandler<GetAccountQuery, AccountDto>
{
    public async Task<AccountDto> Handle(GetAccountQuery request, CancellationToken cancellationToken)
    {
        var account = await accountRepository.GetByIdAsync(request.AccountId);

        if (account is null)
            throw new DomainException("Não existe uma conta para este usuário");

        return new AccountDto
        {
            Id = account.Id,
            OwnerId = account.OwnerId,
            Balance = account.Balance.Amount,
            Currency = account.Balance.Currency,
            Status = account.Status.ToString(),
            CreatedAt = account.CreatedAt
        };
    }
}