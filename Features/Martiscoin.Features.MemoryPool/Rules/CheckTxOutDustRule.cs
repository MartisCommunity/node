﻿using Martiscoin.Consensus.Chain;
using Martiscoin.Features.MemoryPool.Interfaces;
using Martiscoin.NBitcoin.Policy;
using Martiscoin.Networks;
using Microsoft.Extensions.Logging;

namespace Martiscoin.Features.MemoryPool.Rules
{
    /// <summary>
    /// This rule ensures that the transaction does not contain any outputs which are considered dust.
    /// </summary>
    public sealed class CheckTxOutDustRule : MempoolRule
    {
        public CheckTxOutDustRule(
            Network network,
            ITxMempool mempool,
            MempoolSettings mempoolSettings,
            ChainIndexer chainIndexer,
            ILoggerFactory loggerFactory) : base(network, mempool, mempoolSettings, chainIndexer, loggerFactory)
        {
        }

        /// <inheritdoc />
        public override void CheckTransaction(MempoolValidationContext context)
        {
            foreach (var txOut in context.Transaction.Outputs)
            {
                if (StandardTransactionPolicy.IsOpReturn(txOut.ScriptPubKey.ToBytes()))
                    continue;

                if (txOut.IsDust(context.MinRelayTxFee))
                {
                    this.logger.LogTrace("(-)[TX_CONTAINS_DUST_TXOUTS]");
                    context.State.Fail(MempoolErrors.TransactionContainsDustTxOuts, $"{context.Transaction.GetHash()} contains a dust TxOut {txOut.ScriptPubKey.ToString()}.").Throw();
                }
            }
        }
    }
}