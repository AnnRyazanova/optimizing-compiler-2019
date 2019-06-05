# �������� ������
�������� �������� �� ������ Def-Use � �������� �������� �����.

## ���������� ������
��������� ��������� ���	������	��	������	���������,	���	������ ���	�������� � �������� �������� �����. 

## ������� � �����������
AW

## �����������
������� ��:
- ������������ ��� (AW)
- ���������� Def-Use � �������� �������� ����� (AW)

## ������
�������� �������� ������� � ����������� ������������� (Use) � ����������� �� ����� ���� ������������� ��������, ������� �������� ������ ������ ��������� ������������ ��� ����������� ������ ���������� (Def).
��������������� �������� ����������	���	������	��	������	���������,	���	������ ���	�������� � �������� �������� �����. 

## ����������
����� ��� �������� �������� �� ������ Def-Use � �������� �������� �����:
```csharp
using VarNodePair = System.Tuple<string, System.Collections.Generic.LinkedListNode<SimpleLang.TACode.TacNodes.TacNode>>;
...
/// <summary>
/// ����� �������� Def � Use ���� ���������� � �������� �������� �����
/// </summary>
private readonly DefUseDetector _detector; 
...
/// <summary>
/// �����, ����������� �������� ��������, ���� ��� ��������. 
/// ������ ����������� ����� ��������� ��������� ��� ������, �� ��� ���, 
/// ���� ����� ��������� � ���� ������������� ���� �� ���������� ����������.
/// </summary>
/// <param name="tac"> ������������ ��� ��������� </param>
/// <returns> ���� ��������� ����������� ������� ������������ ���, �.�. ����������� 
/// ���� ���������, �� ���������� true, ����� - false </returns>
public bool Optimize(ThreeAddressCode tac)
{
    var initialTac = tac.ToString();

    // ��������� ������ �� ��������� node ������������� ����
    var node = tac.Last;

    // ����� ������������� ���� ����� - �����
    while (node != null)
    {
        if (node.Value is TacAssignmentNode assignment)
        {
            if (assignment.SecondOperand == null && Utility.Utility.IsNum(assignment.FirstOperand))
            {
                var key = new VarNodePair(assignment.LeftPartIdentifier, node);
                if (_detector.Definitions.ContainsKey(key))
                {
                    foreach (var usage in _detector.Definitions[key])
                    {
                        ChangeByConst(usage.Value, assignment);
                    }
                    _detector.Definitions.Remove(key);
                }
            }
        }
        node = node.Previous;
    }
    return !initialTac.Equals(tac.ToString());
}
```

---
��������������� ������, ������� ���� ������������� ��� �������� �������� �� ������ Def-Use � �������� �������� �����:

```csharp
/// <summary>
/// ��������: �������� �� ������� ������ (int ��� double) 
/// </summary>
/// <param name="expression"> �������, ������� ��������� </param>
/// <returns> ���� ������� �������� ������, �� ����� ���������� true, 
/// ����� - false </returns>
public static bool IsNum(string expression) => int.TryParse(expression, out _) == true
    || double.TryParse(expression, out _) == true;

/// <summary>
/// ������ ������������� �� ���������, ������� �����������
/// </summary>
/// <param name="node"> ���� � ������������ ���������� </param>
/// <param name="replacingNode"> ����, � ������� ����� ���������� ������ ������������� �� ��������� </param>
private void ChangeByConst(TacNode node, TacAssignmentNode replacingNode)
{
    switch (node) {
        case TacAssignmentNode assNode:
            if (assNode.FirstOperand.Equals(replacingNode.LeftPartIdentifier))
            {
                assNode.FirstOperand = replacingNode.FirstOperand;
            } 
                    
            if (assNode.SecondOperand != null && assNode.SecondOperand.Equals(replacingNode.LeftPartIdentifier))
            {
                assNode.SecondOperand = replacingNode.FirstOperand;
            }
            break;
        case TacIfGotoNode ifGotoNode:
            //ifGotoNode.Condition = replacingNode.FirstOperand;
            break;
    }
}
```

## �����
������ ��� ������ ��������� ����� � �����������.

## �����
��������� �����, ��������� ����, �� ������ ��������� �������� �������� �� ������ Def-Use � �������� �������� �����. 

