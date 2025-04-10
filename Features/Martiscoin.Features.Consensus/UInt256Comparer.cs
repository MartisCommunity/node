﻿using System.Collections.Generic;
using Martiscoin.Consensus.TransactionInfo;
using Martiscoin.NBitcoin;

namespace Martiscoin.Features.Consensus
{
    public class UInt256Comparer : IComparer<uint256>
    {
        public int Compare(uint256 x, uint256 y)
        {
            if (x < y) return -1;
            if (x > y) return 1;
            return 0;
        }
    }

    public class OutPointComparer : IComparer<OutPoint>
    {
        public int Compare(OutPoint x, OutPoint y)
        {
            if (x < y) return -1;
            if (x > y) return 1;
            return 0;
        }
    }

}
