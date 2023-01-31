using Presentation.Layer.Services.ConsoleHandler;

namespace Presentation.Layer;

public class Program
{
    public static void Main()
    {
        var consoleHandler = new ConsoleHandler();
        Console.WriteLine(consoleHandler.ShowCommands());
        string? command;

        while ((command = Console.ReadLine()) != "/exit")
        {
            string[] arguments = command?.Split(" ") ?? throw new ArgumentNullException();
            Console.WriteLine(consoleHandler.ExecuteCommand(arguments));
        }
    }
}