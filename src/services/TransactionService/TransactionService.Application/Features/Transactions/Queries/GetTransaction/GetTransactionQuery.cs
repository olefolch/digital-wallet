using MediatR;

namespace TransactionService.Application.Features.Transactions.Queries.GetTransaction;

public record GetTransactionQuery(Guid TransactionId) : IRequest<TransactionDto>;