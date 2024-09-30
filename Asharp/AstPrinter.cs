using System.Text;

namespace Asharp;

public class AstPrinter : Expr.IVisitor<string>
{
    public string Print(Expr expr)
    {
        return expr.Accept(this);
    }
    public string VisitBinaryExpr(Expr.Binary expr)
    {
        return PrettyPrint(expr.Operator.Text, expr.Left, expr.Right);
    }
    public string VisitUnaryExpr(Expr.Unary expr)
    {
        return PrettyPrint(expr.Operator.Text, expr.Right);
    }
    public string VisitLiteralExpr(Expr.Literal expr)
    {
        if (expr.Value == null) return "nill";
        return expr.Value.ToString();
    }
    private string PrettyPrint(string name, params Expr[] exprs)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append('(').Append(name);
        foreach (var expr in exprs)
        {
            stringBuilder.Append(' ').Append(expr.Accept(this));
        }
        stringBuilder.Append(')');
        return stringBuilder.ToString();
    }
}
