using MassTransit;
using TransactionService.Application.Common;

namespace TransactionService.Infrastructure.Messaging;

public class MessagePublisher(IPublishEndpoint publishEndpoint) : IMessagePublisher
{
    public async Task PublishAsync<T>(T message) where T : class
    {
        await publishEndpoint.Publish(message);
    }
}