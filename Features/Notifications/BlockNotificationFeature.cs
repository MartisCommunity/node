using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using XOuranos.Base;
using XOuranos.Builder;
using XOuranos.Builder.Feature;
using XOuranos.Connection;
using XOuranos.Consensus;
using XOuranos.Consensus.Chain;
using XOuranos.Features.Notifications.Controllers;
using XOuranos.Features.Notifications.Interfaces;
using XOuranos.P2P.Peer;

[assembly: InternalsVisibleTo("XOuranos.Features.Notifications.Tests")]

namespace XOuranos.Features.Notifications
{
    /// =================================================================
    /// TODO: This class is broken and the logic needs to be redesigned, this effects light wallet.
    /// =================================================================
    /// <summary>
    /// Feature enabling the broadcasting of blocks.
    /// </summary>
    public class BlockNotificationFeature : FullNodeFeature
    {
        private readonly IBlockNotification blockNotification;

        private readonly IConnectionManager connectionManager;

        private readonly IConsensusManager consensusManager;

        private readonly IChainState chainState;

        private readonly ChainIndexer chainIndexer;

        private readonly ILoggerFactory loggerFactory;

        public BlockNotificationFeature(
            IBlockNotification blockNotification,
            IConnectionManager connectionManager,
            IConsensusManager consensusManager,
            IChainState chainState,
            ChainIndexer chainIndexer,
            ILoggerFactory loggerFactory)
        {
            this.blockNotification = blockNotification;
            this.connectionManager = connectionManager;
            this.consensusManager = consensusManager;
            this.chainState = chainState;
            this.chainIndexer = chainIndexer;
            this.loggerFactory = loggerFactory;
        }

        public override Task InitializeAsync()
        {
            NetworkPeerConnectionParameters connectionParameters = this.connectionManager.Parameters;

            this.blockNotification.Start();
            this.chainState.ConsensusTip = this.chainIndexer.Tip;

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            this.blockNotification.Stop();
        }
    }

    /// <summary>
    /// A class providing extension methods for <see cref="IFullNodeBuilder"/>.
    /// </summary>
    public static class FullNodeBuilderBlockNotificationExtension
    {
        public static IFullNodeBuilder UseBlockNotification(this IFullNodeBuilder fullNodeBuilder)
        {
            fullNodeBuilder.ConfigureFeature(features =>
            {
                features
                .AddFeature<BlockNotificationFeature>()
                .FeatureServices(services =>
                {
                    services.AddSingleton<IBlockNotification, BlockNotification>();
                });
            });

            return fullNodeBuilder;
        }
    }
}
