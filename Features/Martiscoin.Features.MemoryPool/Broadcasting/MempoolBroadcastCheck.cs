﻿using System.Threading.Tasks;
using Martiscoin.Consensus.TransactionInfo;
using Martiscoin.Features.MemoryPool.Interfaces;
using Martiscoin.Interfaces;
using Martiscoin.Utilities;

namespace Martiscoin.Features.MemoryPool.Broadcasting
{
    /// <summary>
    /// Adds the transaction to the mempool and if it failed validation retun the error code.
    /// </summary>
    public class MempoolBroadcastCheck : IBroadcastCheck
    {
        private readonly IMempoolValidator mempoolValidator;

        public MempoolBroadcastCheck(IMempoolValidator mempoolValidator)
        {
            Guard.NotNull(mempoolValidator, nameof(mempoolValidator));

            this.mempoolValidator = mempoolValidator;
        }

        public async Task<string> CheckTransaction(Transaction transaction)
        {
            Guard.NotNull(transaction, nameof(transaction));

            var state = new MempoolValidationState(false);

            if (!await this.mempoolValidator.AcceptToMemoryPool(state, transaction).ConfigureAwait(false))
            {
                return $"{state.Error.ConsensusError?.Message ?? state.Error.Code ?? "Failed"}";
            }

            return string.Empty;
        }
    }
}