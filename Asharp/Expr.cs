namespace Asharp;

public abstract class Expr
{
    public interface IVisitor<R>
    {
        R VisitBinaryExpr(Binary expr);
        R VisitUnaryExpr(Unary expr);
        R VisitLiteralExpr(Literal expr);
    }

    public abstract R Accept<R>(IVisitor<R> expr);

    public sealed class Binary(Expr left, SyntaxToken @operator, Expr right) : Expr
    {
        public Expr Left { get; } = left;
        public SyntaxToken Operator { get; } = @operator;
        public Expr Right { get; } = right;

        public override R Accept<R>(IVisitor<R> visitor)
        {
            return visitor.VisitBinaryExpr(this);
        }
    }

    public sealed class Unary(SyntaxToken op, Expr right) : Expr
    {
        public Expr Right { get; } = right;
        public SyntaxToken Operator { get; } = op;

        public override R Accept<R>(IVisitor<R> visitor)
        {
            return visitor.VisitUnaryExpr(this);
        }
    }

    public sealed class Literal(object value) : Expr
    {
        public object Value { get; } = value;

        public override R Accept<R>(IVisitor<R> visitor)
        {
            return visitor.VisitLiteralExpr(this);
        }
    }
}
