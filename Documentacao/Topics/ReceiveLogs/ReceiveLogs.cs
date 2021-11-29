using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

class ReceiveLogs
{
    static void Main()
    {
        var bindingKeys = GetApplicationBindingKeys();

        var factory = new ConnectionFactory() { HostName = "localhost" };

        using (var channel = factory.CreateConnection().CreateModel())
        {
            var exchangeName = "topic_logs";
            channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Topic);

            var queueName = channel.QueueDeclare().QueueName;

            foreach (var bindingKey in bindingKeys)
            {
                channel.QueueBind(
                    exchange: exchangeName,
                    queue: queueName,
                    routingKey: bindingKey
                );

                Console.WriteLine($" Listening: {bindingKey}");
            }

            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            var consumer = new EventingBasicConsumer(channel);

            Console.WriteLine("\n =========== LOGS ===========\n");

            consumer.Received += (model, ea) =>
            {
                var facility = ea.RoutingKey.Split(".")[0];
                var severity = ea.RoutingKey.Split(".")[1];
                Console.WriteLine($" [{facility}] {severity} - {Encoding.UTF8.GetString(ea.Body.ToArray())}");
            };

            channel.BasicConsume(
                queue: queueName,
                autoAck: true,
                consumer: consumer
            );

            Console.ReadKey();
        }
    }

    static string[] GetApplicationBindingKeys()
    {
        Console.Write("\n Insert FACILITY.SEVERITY(,) values: ");
        // var configuration = "ZAYA.ERROR,BETA.*";
        var configuration = Console.ReadLine();


        if (string.IsNullOrWhiteSpace(configuration))
            Environment.Exit(0);

        Console.WriteLine();

        return configuration.ToUpper().Trim().Split(",");
    }
}