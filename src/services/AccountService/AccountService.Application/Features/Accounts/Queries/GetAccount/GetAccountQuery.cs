using MediatR;

namespace AccountService.Application.Features.Accounts.Queries.GetAccount;

public record GetAccountQuery(Guid AccountId) : IRequest<AccountDto>;