namespace Asharp;
// recursive descent parser
public class Parser(List<SyntaxToken> tokens)
{
    
    private List<SyntaxToken> _tokens = tokens;
    private int _current = 0;
    private bool IsAtEnd => Peek.Kind == SyntaxKind.EOF;
    private SyntaxToken Peek => _tokens[_current];
    private SyntaxToken Previous => _tokens[_current - 1];

    private Expr Term()
    {
        Expr expr = Factor();

        while (Match(SyntaxKind.MinusToken, SyntaxKind.PlusToken))
        {
            SyntaxToken op = Previous;
            Expr right = Factor();
            expr = new Expr.Binary(expr, op, right);
        }
    }

    private Expr Factor()
    {
        throw new NotImplementedException();
    }

    private bool Match(SyntaxKind[] kinds)
    {
        foreach (var token in kinds)
        {
            
        }
    }
}

public abstract class Expr
{
    public interface IVisitor<R>
    {
        R VisitBinaryExpr(Binary expr);
    }
    
    public abstract R Accept<R>(IVisitor<R> expr);
    
    public sealed class Binary(Expr left, SyntaxToken @operator, Expr right) : Expr
    {
        readonly Expr left = left;
        readonly SyntaxToken Operator = @operator;
        readonly Expr right = right;

        public override R Accept<R>(IVisitor<R> visitor)
        {
            return visitor.VisitBinaryExpr(this);
        }
    }
}

