using System.Net;

namespace Asharp;

public class Lexer
{
    private int _position;
    private readonly string _text;
    public Lexer(string text)
    {
        _text = text;
    }
    public List<SyntaxToken> Tokens { get; private set; } = [];
    private char Current => _position >= _text.Length ? '\0' : _text[_position];
    private char Peek => _position + 1 >= _text.Length ? '\0' : _text[_position + 1];
    private bool EOF => _position >= _text.Length;
    private void Next()
    {
        _position++;
    }
    public void ScanTokens()
    {
        while (!EOF)
        {
            Tokens.Add(ScanToken());
            Next();
        }
    }
    private SyntaxToken ScanToken()
    {
        // TODO: Rework needed. Probably add token instead of returning token?
        // Hard to handle when there is no token to be return like " " or comment       
        while (true)
        {

            switch (Current)
            {
                case '(': return new SyntaxToken(SyntaxKind.LeftParenToken, _position, "(");
                case ')': return new SyntaxToken(SyntaxKind.RightParenToken, _position, ")");
                case '[': return new SyntaxToken(SyntaxKind.LeftBracketToken, _position, "[");
                case ']': return new SyntaxToken(SyntaxKind.RightBracketToken, _position, "]");
                case '{': return new SyntaxToken(SyntaxKind.LeftBraceToken, _position, "{");
                case '}': return new SyntaxToken(SyntaxKind.RightBraceToken, _position, "}");
                case '+': return new SyntaxToken(SyntaxKind.PlusToken, _position, "+");
                case '-': return new SyntaxToken(SyntaxKind.MinusToken, _position, "-");
                case '*': return new SyntaxToken(SyntaxKind.StarToken, _position, "*");
                case '/':
                    {
                        if (Peek == '/')
                        {
                            // comment
                            while (!IsNewline(Peek) && !EOF) Next();
                            // line++
                        }
                        else
                        {
                            return new SyntaxToken(SyntaxKind.SlashToken, _position, "/");
                        }
                    }
                    break;
                case ' ' or '\r' or '\t' or '\n' or (char)10:
                    {

                        while (Current is ' ' or '\r' or '\n' or (char)10 && !EOF)
                        {
                            Console.WriteLine((int)Current);
                            Next();
                        }
                        continue;
                    }
            }


            if (char.IsDigit(Current))
            {
                var start = _position;
                while (char.IsDigit(Current)) Next();
                var length = _position - start;
                _ = int.TryParse(_text.AsSpan(start, length), out var value);
                var text1 = _text.Substring(start, length);
                return new SyntaxToken(SyntaxKind.NumberToken, start, text1, value);
            }
            if (EOF)
            {
                return new SyntaxToken(SyntaxKind.EOF, _position, "EOF");
            }
            throw new Exception($"Cant find token for {Current}");
        }
    }
    private void AddToken(SyntaxKind kind, object? value = null)
    {
        // Tokens.Add(new SyntaxToken(kind, line, )); 
    }
    private bool IsNewline(char ch) => ch is '\r' || ch is '\n';
}

public class SyntaxToken(SyntaxKind kind, int position, string text, object? value = null)
{
    public object? Value { get; } = value;
    public int Position { get; set; } = position;
    public SyntaxKind Kind { get; set; } = kind;
    public string Text { get; set; } = text;

    public override string ToString()
    {
        return $"Pos {Position} Type {Kind} Lex {Text} Value {Value}";
    }
}

public enum SyntaxKind
{
    NumberToken,
    PlusToken,
    SlashToken,
    LeftParenToken,
    MinusToken,
    StarToken,
    RightParenToken,
    LeftBracketToken,
    RightBracketToken,
    LeftBraceToken,
    RightBraceToken,
    BadToken,
    EOF
}
