using RabbitMQ.Client;
using System.Text;

class Client
{
    static void Main()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };

        using (var channel = factory.CreateConnection().CreateModel())
        {
            channel.ConfirmSelect();
            var queueName = channel.QueueDeclare().QueueName;

            IndividuallyConfirm(channel, queueName);
            BatchConfirm(channel, queueName);
            AsyncConfirm(channel, queueName);
        }
    }

    static void IndividuallyConfirm(IModel channel, string queueName)
    {
        var message = string.Empty;

        channel.BasicPublish(
            exchange: string.Empty,
            routingKey: queueName,
            body: Encoding.UTF8.GetBytes(message));

        channel.WaitForConfirmsOrDie(new TimeSpan(0, 0, 5));
    }

    static void BatchConfirm(IModel channel, string queueName)
    {
        var batchSize = 100;
        var outstandingMessageCount = 0;

        while (true)
        {
            var message = string.Empty;
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(
                exchange: string.Empty,
                routingKey: queueName,
                basicProperties: null);

            outstandingMessageCount++;

            if (outstandingMessageCount == batchSize)
            {
                channel.WaitForConfirmsOrDie(new TimeSpan(0, 0, 5));
                outstandingMessageCount = 0;
            }
        }
    }

    static void AsyncConfirm(IModel channel, string queueName)
    {
        channel.BasicAcks += (sender, ea) =>
        {
            Console.WriteLine($"Sent: {sender?.ToString()}");
        };

        channel.BasicNacks += (sender, ea) => 
        {
            Console.WriteLine($"Not sent: {sender?.ToString()}");
        };

        var message = string.Empty;

        channel.BasicPublish(
            exchange: string.Empty,
            routingKey: queueName,
            body: Encoding.UTF8.GetBytes(message)
        );
    }
}