using System.Collections.Generic;
using XOuranos.Utilities;

namespace XOuranos.Features.Consensus
{
    public class UnspentOutputsComparer : IComparer<UnspentOutput>
    {
        public static UnspentOutputsComparer Instance { get; } = new UnspentOutputsComparer();

        private readonly OutPointComparer Comparer = new OutPointComparer();

        public int Compare(UnspentOutput x, UnspentOutput y)
        {
            return this.Comparer.Compare(x.OutPoint, y.OutPoint);
        }
    }
}
