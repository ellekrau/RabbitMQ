using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

class ScreenLog
{
    static void Main()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };

        using (var channel = factory.CreateConnection().CreateModel())
        {
            var exchangeName = "logs";
            var queueName = channel.QueueDeclare().QueueName;

            channel.ExchangeDeclare(
                exchange: exchangeName,
                type: ExchangeType.Fanout
            );

            channel.QueueBind(
                queue: queueName,
                exchange: exchangeName,
                routingKey: string.Empty
            );

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                Console.WriteLine($" Log: {message}");
            };

            Console.WriteLine("\n ======= SCREEN LOGS =======\n");

            channel.BasicConsume(
                queue: queueName,
                autoAck: true,
                consumer: consumer
            );

            Console.ReadLine();
        }
    }
}