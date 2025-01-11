using Martiscoin.AsyncWork;
using Martiscoin.Base;
using Martiscoin.Base.Deployments;
using Martiscoin.Configuration.Settings;
using Martiscoin.Consensus;
using Martiscoin.Consensus.Chain;
using Martiscoin.Consensus.Checkpoints;
using Martiscoin.Consensus.Rules;
using Martiscoin.Features.Consensus.CoinViews;
using Martiscoin.Features.Consensus.Interfaces;
using Martiscoin.Features.Consensus.ProvenBlockHeaders;
using Martiscoin.Networks;
using Martiscoin.Utilities;
using Microsoft.Extensions.Logging;

namespace Martiscoin.Features.Consensus.Rules
{
    /// <summary>
    /// Extension of consensus rules that provide access to a PoS store.
    /// </summary>
    /// <remarks>
    /// A Proof-Of-Stake blockchain as implemented in this code base represents a hybrid POS/POW consensus model.
    /// </remarks>
    public class PosConsensusRuleEngine : PowConsensusRuleEngine
    {
        /// <summary>Database of stake related data for the current blockchain.</summary>
        public IStakeChain StakeChain { get; }

        /// <summary>Provides functionality for checking validity of PoS blocks.</summary>
        public IStakeValidator StakeValidator { get; }

        public IRewindDataIndexCache RewindDataIndexCache { get; }

        public PosConsensusRuleEngine(Network network, ILoggerFactory loggerFactory, IDateTimeProvider dateTimeProvider, ChainIndexer chainIndexer, NodeDeployments nodeDeployments,
            ConsensusSettings consensusSettings, ICheckpoints checkpoints, ICoinView utxoSet, IStakeChain stakeChain, IStakeValidator stakeValidator, IChainState chainState,
            IInvalidBlockHashStore invalidBlockHashStore, INodeStats nodeStats, IRewindDataIndexCache rewindDataIndexCache, IAsyncProvider asyncProvider, ConsensusRulesContainer consensusRulesContainer)
            : base(network, loggerFactory, dateTimeProvider, chainIndexer, nodeDeployments, consensusSettings, checkpoints, utxoSet, chainState, invalidBlockHashStore, nodeStats, asyncProvider, consensusRulesContainer)
        {
            this.StakeChain = stakeChain;
            this.StakeValidator = stakeValidator;
            this.RewindDataIndexCache = rewindDataIndexCache;
        }

        /// <inheritdoc />
        public override RuleContext CreateRuleContext(ValidationContext validationContext)
        {
            return new PosRuleContext(validationContext, this.DateTimeProvider.GetTimeOffset());
        }

        /// <inheritdoc />
        public override void Initialize(ChainedHeader chainTip)
        {
            base.Initialize(chainTip);

            this.StakeChain.Load();

            // A temporary hack until tip manage will be introduced.
            var coindb = ((CachedCoinView)this.UtxoSet).ICoindb;
            HashHeightPair hash = coindb.GetTipHash();
            ChainedHeader tip = chainTip.FindAncestorOrSelf(hash.Hash);

            this.RewindDataIndexCache.Initialize(tip.Height, this.UtxoSet);
        }
    }
}