﻿using System.Threading.Tasks;
using Martiscoin.Base.Deployments;
using Martiscoin.Consensus;
using Martiscoin.Consensus.BlockInfo;
using Martiscoin.Consensus.Rules;
using Martiscoin.Consensus.TransactionInfo;
using Martiscoin.Features.Consensus.Rules.UtxosetRules;
using Martiscoin.Utilities;
using Microsoft.Extensions.Logging;

namespace Martiscoin.Features.Consensus.Rules.CommonRules
{
    /// <summary>
    /// Prevent duplicate transactions in the coinbase.
    /// </summary>
    /// <remarks>
    /// More info here https://github.com/bitcoin/bips/blob/master/bip-0030.mediawiki
    /// </remarks>
    public class TransactionDuplicationActivationRule : UtxoStoreConsensusRule
    {
        /// <inheritdoc />>
        /// <exception cref="ConsensusErrors.BadTransactionBIP30"> Thrown if BIP30 is not passed.</exception>
        public override Task RunAsync(RuleContext context)
        {
            if (!context.SkipValidation)
            {
                Block block = context.ValidationContext.BlockToValidate;
                DeploymentFlags flags = context.Flags;
                var utxoRuleContext = context as UtxoRuleContext;
                UnspentOutputSet view = utxoRuleContext.UnspentOutputSet;

                if (flags.EnforceBIP30)
                {
                    foreach (Transaction tx in block.Transactions)
                    {
                        foreach(IndexedTxOut indexedTxOut in tx.Outputs.AsIndexedOutputs())
                        {
                            UnspentOutput coins = view.AccessCoins(indexedTxOut.ToOutPoint());
                            if ((coins?.Coins != null) && !coins.Coins.IsPrunable)
                            {
                                this.Logger.LogDebug("Transaction '{0}' already found in store", tx.GetHash());
                                this.Logger.LogTrace("(-)[BAD_TX_BIP_30]");
                                ConsensusErrors.BadTransactionBIP30.Throw();
                            }
}

                    }
                }
            }
            else this.Logger.LogDebug("BIP30 validation skipped for checkpointed block at height {0}.", context.ValidationContext.ChainedHeaderToValidate.Height);

            return Task.CompletedTask;
        }
    }
}