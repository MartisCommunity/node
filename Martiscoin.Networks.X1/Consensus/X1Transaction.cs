using Martiscoin.Consensus.TransactionInfo;

namespace Martiscoin.Networks.X1.Consensus
{
    public class X1Transaction : Transaction
    {
        public uint LotNonce { get; set; }
        public ulong LotFoundHeight { get; set; }

        public override bool IsProtocolTransaction()
        {
            return this.IsCoinBase || this.IsCoinStake;
        }
    }
}