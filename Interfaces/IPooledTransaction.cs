using System.Threading.Tasks;
using XOuranos.Consensus.TransactionInfo;
using XOuranos.NBitcoin;

namespace XOuranos.Interfaces
{
    public interface IPooledTransaction
    {
        Task<Transaction> GetTransaction(uint256 trxid);
    }
}
