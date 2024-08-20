using System.Collections.Generic;
using XOuranos.Features.MemoryPool;

namespace XOuranos.Features.Miner.Comparers
{
    /// <summary>
    /// This matches the calculation in CompareTxMemPoolEntryByAncestorFee,
    /// except operating on CTxMemPoolModifiedEntry.
    /// </summary>
    public sealed class CompareModifiedEntry : IComparer<TxMemPoolModifiedEntry>
    {
        public int Compare(TxMemPoolModifiedEntry a, TxMemPoolModifiedEntry b)
        {
            return TxMempoolEntry.CompareFees(a, b);
        }
    }
}