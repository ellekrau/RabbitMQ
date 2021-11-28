using RabbitMQ.Client;
using System.Text;

class CreateLogs
{
    static void Main()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };

        using (var channel = factory.CreateConnection().CreateModel())
        {
            var exchangeName = "topic_logs";

            channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Topic);

            while (true)
            {
                var log = CreateLog();

                channel.BasicPublish(
                    exchange: exchangeName,
                    routingKey: log.routingKey,
                    body: Encoding.UTF8.GetBytes(log.content)
                );

                Console.WriteLine($"\n => Sent: [{log.routingKey}] {log.content}");
            }
        }
    }

    static (string routingKey, string content) CreateLog()
    {
        Console.Write("\n =========== NEW LOG ===========\n");

        var routingKey = $"{GetLogFacility()}.{GetLogSeverity()}";
        Console.WriteLine();
        var content = GetLogContent();

        return new(routingKey, content);
    }

    static string GetLogContent()
    {
        var content = string.Empty;

        while (string.IsNullOrWhiteSpace(content))
        {
            Console.Write(" Insert the content: ");
            content = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(content))
                Console.WriteLine(" Invalid!");
        }

        return content;
    }

    static string GetLogFacility()
    {
        var facility = string.Empty;
        var options = new string[] { "ZAYA", "ORUS", "BETA" };

        Console.WriteLine("\n [0] ZAYA [1] ORUS [2] BETA");

        while (string.IsNullOrWhiteSpace(facility))
        {
            Console.Write(" Insert the machine's ID: ");
            facility = options.ElementAtOrDefault(int.Parse(Console.ReadKey().KeyChar.ToString()))
                       ?? string.Empty;

            if (string.IsNullOrWhiteSpace(facility))
                Console.WriteLine("\n Invalid!");
        }

        Console.WriteLine();

        return facility;
    }

    static string GetLogSeverity()
    {
        var severity = string.Empty;
        var options = new string[] { "ERROR", "WARN", "INFO" };

        Console.WriteLine("\n [0] ERROR [1] WARN [2] INFO");

        while (string.IsNullOrWhiteSpace(severity))
        {
            Console.Write(" Insert the log's severity: ");
            severity = options.ElementAtOrDefault(int.Parse(Console.ReadKey().KeyChar.ToString()))
                       ?? string.Empty;

            if (string.IsNullOrWhiteSpace(severity))
                Console.WriteLine("\n Invalid!");
        }

        Console.WriteLine();

        return severity;
    }
}