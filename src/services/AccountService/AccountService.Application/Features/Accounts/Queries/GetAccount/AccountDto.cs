namespace AccountService.Application.Features.Accounts.Queries.GetAccount;

public record AccountDto
{
    public Guid Id { get; init; }
    public Guid OwnerId { get; init; }
    public decimal Balance { get; init; }
    public string Currency { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
}