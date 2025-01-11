using Martiscoin.AsyncWork;
using Martiscoin.Base;
using Martiscoin.Base.Deployments;
using Martiscoin.Configuration.Settings;
using Martiscoin.Consensus;
using Martiscoin.Consensus.Chain;
using Martiscoin.Consensus.Checkpoints;
using Martiscoin.Consensus.Rules;
using Martiscoin.Features.Consensus.CoinViews;
using Martiscoin.Features.Consensus.Rules;
using Martiscoin.Features.PoA.Voting;
using Martiscoin.Networks;
using Martiscoin.Utilities;
using Microsoft.Extensions.Logging;

namespace Martiscoin.Features.PoA
{
    /// <inheritdoc />
    public class PoAConsensusRuleEngine : PowConsensusRuleEngine
    {
        public ISlotsManager SlotsManager { get; private set; }

        public PoABlockHeaderValidator PoaHeaderValidator { get; private set; }

        public VotingManager VotingManager { get; private set; }

        public IFederationManager FederationManager { get; private set; }

        public PoAConsensusRuleEngine(Network network, ILoggerFactory loggerFactory, IDateTimeProvider dateTimeProvider, ChainIndexer chainIndexer,
            NodeDeployments nodeDeployments, ConsensusSettings consensusSettings, ICheckpoints checkpoints, ICoinView utxoSet, IChainState chainState,
            IInvalidBlockHashStore invalidBlockHashStore, INodeStats nodeStats, ISlotsManager slotsManager, PoABlockHeaderValidator poaHeaderValidator,
            VotingManager votingManager, IFederationManager federationManager, IAsyncProvider asyncProvider, ConsensusRulesContainer consensusRulesContainer)
            : base(network, loggerFactory, dateTimeProvider, chainIndexer, nodeDeployments, consensusSettings, checkpoints, utxoSet, chainState, invalidBlockHashStore, nodeStats, asyncProvider, consensusRulesContainer)
        {
            this.SlotsManager = slotsManager;
            this.PoaHeaderValidator = poaHeaderValidator;
            this.VotingManager = votingManager;
            this.FederationManager = federationManager;
        }
    }
}
