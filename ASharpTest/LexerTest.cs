using Asharp;
using NUnit.Framework.Constraints;

namespace ASharpTest;

[TestFixture]
public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    [TestCaseSource(nameof(Files))]
    public void TestTokenCount(string filename)
    {
        using StreamReader reader = new(filename);
        var text = reader.ReadToEnd();
        var lexer = new Lexer(text);
        Assert.That(lexer.Tokens, Has.Count.EqualTo(7));
    }

    [Test]
    [TestCaseSource(nameof(Files))]
    public void TestLineNumber(string filename)
    {
        using StreamReader reader = new(filename);
        var text = reader.ReadToEnd();
        var lexer = new Lexer(text);
        Assert.That(lexer.Tokens[4].Line, Is.EqualTo(1));
        Assert.That(lexer.Tokens[2].Line, Is.EqualTo(0));
    }

    private static IEnumerable<string> Files()
    {
        yield return "LF.as";
        yield return "CRLF.as";
        yield return "CR.as";
    }
}