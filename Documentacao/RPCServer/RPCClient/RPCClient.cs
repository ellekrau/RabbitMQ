class RPCClient
{
    static void Main()
    {
        var client = new Client();

        Console.Write(" Insert a number: ");
        // var input = Console.ReadLine() ?? string.Empty;
        var input = "50";

        Console.WriteLine($"\n Response: {client.Call(input)}");

        client.Close();

        Console.ReadLine();
    }
}