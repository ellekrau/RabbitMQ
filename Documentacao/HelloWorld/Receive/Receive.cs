using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

class Receive
{
    static void Main()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        
        using (var channel = factory.CreateConnection().CreateModel())
        {
            var queueName = "hello";

             channel.QueueDeclare
             (
                queue: queueName,
                 durable: true,
                 exclusive: false,
                 autoDelete: true,
                 arguments: null
             );

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                Console.WriteLine($" Received: {message}");
            };     

            while (true)
            {
                channel.BasicConsume(
                    queue: queueName,
                    autoAck: true,
                    consumer: consumer);  
            }          
        }
    }

}