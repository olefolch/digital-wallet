using MediatR;

namespace AccountService.Application.Features.Accounts.Commands.Withdraw;

public record WithdrawCommand(Guid AccountId, decimal Amount) : IRequest;