using MediatR;

namespace AccountService.Application.Features.Accounts.Commands.Deposit;

public record DepositCommand(Guid AccountId, decimal Amount) : IRequest;