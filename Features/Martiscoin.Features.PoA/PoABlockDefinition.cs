using Martiscoin.Base.Deployments;
using Martiscoin.Consensus;
using Martiscoin.Consensus.Chain;
using Martiscoin.Consensus.ScriptInfo;
using Martiscoin.Features.MemoryPool;
using Martiscoin.Features.MemoryPool.Interfaces;
using Martiscoin.Features.Miner;
using Martiscoin.Features.PoA.BasePoAFeatureConsensusRules;
using Martiscoin.Mining;
using Martiscoin.Networks;
using Martiscoin.Utilities;
using Microsoft.Extensions.Logging;

namespace Martiscoin.Features.PoA
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
