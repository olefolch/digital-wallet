namespace SharedContracts.Events;

public record TransactionCreatedEvent(
    Guid TransactionId,
    Guid SourceAccountId,
    Guid? DestinationAccountId,
    decimal Amount,
    string Currency,
    string Type
);