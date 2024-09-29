using System.Security.AccessControl;

namespace Asharp;

class Program
{
    static void Main(string[] args)
    {
        using StreamReader reader = new("main.as");
        var text = reader.ReadToEnd();
        Console.WriteLine(text);
        var lexer = new Lexer(text);
        lexer.ScanTokens();
        foreach (var token in lexer.Tokens)
        {
            Console.WriteLine(token.ToString());
        }
    }
}
