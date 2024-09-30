using System.Security.AccessControl;

namespace Asharp;

internal static class Program
{
    private static void Main(string[] args)
    {
        switch (args.Length)
        {
            case 1:
                {
                    File(args.First());
                    break;
                }
            case 0:
                Repl();
                break;
        }
    }

    private static void Repl()
    {
        while (true)
        {
            Console.Write("> ");
            var line = Console.ReadLine();
            if (line == "exit") return;
            var lexer = new Lexer(line!);
            var parser = new Parser(lexer.Tokens);
            var astPrinter = new AstPrinter();
            foreach (var expr in parser.Parse())
            {
                Console.WriteLine(astPrinter.Print(expr));
            }


        }
    }

    private static void File(string path)
    {
        using StreamReader reader = new(path);
        var text = reader.ReadToEnd();
        var lexer = new Lexer(text);
        foreach (var token in lexer.Tokens)
        {
            Console.WriteLine(token.ToString());
        }
        var astPrinter = new AstPrinter();
        var parser = new Parser(lexer.Tokens);
        foreach (var expr in parser.Parse())
        {
            Console.WriteLine(astPrinter.Print(expr));
        }
    }
}
