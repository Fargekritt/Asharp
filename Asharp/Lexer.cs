namespace Asharp;

public class Lexer(string text)
{
    private int _position;

    private char Current => _position >= text.Length ? '\0' : text[_position];
    private char Peek => _position + 1 >= text.Length ? '\0' : text[_position + 1];
    private void Next()
    {
        _position++;
    }
    public SyntaxToken NextToken()
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
                        while (Current is not '\n' or '\0') Next();
                    }
                    else
                    {
                        return new SyntaxToken(SyntaxKind.SlashToken, _position, "/");
                    }
                }
                break;
        }
        if (char.IsDigit(Current))
        {
            var start = _position;
            while (char.IsDigit(Current)) Next();
            var length = _position - start;
            _ = int.TryParse(text.AsSpan(start, length), out var value);
            var text1 = text.Substring(start, length);
            return new SyntaxToken(SyntaxKind.NumberToken, start, text1, value);
        }
        return new SyntaxToken(SyntaxKind.BadToken, _position, "Bad Token");
    }
}

public class SyntaxToken(SyntaxKind kind, int position, string text, object? value = null)
{
    public object? Value { get; } = value;
    public int Position { get; set; } = position;
    public SyntaxKind Kind { get; set; } = kind;
    public string Text { get; set; } = text;
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
    BadToken
}
