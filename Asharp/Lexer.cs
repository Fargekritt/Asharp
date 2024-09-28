namespace Asharp;

public class Lexer
{
    private readonly string _text;
    private int _position;

    public Lexer(string text)
    {
        _text = text;
    }

    private char Current => _position >= _text.Length ? '\0' : _text[_position];
    private void Next()
    {
        _position++;
    }
    public SyntaxToken NextToken()
    {
        if (char.IsDigit(Current))
        {
            var start = _position;
            while (char.IsDigit(Current))
            {
                Next();
            }
            var length = _position - start;
            int.TryParse(_text.Substring(start, length), out var value);
            var text = _text.Substring(start, length);
            return new SyntaxToken(SyntaxKind.NumberToken, start, text, value);
        }
        
    }
}

public class SyntaxToken
{
    private readonly object _value;
    public SyntaxToken(SyntaxKind kind, int position, string text, object value)
    {
        _value = value;
        Kind = kind;
        Position = position;
        Text = text;
    }
    public int Position { get; set; }
    public SyntaxKind Kind { get; set; }
    public string Text { get; set; }
}

public enum SyntaxKind
{
    NumberToken
}
