using MassTransit;
using Microsoft.Extensions.Configuration;
using NotificationService.Application.Services;
using SharedContracts.Events;

namespace NotificationService.Infrastructure.Consumers;

public class TransactionCreatedConsumer(TransactionNotificationService notificationService, IConfiguration configuration) : IConsumer<TransactionCreatedEvent>
{
    public async Task Consume(ConsumeContext<TransactionCreatedEvent> context)
    {
        // TODO: buscar e-mail real do usuário via Account/Auth Service (Fase 6)
        var recipientEmail = configuration["Email:DefaultRecipient"]!;

        await notificationService.HandleAsync(context.Message, recipientEmail);
    }
}