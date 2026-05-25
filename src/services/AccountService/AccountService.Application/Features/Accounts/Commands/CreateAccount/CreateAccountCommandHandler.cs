using AccountService.Domain.Aggregates;
using AccountService.Domain.Exceptions;
using AccountService.Domain.Interfaces;
using MediatR;

namespace AccountService.Application.Features.Accounts.Commands.CreateAccount;

public class CreateAccountCommandHandler(IAccountRepository accountRepository) : IRequestHandler<CreateAccountCommand, Guid>
{
    public async Task<Guid> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await accountRepository.GetByOwnerIdAsync(request.OwnerId);

        if (account is not null)
            throw new DomainException("Já existe uma conta para este usuário");

        var createdAccount = Account.Create(request.OwnerId, request.Currency);
        await accountRepository.AddAsync(createdAccount);

        return createdAccount.Id;
    }
}