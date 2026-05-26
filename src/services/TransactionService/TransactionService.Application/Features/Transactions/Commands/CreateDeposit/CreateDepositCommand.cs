using MediatR;

namespace TransactionService.Application.Features.Transactions.Commands.CreateDeposit;

public record CreateDepositCommand(Guid SourceAccountId, decimal Amount, string Currency, string IdempotencyKey) : IRequest<Guid>;