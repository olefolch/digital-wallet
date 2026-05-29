using MediatR;
using SharedContracts.Events;
using TransactionService.Application.Common;
using TransactionService.Domain.Aggregates;
using TransactionService.Domain.Interfaces;

namespace TransactionService.Application.Features.Transactions.Commands.CreateDeposit;

public class CreateDepositCommandHandler(ITransactionRepository transactionRepository, IMessagePublisher messagePublisher) : IRequestHandler<CreateDepositCommand, Guid>
{
    public async Task<Guid> Handle(CreateDepositCommand request, CancellationToken cancellationToken)
    {
        var transaction = await transactionRepository.GetByIdempotencyKeyAsync(request.IdempotencyKey);

        if (transaction is not null)
            return transaction.Id;

        var deposit = Transaction.CreateDeposit(request.SourceAccountId, request.Amount, request.Currency, request.IdempotencyKey);
        await transactionRepository.AddAsync(deposit);

        var transactionCreatedEvent = new TransactionCreatedEvent(
            deposit.Id,
            deposit.SourceAccountId,
            deposit.DestinationAccountId,
            deposit.Amount,
            deposit.Currency,
            deposit.Type.ToString()
        );

        await messagePublisher.PublishAsync(transactionCreatedEvent);

        // TODO: marcar como Complete() ou Fail() após Account Service confirmar a atualização do saldo via evento (Fase 5)

        return deposit.Id;
    }
}