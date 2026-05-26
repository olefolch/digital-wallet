using MediatR;
using TransactionService.Domain.Exceptions;
using TransactionService.Domain.Interfaces;

namespace TransactionService.Application.Features.Transactions.Queries.GetTransaction;

public class GetTransactionQueryHandler(ITransactionRepository transactionRepository) : IRequestHandler<GetTransactionQuery, TransactionDto>
{
    public async Task<TransactionDto> Handle(GetTransactionQuery request, CancellationToken cancellationToken)
    {
        var transaction = await transactionRepository.GetByIdAsync(request.TransactionId);

        if (transaction is null)
            throw new DomainException("Transaction not found.");

        return new TransactionDto
        {
            Id = transaction.Id,
            SourceAccountId = transaction.SourceAccountId,
            DestinationAccountId = transaction.DestinationAccountId,
            Amount = transaction.Amount,
            Currency = transaction.Currency,
            Type = transaction.Type.ToString(),
            Status = transaction.Status.ToString(),
            CreatedAt = transaction.CreatedAt
        };
    }
}