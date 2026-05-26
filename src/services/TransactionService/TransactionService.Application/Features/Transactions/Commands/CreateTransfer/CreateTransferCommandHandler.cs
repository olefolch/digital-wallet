using MediatR;
using TransactionService.Application.Common;
using TransactionService.Application.Events;
using TransactionService.Domain.Aggregates;
using TransactionService.Domain.Interfaces;

namespace TransactionService.Application.Features.Transactions.Commands.CreateTransfer;

public class CreateTransferCommandHandler(ITransactionRepository transactionRepository, IMessagePublisher messagePublisher) : IRequestHandler<CreateTransferCommand, Guid>
{
    public async Task<Guid> Handle(CreateTransferCommand request, CancellationToken cancellationToken)
    {
        var transaction = await transactionRepository.GetByIdempotencyKeyAsync(request.IdempotencyKey);

        if (transaction is not null)
            return transaction.Id;

        var transfer = Transaction.CreateTransfer(request.SourceAccountId, request.DestinationAccountId, request.Amount, request.Currency, request.IdempotencyKey);
        await transactionRepository.AddAsync(transfer);

        var transactionCreatedEvent = new TransactionCreatedEvent(
            transfer.Id,
            transfer.SourceAccountId,
            transfer.DestinationAccountId,
            transfer.Amount,
            transfer.Currency,
            transfer.Type.ToString()
        );

        await messagePublisher.PublishAsync(transactionCreatedEvent);

        // TODO: marcar como Complete() ou Fail() após Account Service confirmar a atualização do saldo via evento (Fase 5)

        return transfer.Id;
    }
}