using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using XOuranos.Base;
using XOuranos.Base.Deployments;
using XOuranos.Connection;
using XOuranos.Consensus;
using XOuranos.Consensus.Chain;
using XOuranos.Interfaces;
using XOuranos.Networks;
using XOuranos.Signals;
using XOuranos.Utilities.Store;
using Microsoft.Extensions.Logging;

[assembly: InternalsVisibleTo("XOuranos.Features.Miner.Tests")]
[assembly: InternalsVisibleTo("XOuranos.Features.Consensus.Tests")]

namespace XOuranos.Features.Consensus
{
    public class PowConsensusFeature : ConsensusFeature
    {
        private readonly IChainState chainState;
        private readonly IConnectionManager connectionManager;
        private readonly IConsensusManager consensusManager;
        private readonly NodeDeployments nodeDeployments;
        private readonly ChainIndexer chainIndexer;
        private readonly IInitialBlockDownloadState initialBlockDownloadState;
        private readonly IPeerBanning peerBanning;
        private readonly ILoggerFactory loggerFactory;

        public PowConsensusFeature(
            Network network,
            IChainState chainState,
            IConnectionManager connectionManager,
            IConsensusManager consensusManager,
            NodeDeployments nodeDeployments,
            ChainIndexer chainIndexer,
            IInitialBlockDownloadState initialBlockDownloadState,
            IPeerBanning peerBanning,
            ISignals signals,
            ILoggerFactory loggerFactory,
            IKeyValueRepository keyValueRepository) : base(network, chainState, connectionManager, signals, consensusManager, nodeDeployments, keyValueRepository)
        {
            this.chainState = chainState;
            this.connectionManager = connectionManager;
            this.consensusManager = consensusManager;
            this.nodeDeployments = nodeDeployments;
            this.chainIndexer = chainIndexer;
            this.initialBlockDownloadState = initialBlockDownloadState;
            this.peerBanning = peerBanning;
            this.loggerFactory = loggerFactory;

            this.chainState.MaxReorgLength = network.Consensus.MaxReorgLength;
        }

        /// <summary>
        /// Prints command-line help. Invoked via reflection.
        /// </summary>
        /// <param name="network">The network to extract values from.</param>
        public static new void PrintHelp(Network network)
        {
            ConsensusFeature.PrintHelp(network);
        }

        /// <summary>
        /// Get the default configuration. Invoked via reflection.
        /// </summary>
        /// <param name="builder">The string builder to add the settings to.</param>
        /// <param name="network">The network to base the defaults off.</param>
        public static new void BuildDefaultConfigurationFile(StringBuilder builder, Network network)
        {
            ConsensusFeature.BuildDefaultConfigurationFile(builder, network);
        }

        /// <inheritdoc />
        public override Task InitializeAsync()
        {
            base.InitializeAsync();

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public override void Dispose()
        {
        }
    }


}