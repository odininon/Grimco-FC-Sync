using CommandLine;
using ConsoleTool.Commands;

namespace ConsoleTool;

public static class Program
{
    private static void Main(string[] args)
    {
        Parser.Default.ParseArguments<UpdateDBCommand, GenerateAPIKeyCommand>(args)
            .WithParsed<ICommand>(t => t.Execute());
    }
}