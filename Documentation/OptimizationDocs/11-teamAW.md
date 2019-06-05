# �������� ������
����������� �� ������ �11: ������ `if (true) st1; else st2;` �� `st1`.

## ���������� ������
�������� ��������� ���� `if (true) st1; else st2;` �� `st1` � ������� ��������.

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
class IfNodeWithBoolExprVisitor : ChangeVisitor
{
    public override void VisitIfNode(IfNode inode)
    {
        if (inode.Expr is BoolNode bln && bln.Value == true)
        {
            inode.Stat1.Visit(this);
            ReplaceStatement(inode, inode.Stat1);
        }
        else
        {
            base.VisitIfNode(inode);
        }
    }
}
```

����� ��� ���������� ����� ReplaceStatement ��� �������� ChangeVisitor, ������� ��������� �������� StatementNode:

```csharp
public void ReplaceStatement(StatementNode from, StatementNode to)
{
    var p = from.Parent;
    if (p is AssignNode || p is ExprNode)
    {
        throw new Exception("������������ ���� �� �������� ����������");
    }
    to.Parent = p;
    if (p is BlockNode bln)
    {
        for (var i = 0; i < bln.StList.Count; ++i)
        {
            if (bln.StList[i] == from)
            {
                bln.StList[i] = to;
                break;
            }
        }
    }
    else if (p is IfNode ifn)
    {
        if (ifn.Stat1 == from)
        {
            ifn.Stat1 = to;
        }
        else if (ifn.Stat2 == from)
        {
            ifn.Stat2 = to;
        }
    }
    else
    {
        throw new Exception("ReplaceStatement �� ��������� ��� ������� ���� ����");
    }
}
```

## �����
������ ��� ������ ��������� ����� � �����������.

## �����
��������� �����, ��������� ����, �� �������� �������, ���������� ��������� ���� `if (true) st1; else st2;` �� `st1`.
