using System.Security.AccessControl;

namespace Asharp;

class Program
{
    static void Main(string[] args)
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

    static void Repl()
    {
        while (true)
        {
            Console.Write("> ");
            var line = Console.ReadLine();
            if(line == "exit") return;
            var lexer = new Lexer(line!);
            foreach (var token in lexer.Tokens)
            {
                Console.WriteLine(token.ToString());
            }
        }
    }

    static void File(string path)
    {
        using StreamReader reader = new(path);
        var text = reader.ReadToEnd();
        var lexer = new Lexer(text);
        foreach (var token in lexer.Tokens)
        {
            Console.WriteLine(token.ToString());
        }

    }
}