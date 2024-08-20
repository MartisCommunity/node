using XOuranos.Consensus.TransactionInfo;
using XOuranos.NBitcoin;

namespace XOuranos.Features.RPC
{
    public class FundRawTransactionResponse
    {
        public Transaction Transaction
        {
            get; set;
        }
        public Money Fee
        {
            get; set;
        }
        public int ChangePos
        {
            get; set;
        }
    }
}
