using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;


public class AlarmEvent
{
    public string Id { get; set; }
    public string Description { get; set; }
    public DateTime Timestamp { get; set; }
}

public class Ticket
{
    public string TicketId { get; set; }
    public string AlarmEventId { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
}
class Publicar
{
    const string connectionString = "Endpoint=sb://pagoproveedores.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=W+CODu7cj4zIvhWsQC+4QQF9mn6vZEVus+ASbGY8Jq4=";
    const string queueName = "pagos";

    static async Task Main(string[] args)
    {
        await SendAlarmEventAsync(new AlarmEvent
        {
            Id = Guid.NewGuid().ToString(),
            Description = "Hola Mateo Pillajo de la clase de Arquitectura y diseño",
            Timestamp = DateTime.UtcNow
        });
    }

    static async Task SendAlarmEventAsync(AlarmEvent alarmEvent)
    {
        ServiceBusClient client = new ServiceBusClient(connectionString);
        ServiceBusSender sender = client.CreateSender(queueName);

        string messageBody = JsonConvert.SerializeObject(alarmEvent);
        ServiceBusMessage message = new ServiceBusMessage(messageBody);

        await sender.SendMessageAsync(message);
        Console.WriteLine($"Alarm event sent: {alarmEvent.Id}");

        await client.DisposeAsync();
    }
}