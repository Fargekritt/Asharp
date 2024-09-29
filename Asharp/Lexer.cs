using System.Net;

namespace Asharp;

public class Lexer
{
    private int _position;
    private readonly string _text;

    public Lexer(string text)
    {
        _text = text;
        ScanTokens();
    }

    public List<SyntaxToken> Tokens { get; private set; } = [];
    private char Current => _position >= _text.Length ? '\0' : _text[_position];
    private char Peek => _position + 1 >= _text.Length ? '\0' : _text[_position + 1];
    private bool EOF => _position >= _text.Length;

    private int _line = 0;

    private void Next()
    {
        _position++;
    }

    private void ScanTokens()
    {
        while (!EOF)
        {
            ScanToken();
            Next();
        }
        AddToken(SyntaxKind.EOF);
    }

   

    private void ScanToken()
    {
        switch (Current)
        {
            case '(':
                AddToken(SyntaxKind.LeftParenToken);
                break;
            case ')':
                AddToken(SyntaxKind.RightParenToken);
                break;
            case '[':
                AddToken(SyntaxKind.LeftBracketToken);
                break;
            case ']':
                AddToken(SyntaxKind.RightBracketToken);
                break;
            case '{':
                AddToken(SyntaxKind.LeftBraceToken);
                break;
            case '}':
                AddToken(SyntaxKind.RightBraceToken);
                break;
            case '+':
                AddToken(SyntaxKind.PlusToken);
                break;
            case '-':
                AddToken(SyntaxKind.MinusToken);
                break;
            case '*':
                AddToken(SyntaxKind.StarToken);
                break;
            case '\n':
                _line++;
                break;
            case '\r':
                if (Peek != '\n') _line++;
                break;
            case '/':
            {
                if (Peek == '/')
                {
                    while (!IsNewline(Current) && !EOF) Next();
                    _line++;
                }
                else
                {
                    AddToken(SyntaxKind.SlashToken);
                }
            }
                break;
            case ' ' or '\t':
            {
                while (Peek is ' ' or '\t' && !EOF)
                {
                    Console.WriteLine((int)Current);
                    Next();
                }

                break;
            }
            default:
                if (char.IsDigit(Current))
                {
                    var start = _position;
                    while (char.IsDigit(Peek)) Next();
                    var length = _position - start + 1;
                    _ = int.TryParse(_text.AsSpan(start, length), out var value);
                    var text = _text.Substring(start, length); 
                    AddToken(SyntaxKind.NumberToken, text, value);
                    break;
                }


                throw new Exception($"Cant find token for {Current}");
        }
    }

    private void AddToken(SyntaxKind kind, string? text = null, object? value = null)
    {
        if (EOF)
        {
            Tokens.Add(new SyntaxToken(kind, _line, "EOF", value));
            return;
        }    
        text ??= _text.Substring(_position, 1);
        
        Tokens.Add(new SyntaxToken(kind, _line, text, value));
    }

    private bool IsNewline(char ch)
    {
        if (ch == '\r')
        {
            if (Peek == '\n') return false;
            if (Peek != '\n') return true;
        }

        if (ch == '\n') return true;

        return false;
    }
}

public class SyntaxToken(SyntaxKind kind, int line, string text, object? value = null)
{
    public object? Value { get; } = value;
    public int Line { get; set; } = line;
    public SyntaxKind Kind { get; set; } = kind;
    public string Text { get; set; } = text;

    public override string ToString()
    {
        return $"Line: {Line}, Kind: {Kind}, Lexical {Text}, Value: {Value}";
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