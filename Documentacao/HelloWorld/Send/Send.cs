using RabbitMQ.Client;
using System.Text;
class Send
{
    public static void Main()
    {
        var factory = new ConnectionFactory() { HostName = "localhost"}; 

        using (var channel = factory.CreateConnection().CreateModel())
        {
            var queueName = "hello";

            channel.QueueDeclare(
                queue: queueName,
                durable: true,
                exclusive: false,
                arguments: null
            );

            var message = string.Empty;

            while (true)
            {
                Console.Write("\n Insert a message: ");
                message = Console.ReadLine();

                if (!string.IsNullOrEmpty(message))
                {
                    channel.BasicPublish(
                    exchange: string.Empty,
                    routingKey: queueName,
                    basicProperties: null,
                    body: Encoding.UTF8.GetBytes(message));

                    Console.WriteLine($" Sent message: {message}");
                } else {
                    Console.WriteLine(" Invalid message!");
                }
            }
        }
    }
    
}

