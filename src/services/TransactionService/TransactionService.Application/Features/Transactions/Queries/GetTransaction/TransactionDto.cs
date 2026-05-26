namespace TransactionService.Application.Features.Transactions.Queries.GetTransaction;

public record TransactionDto
{
    public Guid Id { get; init; }
    public Guid SourceAccountId { get; init; }
    public Guid? DestinationAccountId  { get; init; }
    public decimal Amount { get; init; }
    public string Currency { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
}