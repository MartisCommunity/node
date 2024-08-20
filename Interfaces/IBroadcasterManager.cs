using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XOuranos.Connection.Broadcasting;
using XOuranos.Consensus.TransactionInfo;
using XOuranos.NBitcoin;

namespace XOuranos.Interfaces
{
    /// <summary>
    /// Allow to check a transaction is valid before broadcasting it.
    /// </summary>
    public interface IBroadcastCheck
    {
        Task<string> CheckTransaction(Transaction transaction);
    }

    public interface IBroadcasterManager
    {
        bool CanRespondToTrxGetData { get; set; }

        Task BroadcastTransactionAsync(Transaction transaction);

        BroadcastTransactionStateChanedEntry GetTransaction(uint256 transactionHash);

        void AddOrUpdate(Transaction transaction, TransactionBroadcastState transactionBroadcastState, string errorMessage = null);

        Task<bool> BroadcastTransactionAsync(uint256 trxHash);
    }
}