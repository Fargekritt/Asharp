using System.Security.AccessControl;

namespace Asharp;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, Worsld!");
        var lexer = new Lexer("3 + 5 * 2");
        foreach (var token in lexer.Tokens)
        {
            Console.WriteLine(token.ToString());
        }
    }
}
