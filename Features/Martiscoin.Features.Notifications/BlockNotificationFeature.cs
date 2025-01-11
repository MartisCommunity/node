using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Martiscoin.Base;
using Martiscoin.Builder;
using Martiscoin.Builder.Feature;
using Martiscoin.Connection;
using Martiscoin.Consensus;
using Martiscoin.Consensus.Chain;
using Martiscoin.Features.Notifications.Controllers;
using Martiscoin.Features.Notifications.Interfaces;
using Martiscoin.P2P.Peer;

[assembly: InternalsVisibleTo("Martiscoin.Features.Notifications.Tests")]

namespace Martiscoin.Features.Notifications
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
