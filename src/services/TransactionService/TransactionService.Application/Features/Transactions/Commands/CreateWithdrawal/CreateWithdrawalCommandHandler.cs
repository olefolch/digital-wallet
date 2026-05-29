using MediatR;
using SharedContracts.Events;
using TransactionService.Application.Common;
using TransactionService.Domain.Aggregates;
using TransactionService.Domain.Interfaces;

namespace TransactionService.Application.Features.Transactions.Commands.CreateWithdrawal;

public class CreateWithdrawalCommandHandler(ITransactionRepository transactionRepository, IMessagePublisher messagePublisher) : IRequestHandler<CreateWithdrawalCommand, Guid>
{
    public async Task<Guid> Handle(CreateWithdrawalCommand request, CancellationToken cancellationToken)
    {
        var transaction = await transactionRepository.GetByIdempotencyKeyAsync(request.IdempotencyKey);

        if (transaction is not null)
            return transaction.Id;

        var withdrawal = Transaction.CreateWithdrawal(request.SourceAccountId, request.Amount, request.Currency, request.IdempotencyKey);
        await transactionRepository.AddAsync(withdrawal);

        var transactionCreatedEvent = new TransactionCreatedEvent(
            withdrawal.Id,
            withdrawal.SourceAccountId,
            withdrawal.DestinationAccountId,
            withdrawal.Amount,
            withdrawal.Currency,
            withdrawal.Type.ToString()
        );

        await messagePublisher.PublishAsync(transactionCreatedEvent);

        // TODO: marcar como Complete() ou Fail() após Account Service confirmar a atualização do saldo via evento (Fase 5)

        return withdrawal.Id;
    }
}