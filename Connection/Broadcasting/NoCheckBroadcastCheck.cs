using System.Linq;
using System.Threading.Tasks;
using XOuranos.Consensus.TransactionInfo;
using XOuranos.Interfaces;
using XOuranos.Utilities;

namespace XOuranos.Connection.Broadcasting
{
    /// <summary>
    /// Broadcast that makes not checks.
    /// </summary>
    public class NoCheckBroadcastCheck : IBroadcastCheck
    {
        public NoCheckBroadcastCheck()
        {
        }

        public Task<string> CheckTransaction(Transaction transaction)
        {
            return Task.FromResult(string.Empty);
        }
    }
}