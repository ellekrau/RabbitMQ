using RabbitMQ.Client;
using System.Text;


var factory = new ConnectionFactory() { HostName = "localhost" };

using (var channel = factory.CreateConnection().CreateModel())
{
    var exchangeName = "directLogs";

    channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Direct);

    while (true)
    {
        Console.Write("\n Insert the log level ([I]nfo, [W]arning, [E]rror): ");
        var severity = Console.ReadLine()?.ToUpper();
        
        switch (severity)
        {
            case "E": severity = "ERROR"; break;
            case "W": severity = "WARN"; break;
            case "I": severity = "INFO"; break;
            default: return;
        }

        Console.Write(" Insert the log message: ");
        var message = Console.ReadLine();

        channel.BasicPublish(
            exchange: exchangeName,
            routingKey: severity,
            basicProperties: null,
            body: Encoding.UTF8.GetBytes(message ?? string.Empty)
        );

        Console.WriteLine($" Sent: [{severity}] {message}");
    }
}