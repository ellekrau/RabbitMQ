using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading;

class Worker
{
    static void Main()
    {
        var factory = new ConnectionFactory { HostName = "localhost" };

        using (var channel = factory.CreateConnection().CreateModel())
        {
            var routingKey = "task_queue";

            channel.QueueDeclare(
                queue: routingKey,
                durable: false,
                exclusive: false,
                autoDelete: true
            );

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                Console.WriteLine($" Received: {message}");

                var sleepTime = (message.Split(".").Length - 1) * 1000;
                Console.WriteLine($" Sleep time: {sleepTime} milliseconds");
                
                Console.WriteLine(" Starting...");
                Thread.Sleep(sleepTime);
                Console.WriteLine(" Done!");

                Console.WriteLine("======================================");
            };

            while (true)
            {
                channel.BasicConsume(queue: routingKey, autoAck: true, consumer: consumer);
            }
        }
    }
}