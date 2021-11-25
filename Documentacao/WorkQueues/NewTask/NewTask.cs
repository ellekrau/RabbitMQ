using RabbitMQ.Client;
using System.Text;

class NewTask
{
    static void Main()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };

        using (var channel = factory.CreateConnection().CreateModel())
        {
            var routingKey = "task_queue";

            channel.QueueDeclare(
                queue: routingKey,
                durable: false,
                exclusive: false,
                autoDelete: false
            );

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            while(true)
            {
                channel.BasicPublish(
                    exchange: "",
                    routingKey: routingKey,
                    basicProperties: properties,
                    body: Encoding.UTF8.GetBytes(GetMessage())
                );
            }
        }
    }

    static string GetMessage()
    {
        var message = string.Empty;

        do
        {
            Console.Write(" Insert a message: ");
            message = Console.ReadLine();
        } while (string.IsNullOrWhiteSpace(message));
        
        return message;
    }
}