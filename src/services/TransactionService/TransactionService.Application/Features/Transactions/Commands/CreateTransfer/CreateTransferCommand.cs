using MediatR;

namespace TransactionService.Application.Features.Transactions.Commands.CreateTransfer;

public record CreateTransferCommand(Guid SourceAccountId, Guid DestinationAccountId, decimal Amount, string Currency, string IdempotencyKey) : IRequest<Guid>;