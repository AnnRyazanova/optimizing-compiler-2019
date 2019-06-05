# �������� ������
���������� Def-Use � �������� �������� �����.

## ���������� ������
��� �������� ����� ����������� ���������� �������� Def-Use.

## ������� � �����������
AW

## �����������
������� ��:
- ������������ ��� (AW)

## ������
���������� � ������� ����� ����������� �� ��� ����: ����������� (Def) � ������������� (Use).

Def - ��� ����������� ����������, �.�. ����� � ���� ���������, ����� ���������� ������������� �����-���� ��������.

Use - ��� ������������� ����������, �.�. ����� � ���� ���������, ����� ���������� ��������� � ���������.

������ Use ������ ������ �� Def ���������� ��� null, ���� ���������� �� ������������ � ������ ������� �����. ������ Def ������ ������ Use ������ ���������� � �������� �������� �����.

```csharp
x = a;			// x ������������ (Def); � ������������� (Use)
...
y = x + z;		// y ������������ (Def); x, z ������������ (Use)
...
if (b) {		// b ������������� (Use)
...
}
```

## ����������
��� ���������� ������ ������ ���� ������ ������������ Def � Use ���������� ����:
```csharp
using LinkedListTacNode = System.Collections.Generic.LinkedListNode<SimpleLang.TACode.TacNodes.TacNode>;
using VarNodePair = System.Tuple<string, System.Collections.Generic.LinkedListNode<SimpleLang.TACode.TacNodes.TacNode>>;

/// <summary>
/// ����������� ���������� � ������� �����
/// ����: ���� ��� ���������� � ����, ��� ���������� ����������
/// ��������: ������ �����, ��� ������������ ������������� ������� ����������� ���������� 
/// </summary>
public readonly Dictionary<VarNodePair, List<LinkedListTacNode>> Definitions =
    new Dictionary<VarNodePair, List<LinkedListTacNode>>();

/// <summary>
/// ������������� ���������� � ������� �����
/// ����: ���� ��� ���������� � ����, ��� ���������� ������������
/// ��������: ����, ��� ������������ ����������� ������ ������������ ����������
/// </summary>
public readonly Dictionary<VarNodePair, LinkedListTacNode> Usages =
    new Dictionary<VarNodePair, LinkedListTacNode>();
```

����� ��� ������ � ���������� Def � Use ����������:

```csharp
public void DetectAndFillDefUse(ThreeAddressCode threeAddressCode)
{
    // ��������� ������ �� ��������� node ������������� ����
    var lastNode = threeAddressCode.Last;

    // ��������� ������� ��� �������� ������������� ����������, 
    //���� �� ����� ������� ����������� ����������
    // ���� - ��� ����������
    // �������� - ������, ��������� �� ������ �� node ������������� ����, 
    //��� ������������ ����������
    var tmpUsagesNodes = new Dictionary<string, List<LinkedListTacNode>>();

    // ����� ������������� ���� ����� - �����
    while (lastNode != null)
    {
        switch (lastNode.Value)
        {
            case TacAssignmentNode assignmentNode:
            {
                // ���������� ������������� ���������� (������� �������� � ���������) �� ��������� ������� tmpUsagesNodes
                FillTmpUsagesForNode(assignmentNode.FirstOperand, lastNode, tmpUsagesNodes);

                if (assignmentNode.SecondOperand != null)
                {
                    // ���������� ������������� ���������� (������� �������� � ���������, ���� �� ����) �� ��������� ������� tmpUsagesNodes
                    FillTmpUsagesForNode(assignmentNode.SecondOperand, lastNode, tmpUsagesNodes);
                }

                // �������� ����� ������ � ������� Definitions � ������� ��������������� ����������
                var keyNode = new VarNodePair(assignmentNode.LeftPartIdentifier, lastNode);
                Definitions[keyNode] = new List<LinkedListTacNode>();

                // ���� ��������� ������������� ����������, ����� ������������ � tmpUsagesNodes  
                if (tmpUsagesNodes.ContainsKey(keyNode.Item1))
                {
                    // ��������� ������ Definitions ������� ���������� �� tmpUsagesNodes
                    Definitions[keyNode] = tmpUsagesNodes[keyNode.Item1];

                    // ��������� Usages ������ �� tmpUsagesNodes
                    foreach (var tmp in tmpUsagesNodes[keyNode.Item1])
                    {
                        Usages[new VarNodePair(assignmentNode.LeftPartIdentifier, tmp)] = lastNode;
                    }

                    // ������� �� ���������� ������� ������ � ������������� ���������� 
                    tmpUsagesNodes.Remove(keyNode.Item1);
                }
                break;
            }
            case TacIfGotoNode ifGotoNode:
            {
                // � ������ goto �� ��������� ������ ������������� ����������, ������� ������ ��������� �� � tmpUsagesNodes
                FillTmpUsagesForNode(ifGotoNode.Condition, lastNode, tmpUsagesNodes);
                break;
            }
        }
        lastNode = lastNode.Previous;
    }

    // ���� ��� �����-�� ���������� ���� �������������, �� ��� ����������, ��������� Usages, Definitions ��� ������ ���������� ����� ������
    FillUsagesWithoutDefinitions(tmpUsagesNodes);
}        
```

---
��������������� ������, ������� ���� ������������� ��� ������ � ���������� ����������� � �������������:

```csharp
/// <summary>
/// ���������� � tmpUses ���������� operand ����, ��� ���� ������������� ������ ����������
/// </summary>
/// <param name="operand"> ����������, ��������������� � ������ ������ </param>
/// <param name="node"> ���� ������������� ����, ��� ������������ ������ ���������� </param>
/// <param name="tmpUses"> ��������� �������, ��� �������� ���������� �� ������������� ���������� </param>
private void FillTmpUsagesForNode(string operand, LinkedListTacNode node,
    IDictionary<string, List<LinkedListTacNode>> tmpUses)
{
    if (!Utility.Utility.IsVariable(operand)) return;
    if (tmpUses.ContainsKey(operand))
    {
        tmpUses[operand].Add(node);
    }
    else
    {
        tmpUses[operand] = new List<LinkedListTacNode>() {node};
    }
}

/// <summary>
/// ���������� Usages ��� ����������, � ������� ��� �����������
/// </summary>
/// <param name="tmpUses"> ��������� ������� ������������� </param>
private void FillUsagesWithoutDefinitions(Dictionary<string, List<LinkedListTacNode>> tmpUses)
{
    foreach (var tmpUse in tmpUses)
    {
        foreach (var usageTacNode in tmpUse.Value)
        {
            Usages[new VarNodePair(tmpUse.Key, usageTacNode)] = null;
        }
    }
}

```

## �����
������ ��� ������ ��������� ����� � �����������.

## �����
��������� ������, ��������� ����, �� �������� Def-Use ��� ������ ���������� � �������� �������� �����. ���� ��� �����-���� ���������� ��� �����������, ��� � ������������� ����� ����������� null. ���� ��� �����-���� ���������� ��� �������������, �� � ����������� ����� ������ ������ ������������� (Count == 0).
