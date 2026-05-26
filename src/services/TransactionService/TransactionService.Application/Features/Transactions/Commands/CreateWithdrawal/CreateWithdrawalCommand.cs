using MediatR;

namespace TransactionService.Application.Features.Transactions.Commands.CreateWithdrawal;

public record CreateWithdrawalCommand(Guid SourceAccountId, decimal Amount, string Currency, string IdempotencyKey) : IRequest<Guid>;