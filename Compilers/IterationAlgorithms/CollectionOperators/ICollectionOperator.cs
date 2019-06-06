﻿using System;
using System.Collections.Generic;

using SimpleLang.TACode.TacNodes;

namespace SimpleLang.IterationAlgorithms.Interfaces
{
    public interface ICollectionOperator
    {
        HashSet<TacNode> Collect(HashSet<TacNode> firstSet, HashSet<TacNode> secondSet);
    }
}