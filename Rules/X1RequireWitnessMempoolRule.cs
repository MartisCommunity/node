using XOuranos.Consensus;
using XOuranos.Consensus.Chain;
using XOuranos.Features.MemoryPool;
using XOuranos.Features.MemoryPool.Interfaces;
using Microsoft.Extensions.Logging;
using XOuranos.Networks;

namespace XOuranos.X1.Rules
{
    /// <summary>
    /// Checks weather the transaction has witness.
    /// </summary>
    public class X1RequireWitnessMempoolRule : MempoolRule
    {
        public X1RequireWitnessMempoolRule(Network network,
            ITxMempool mempool,
            MempoolSettings mempoolSettings,
            ChainIndexer chainIndexer,
            IConsensusRuleEngine consensusRules,
            ILoggerFactory loggerFactory) : base(network, mempool, mempoolSettings, chainIndexer, loggerFactory)
        {
        }

        public override void CheckTransaction(MempoolValidationContext context)
        {
            if (!context.Transaction.HasWitness)
            {
                this.logger.LogTrace($"(-)[FAIL_{nameof(X1RequireWitnessMempoolRule)}]".ToUpperInvariant());
                X1ConsensusErrors.MissingWitness.Throw();
            }
        }
    }
}