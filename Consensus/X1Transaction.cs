using XOuranos.Consensus.TransactionInfo;

namespace XOuranos.X1.Consensus
{
    public class X1Transaction : Transaction
    {
        public override bool IsProtocolTransaction()
        {
            return this.IsCoinBase || this.IsCoinStake;
        }
    }
}