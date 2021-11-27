﻿using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

class HighPriorityLogs
{
    static void Main()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };

        using (var channel = factory.CreateConnection().CreateModel())
        {
            var exchangeName = "directLogs";

            channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Direct);

            var queueName = channel.QueueDeclare().QueueName;

            channel.QueueBind(
                queue: queueName,
                exchange: exchangeName,
                routingKey: "ERROR"
            );

            Console.WriteLine("\n ===== High Priority Logs =====\n");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                Console.WriteLine($" [{ea.RoutingKey}] Log: {message}");
            };

            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

            Console.ReadKey();
        }
    }
}