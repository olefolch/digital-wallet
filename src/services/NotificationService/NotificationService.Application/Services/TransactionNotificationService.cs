using NotificationService.Application.Interfaces;
using SharedContracts.Events;

namespace NotificationService.Application.Services;

public class TransactionNotificationService(IEmailSender emailSender)
{
    public async Task HandleAsync(TransactionCreatedEvent @event, string recipientEmail)
    {
        var subject = BuildSubject(@event.Type);
        var body = BuildBody(@event);

        await emailSender.SendAsync(recipientEmail, subject, body);
    }

    private static string BuildSubject(string type) => type switch
    {
        "Deposit" => "Depósito confirmado",
        "Withdrawal" => "Saque confirmado",
        "Transfer" => "Transferência confirmada",
        _ => "Transação confirmada"
    };

    private static string BuildBody(TransactionCreatedEvent @event) =>
        $"Transação {@event.TransactionId} do tipo {@event.Type} " +
        $"no valor de {@event.Amount:F2} {@event.Currency} foi registrada com sucesso.";
}