using Martiscoin.Base.Deployments;
using Martiscoin.Consensus;
using Martiscoin.Consensus.Chain;
using Martiscoin.Consensus.ScriptInfo;
using Martiscoin.Features.MemoryPool;
using Martiscoin.Features.MemoryPool.Interfaces;
using Martiscoin.Mining;
using Martiscoin.Networks;
using Martiscoin.Utilities;
using Microsoft.Extensions.Logging;

namespace Martiscoin.Features.Miner
{
    public class PowBlockDefinition : BlockDefinition
    {
        private readonly IConsensusRuleEngine consensusRules;
        private readonly ILogger logger;

        public PowBlockDefinition(
            IConsensusManager consensusManager,
            IDateTimeProvider dateTimeProvider,
            ILoggerFactory loggerFactory,
            ITxMempool mempool,
            MempoolSchedulerLock mempoolLock,
            MinerSettings minerSettings,
            Network network,
            IConsensusRuleEngine consensusRules,
            NodeDeployments nodeDeployments,
            BlockDefinitionOptions options = null)
            : base(consensusManager, dateTimeProvider, loggerFactory, mempool, mempoolLock, minerSettings, network, nodeDeployments)
        {
            this.consensusRules = consensusRules;
            this.logger = loggerFactory.CreateLogger(this.GetType().FullName);
        }

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

            this.block.Header.Bits = this.block.Header.GetWorkRequired(this.Network, this.ChainTip);
        }
    }
}