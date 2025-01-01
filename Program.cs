using System;
using Bot.Core; // Assuming CoreProcessor is defined in Bot.Core namespace

class Program
{
    static void Main()
    {
        string script = "";
        Console.WriteLine("Which script would you like to run?");
        script = Console.ReadLine();

        Console.WriteLine("Starting bot...");

        CoreProcessor processor = new CoreProcessor();
        processor.startProcessor(script);

        // Wait for user input before closing the console window
        Console.ReadKey();
        
    }
}
