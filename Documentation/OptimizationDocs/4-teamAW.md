# �������� ������
����������� �� ������ �4: ������ ��������� ���� `0 + expr` (`expr + 0`) �� `expr`.

## ���������� ������
�������� ��������� ���� `0 + expr` (`expr + 0`) �� `expr` � ������� ��������.

## ������� � �����������
AW

## �����������
������� ��:
- ������� ��������
- PrettyPrinter

## ������
&mdash;

## ����������
��� ������� ������������ ������ ��� ���������� �������, ����������� �� ChangeVisitor.

```csharp
class PlusZeroExprVisitor : ChangeVisitor
{
    public override void VisitBinOpNode(BinOpNode binop)
    {
        base.VisitBinOpNode(binop);
        if (binop.Op == "+")
        {
            ExprNode expr1 = binop.Left;
            ExprNode expr2 = binop.Right;
            if (expr1 is IntNumNode expr && expr.Num == 0)
            {
                ReplaceExpr(expr1.Parent as ExprNode, expr2);
            }
            else if (expr2 is IntNumNode exp && exp.Num == 0)
            {
                ReplaceExpr(expr2.Parent as ExprNode, expr1);
            }
        }
    }
}
```

## �����
������ ��� ������ ��������� ����� � �����������.

## �����
��������� �����, ��������� ����, �� �������� �������, ���������� ��������� ���� `0 + expr` (`expr + 0`) �� `expr`.