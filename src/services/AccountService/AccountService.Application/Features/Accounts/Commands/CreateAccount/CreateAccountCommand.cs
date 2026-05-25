using MediatR;

namespace AccountService.Application.Features.Accounts.Commands.CreateAccount;

public record CreateAccountCommand(Guid OwnerId, string Currency) : IRequest<Guid>;