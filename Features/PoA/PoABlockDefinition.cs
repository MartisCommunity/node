using XOuranos.Base.Deployments;
using XOuranos.Consensus;
using XOuranos.Consensus.Chain;
using XOuranos.Consensus.ScriptInfo;
using XOuranos.Features.MemoryPool;
using XOuranos.Features.MemoryPool.Interfaces;
using XOuranos.Features.Miner;
using XOuranos.Features.PoA.BasePoAFeatureConsensusRules;
using XOuranos.Mining;
using XOuranos.Networks;
using XOuranos.Utilities;
using Microsoft.Extensions.Logging;

namespace XOuranos.Features.PoA
{
    public class PoABlockDefinition : BlockDefinition
    {
        public PoABlockDefinition(
            IConsensusManager consensusManager,
            IDateTimeProvider dateTimeProvider,
            ILoggerFactory loggerFactory,
            ITxMempool mempool,
            MempoolSchedulerLock mempoolLock,
            Network network,
            MinerSettings minerSettings,
            NodeDeployments nodeDeployments)
            : base(consensusManager, dateTimeProvider, loggerFactory, mempool, mempoolLock, minerSettings, network, nodeDeployments)
        {
        }

        /// <inheritdoc/>
        public override void AddToBlock(TxMempoolEntry mempoolEntry)
        {
            this.AddTransactionToBlock(mempoolEntry.Transaction);
            this.UpdateBlockStatistics(mempoolEntry);
            this.UpdateTotalFees(mempoolEntry.Fee);
        }

        /// <inheritdoc/>
        public override BlockTemplate Build(ChainedHeader chainTip, Script scriptPubKey)
        {
            base.OnBuild(chainTip, scriptPubKey);

            return this.BlockTemplate;
        }

        /// <inheritdoc/>
        public override void UpdateHeaders()
        {
            base.UpdateBaseHeaders();

            this.block.Header.Bits = PoAHeaderDifficultyRule.PoABlockDifficulty;
        }
    }
}
