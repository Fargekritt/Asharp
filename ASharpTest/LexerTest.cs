using Asharp;
using NUnit.Framework.Constraints;

namespace ASharpTest;

public class Tests
{
    Lexer _lexer;
    [SetUp]
    public void Setup()
    {
        _lexer = new Lexer("2 + 3\n5 + 2");
        _lexer.ScanTokens();
    }

    [Test]
    public void TestTokenCount()
    {

        Assert.That(_lexer.Tokens, Has.Count.EqualTo(7));
    }

    [Test]
    public void TestLineNumber()
    {
        Assert.That(_lexer.Tokens[4].Line, Is.EqualTo(1) );
        Assert.That(_lexer.Tokens[2].Line, Is.EqualTo(0) );
    }
}