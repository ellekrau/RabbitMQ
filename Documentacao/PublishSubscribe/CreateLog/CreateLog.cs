using RabbitMQ.Client;
using System.Text;

class CreateLog
{
    static void Main()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };

        using (var channel = factory.CreateConnection().CreateModel())
        {
            var exchangeName = "logs";

            channel.ExchangeDeclare(exchangeName, ExchangeType.Fanout);

            while (true)
            {
                channel.BasicPublish(
                   exchange: exchangeName,
                   routingKey: string.Empty,
                   body: Encoding.UTF8.GetBytes(CreateMessageContent())
                );
            }
        }
    }

    static string CreateMessageContent()
    {
        var message = string.Empty;

        while (string.IsNullOrWhiteSpace(message))
        {
            Console.Write("\n Insert the log: ");
            message = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(message))
                Console.WriteLine(" Invalid log message!");
        }
        Console.WriteLine(" Sent!");

        return message;
    }
}