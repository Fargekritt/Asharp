using System.Runtime.InteropServices;

namespace Asharp;

// recursive descent parser
public class Parser(List<SyntaxToken> tokens)
{
    private sealed class ParseError(string message) : Exception(message)
    {
    }

    private List<SyntaxToken> _tokens = tokens;
    private int _current;
    private bool IsAtEnd => Peek.Kind == SyntaxKind.EOF;
    private SyntaxToken Next => _tokens[_current + 1];
    private SyntaxToken Previous => _tokens[_current - 1];
    private SyntaxToken Peek => _tokens[_current];

    public List<Expr> Parse()
    {
        var exprs = new List<Expr>();
        while (!IsAtEnd)
        {
            exprs.Add(Term());
        }
        return exprs;
    }

    private Expr Term()
    {
        var expr = Factor();

        while (Match(SyntaxKind.MinusToken, SyntaxKind.PlusToken))
        {
            var op = Previous;
            var right = Factor();
            expr = new Expr.Binary(expr, op, right);
        }
        return expr;
    }

    private Expr Factor()
    {
        var expr = Unary();

        while (Match(SyntaxKind.StarToken, SyntaxKind.SlashToken))
        {
            var op = Previous;
            var right = Unary();
            expr = new Expr.Binary(expr, op, right);
        }
        return expr;
    }

    private Expr Unary()
    {
        if (Match(SyntaxKind.BangToken, SyntaxKind.MinusToken))
        {
            var op = Previous;
            var right = Unary();
            return new Expr.Unary(op, right);
        }
        return Primary();

    }
    private Expr Primary()
    {
        if (Match(SyntaxKind.NumberToken)) return new Expr.Literal(Previous.Value!);
        throw new ParseError("Invalid syntax");
    }

    private bool Match(params SyntaxKind[] kinds)
    {
        foreach (var token in kinds)
        {
            if (Peek.Kind == token)
            {
                Advance();
                return true;
            }
        }
        return false;
    }

    private bool Check(SyntaxKind kind)
    {
        if (IsAtEnd) return false;
        return Peek.Kind == kind;
    }
    private SyntaxToken Advance()
    {
        if (!IsAtEnd) _current++;
        return Previous;
    }
}