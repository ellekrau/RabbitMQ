using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

class RPCServer
{
    static void Main()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };

        using (var channel = factory.CreateConnection().CreateModel())
        {
            var queueName = "rpc_queue";

            channel.QueueDeclare(
                queue: queueName,
                durable: false,
                exclusive: false,
                autoDelete: false
            );

            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var response = string.Empty;
                var body = ea.Body.ToArray();

                var requestProperties = ea.BasicProperties;
                var responseProperties = channel.CreateBasicProperties();
                responseProperties.CorrelationId = requestProperties.CorrelationId;

                try
                {
                    var message = Encoding.UTF8.GetString(body);
                    var inputNumber = int.Parse(message);

                    Console.WriteLine($" Received: {inputNumber}");
                    response = fib(inputNumber).ToString();
                    Console.WriteLine($" Fib: {response}");

                }
                catch (Exception e)
                {
                    Console.WriteLine($" Exception: {e}");
                    response = string.Empty;
                }
                finally
                {
                    channel.BasicPublish(
                        exchange: string.Empty,
                        routingKey: requestProperties.ReplyTo,
                        basicProperties: responseProperties,
                        body: Encoding.UTF8.GetBytes(response)
                    );

                    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                }
            };

            Console.WriteLine("\n ========== RPC SERVER ==========\n");

            channel.BasicConsume(
                queue: queueName,
                autoAck: false,
                consumer: consumer
            );

            Console.ReadLine();
        }
    }

    static int fib(int n)
    {
        if (n == 0 || n == 1)
        {
            return n;
        }

        return fib(n - 1) + fib(n - 2);
    }
}