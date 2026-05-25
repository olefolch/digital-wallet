using AccountService.Domain.Exceptions;
using AccountService.Domain.Interfaces;
using MediatR;

namespace AccountService.Application.Features.Accounts.Commands.Deposit;

public class DepositCommandHandler(IAccountRepository accountRepository) : IRequestHandler<DepositCommand>
{
    public async Task Handle(DepositCommand request, CancellationToken cancellationToken)
    {
        var account = await accountRepository.GetByIdAsync(request.AccountId);

        if (account is null)
            throw new DomainException("Não existe uma conta para este usuário");

        account.Deposit(request.Amount);

        await accountRepository.UpdateAsync(account);
    }
}