# �������� ������
�������� IN-OUT.

## ���������� ������
�����������	�������� ���������� IN-OUT  �������� ��� ���� ������� ������ ���������.
## ������� � �����������
AW

## �����������
������� ��:
- ������������ ��� (AW)
- ��������� �� ������� ����� (Enterprise)
- ���������� �������� GenB � KillB (Kt, Enterprise)

## ������
![](../images/31-teamAW-1.PNG)
�������� ����� � �������� ��� �����������. ��� ������ ������ ���������� ����� �������� �����������.

## ����������
������ ��� ���������� � �������� In-Out �� ���� ������� ������:
```csharp
public Dictionary<OneBasicBlock, HashSet<TacNode>> In = new Dictionary<OneBasicBlock, HashSet<TacNode>>();
public Dictionary<OneBasicBlock, HashSet<TacNode>> Out = new Dictionary<OneBasicBlock, HashSet<TacNode>>();

/// <summary>
/// ���������� �������� In � Out �� ������ gen � kill ��� ������� �������� �����
/// </summary>
/// <param name="bBlocks"> ��� ������� ����� </param>
/// <param name="genKillContainers"> ��� ��������� gen � kill �� ���� ������� ������ </param>
public InOutContainer(BasicBlocks bBlocks,
    Dictionary<OneBasicBlock, IExpressionSetsContainer> genKillContainers)
{
    for (var i = 0; i < bBlocks.BasicBlockItems.Count; ++i)
    {
        var curBlock = bBlocks.BasicBlockItems[i];

        if (i == 0)
        {
            In[curBlock] = new HashSet<TacNode>();
        }
        else
        {
            var prevBlock = bBlocks.BasicBlockItems[i - 1];
            FillInForBasicBlock(curBlock, prevBlock);
        }

        FillOutForBasicBlock(curBlock, genKillContainers);
    }
}

/// <summary>
/// ��������� ��������� In ��� �������� �������� �����
/// �.�. ������ ����������� In - ��� ����������� Out ���� ���������� ������,
/// �� ���������� ����� ������ In � Out ����������� �����, ����� �������� 
/// �������� In �������� �����
/// </summary>
/// <param name="curBlock"> �������������� ������� ���� </param>
/// <param name="prevBlock"> ���������� ������� ���� </param>
public void FillInForBasicBlock(OneBasicBlock curBlock, OneBasicBlock prevBlock)
{
    In[curBlock] = new HashSet<TacNode>();
    In[curBlock].UnionWith(In[prevBlock]);
    In[curBlock].UnionWith(Out[prevBlock]);
}

/// <summary>
/// ��������� ��������� OUT ��� �������� �������� �����
/// </summary>
/// <param name="curBlock"> �������������� ������� ���� </param>
/// <param name="genKillContainers"> ���������� � gen � kill </param>
public void FillOutForBasicBlock(OneBasicBlock curBlock,
    Dictionary<OneBasicBlock, IExpressionSetsContainer> genKillContainers)
{
    if (genKillContainers.ContainsKey(curBlock))
    {
        Out[curBlock] = new HashSet<TacNode>(genKillContainers[curBlock].GetFirstSet()
            .Union(In[curBlock]
                .Except(genKillContainers[curBlock].GetSecondSet())));
    }
    else
    {
        Out[curBlock] = new HashSet<TacNode>(In[curBlock]);
    }
}
```

## �����
������ ��� ������ ��������� ����� � �����������.

## �����
��������� �����, ��������� ����, �� ������ ��������� ��������� In-Out ��� ���� ������� ������. 
