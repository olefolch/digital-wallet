namespace TransactionService.Application.Common;

public interface IMessagePublisher
{
    Task PublishAsync<T>(T message) where T : class;
}