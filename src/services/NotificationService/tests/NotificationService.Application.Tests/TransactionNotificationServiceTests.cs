using NotificationService.Application.Interfaces;
using NotificationService.Application.Services;
using NSubstitute;
using SharedContracts.Events;

namespace NotificationService.Application.Tests;

public class TransactionNotificationServiceTests
{
    private readonly IEmailSender _emailSender = Substitute.For<IEmailSender>();
    private readonly TransactionNotificationService _sut;

    public TransactionNotificationServiceTests()
    {
        _sut = new TransactionNotificationService(_emailSender);
    }

    [Fact]
    public async Task HandleAsync_WhenDeposit_ShouldSendEmailWithCorrectSubject()
    {
        // Arrange
        var @event = new TransactionCreatedEvent(Guid.NewGuid(), Guid.NewGuid(), null, 100, "BRL", "Deposit");

        // Act
        await _sut.HandleAsync(@event, "user@test.com");

        // Assert
        await _emailSender.Received(1).SendAsync("user@test.com", "Depósito confirmado", Arg.Any<string>());
    }

    [Fact]
    public async Task HandleAsync_WhenWithdrawal_ShouldSendEmailWithCorrectSubject()
    {
        // Arrange
        var @event = new TransactionCreatedEvent(Guid.NewGuid(), Guid.NewGuid(), null, 50, "BRL", "Withdrawal");

        // Act
        await _sut.HandleAsync(@event, "user@test.com");

        // Assert
        await _emailSender.Received(1).SendAsync("user@test.com", "Saque confirmado", Arg.Any<string>());
    }

    [Fact]
    public async Task HandleAsync_WhenTransfer_ShouldSendEmailWithCorrectSubject()
    {
        // Arrange
        var @event = new TransactionCreatedEvent(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 200, "BRL", "Transfer");

        // Act
        await _sut.HandleAsync(@event, "user@test.com");

        // Assert
        await _emailSender.Received(1).SendAsync("user@test.com", "Transferência confirmada", Arg.Any<string>());
    }

    [Fact]
    public async Task HandleAsync_ShouldSendEmailToCorrectRecipient()
    {
        // Arrange
        var @event = new TransactionCreatedEvent(Guid.NewGuid(), Guid.NewGuid(), null, 75, "BRL", "Deposit");
        const string recipientEmail = "leonardo@test.com";

        // Act
        await _sut.HandleAsync(@event, recipientEmail);

        // Assert
        await _emailSender.Received(1).SendAsync(recipientEmail, Arg.Any<string>(), Arg.Any<string>());
    }

    [Fact]
    public async Task HandleAsync_ShouldIncludeTransactionIdInBody()
    {
        // Arrange
        var transactionId = Guid.NewGuid();
        var @event = new TransactionCreatedEvent(transactionId, Guid.NewGuid(), null, 100, "BRL", "Deposit");

        // Act
        await _sut.HandleAsync(@event, "user@test.com");

        // Assert
        await _emailSender.Received(1).SendAsync(
            Arg.Any<string>(),
            Arg.Any<string>(),
            Arg.Is<string>(body => body.Contains(transactionId.ToString()))
        );
    }
}
