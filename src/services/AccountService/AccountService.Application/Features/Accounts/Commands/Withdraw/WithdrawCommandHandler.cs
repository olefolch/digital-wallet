using AccountService.Domain.Exceptions;
using AccountService.Domain.Interfaces;
using MediatR;

namespace AccountService.Application.Features.Accounts.Commands.Withdraw;

public class WithdrawCommandHandler(IAccountRepository accountRepository) : IRequestHandler<WithdrawCommand>
{
    public async Task Handle(WithdrawCommand request, CancellationToken cancellationToken)
    {
        var account = await accountRepository.GetByIdAsync(request.AccountId);

        if (account is null)
            throw new DomainException("Não existe uma conta para este usuário");

        account.Withdraw(request.Amount);

        await accountRepository.UpdateAsync(account);
    }
}