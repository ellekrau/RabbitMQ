using System.Collections.Concurrent;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

class Client
{
    private readonly IConnection connection;
    private readonly IModel channel;
    private readonly string replyQueueName;
    private readonly EventingBasicConsumer consumer;
    private readonly BlockingCollection<string> responseQueue = new BlockingCollection<string>();
    private readonly IBasicProperties properties;

    public Client()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        connection = factory.CreateConnection();
        channel = connection.CreateModel();

        var correlationId = Guid.NewGuid().ToString();
        replyQueueName = channel.QueueDeclare().QueueName;

        properties = channel.CreateBasicProperties();
        properties.CorrelationId = correlationId;
        properties.ReplyTo = replyQueueName;

        consumer = new EventingBasicConsumer(channel);

        consumer.Received += (model, ea) =>
        {
            var response = Encoding.UTF8.GetString(ea.Body.ToArray());

            if (ea.BasicProperties.CorrelationId == correlationId)
                responseQueue.Add(response);
        };

        channel.BasicConsume(
            consumer: consumer,
            queue: replyQueueName,
            autoAck: true
        );
    }

    public string Call(string message)
    {
        var correlationId = Guid.NewGuid().ToString();
        properties.CorrelationId = correlationId;

        channel.BasicPublish(
            exchange: string.Empty,
            routingKey: "rpc_queue",
            basicProperties: properties,
            body: Encoding.UTF8.GetBytes(message)
        );

        return responseQueue.Take();
    }

    public void Close()
    {
        connection.Close();
    }
}