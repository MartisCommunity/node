using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Martiscoin.Base;
using Martiscoin.Base.Deployments;
using Martiscoin.Builder.Feature;
using Martiscoin.Configuration.Settings;
using Martiscoin.Connection;
using Martiscoin.Consensus;
using Martiscoin.Consensus.ScriptInfo;
using Martiscoin.Networks;
using Martiscoin.P2P.Protocol.Payloads;
using Martiscoin.Signals;
using Martiscoin.Utilities.Store;

[assembly: InternalsVisibleTo("Martiscoin.Features.Miner.Tests")]
[assembly: InternalsVisibleTo("Martiscoin.Features.Consensus.Tests")]

namespace Martiscoin.Features.Consensus
{
    public class ConsensusFeature : FullNodeFeature
    {
        private readonly IChainState chainState;

        private readonly IConnectionManager connectionManager;

        private readonly ISignals signals;

        private readonly IConsensusManager consensusManager;

        private readonly NodeDeployments nodeDeployments;

        private readonly IKeyValueRepository keyValueRepository;

        public ConsensusFeature(
            Network network,
            IChainState chainState,
            IConnectionManager connectionManager,
            ISignals signals,
            IConsensusManager consensusManager,
            NodeDeployments nodeDeployments,
            IKeyValueRepository keyValueRepository)
        {
            this.chainState = chainState;
            this.connectionManager = connectionManager;
            this.signals = signals;
            this.consensusManager = consensusManager;
            this.nodeDeployments = nodeDeployments;
            this.keyValueRepository = keyValueRepository;

            this.chainState.MaxReorgLength = network.Consensus.MaxReorgLength;
        }

        /// <inheritdoc />
        public override Task InitializeAsync()
        {
            DeploymentFlags flags = this.keyValueRepository.LoadValueJson<DeploymentFlags>("deploymentflags");

            if (flags == null)
            {
                flags = this.nodeDeployments.GetFlags(this.consensusManager.Tip);

                // Update the cache of Flags when we retrieve it.
                this.keyValueRepository.SaveValueJson("deploymentflags", flags);
            }

            if (flags.ScriptFlags.HasFlag(ScriptVerify.Witness))
            {
                // Add witness discovery as a requirement if witness is activated.
                this.connectionManager.AddDiscoveredNodesRequirement(NetworkPeerServices.NODE_WITNESS);
            }

            // Always announce that the node supports WITNESS even if witness is not activated yet.
            this.connectionManager.Parameters.Services |= NetworkPeerServices.NODE_WITNESS;

            return Task.CompletedTask;
        }

        /// <summary>
        /// Prints command-line help.
        /// </summary>
        /// <param name="network">The network to extract values from.</param>
        public static void PrintHelp(Network network)
        {
            ConsensusSettings.PrintHelp(network);
        }

        /// <summary>
        /// Get the default configuration.
        /// </summary>
        /// <param name="builder">The string builder to add the settings to.</param>
        /// <param name="network">The network to base the defaults off.</param>
        public static void BuildDefaultConfigurationFile(StringBuilder builder, Network network)
        {
            ConsensusSettings.BuildDefaultConfigurationFile(builder, network);
        }

        /// <inheritdoc />
        public override void Dispose()
        {
        }
    }
}