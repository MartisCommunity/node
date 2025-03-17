using Martiscoin.AsyncWork;
using Martiscoin.Builder.Feature;
using Martiscoin.Builder;
using Martiscoin.Connection;
using Martiscoin.P2P.Protocol.Payloads;
using Martiscoin.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Martiscoin.Configuration.Logging;
using Microsoft.Extensions.DependencyInjection;
using Martiscoin.P2P.Peer;

namespace Martiscoin.Features.BlockStore
{
    public class NodeSyncFeature : FullNodeFeature
    {
        /// <summary>The async loop we need to wait upon before we can shut down this manager.</summary>
        private IAsyncLoop asyncLoop;

        /// <summary>Factory for creating background async loop tasks.</summary>
        private readonly IAsyncProvider asyncProvider;

        /// <summary>
        /// Connection manager injected dependency.
        /// </summary>
        private readonly IConnectionManager connection;

        /// <summary>Global application life cycle control - triggers when application shuts down.</summary>
        private readonly INodeLifetime nodeLifetime;

        private readonly PayloadProvider payloadProvider;

        private readonly NodeSyncBehavior nodeSyncBehavior;

        public NodeSyncFeature(
            IConnectionManager connection,
            INodeLifetime nodeLifetime,
            IAsyncProvider asyncProvider,
            PayloadProvider payloadProvider,
            NodeSyncBehavior nodeSyncBehavior)
        {
            this.connection = connection;
            this.nodeLifetime = nodeLifetime;
            this.payloadProvider = payloadProvider;
            this.asyncProvider = asyncProvider;
            this.nodeSyncBehavior = nodeSyncBehavior;
        }

        public override Task InitializeAsync()
        {
            // Register the behavior.
            this.connection.Parameters.TemplateBehaviors.Add(this.nodeSyncBehavior);

            //// Register the payload types.
            //this.payloadProvider.AddPayload(typeof(StoragePayload));

            // Make a worker that will filter connected knows that has announced our custom payload behavior.
            this.asyncLoop = this.asyncProvider.CreateAndRunAsyncLoop("NodeSync.SyncWorker", async token =>
            {
                IReadOnlyNetworkPeerCollection peers = this.connection.ConnectedPeers;

                if (!peers.Any())
                {
                    return;
                }

                // Announce the blocks on each nodes behavior which supports relaying.
                //IEnumerable<StorageBehavior> behaviors = peers.Where(x => x.PeerVersion?.Relay ?? false)
                //                                              .Select(x => x.Behavior<StorageBehavior>())
                //                                              .Where(x => x != null)
                //                                              .ToList();

                //foreach (StorageBehavior behavior in behaviors)
                //{
                //    await behavior.SendTrickleAsync().ConfigureAwait(false);
                //}
            },
                this.nodeLifetime.ApplicationStopping,
                repeatEvery: TimeSpans.FiveSeconds,
                startAfter: TimeSpans.TenSeconds);

            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// A class providing extension methods for <see cref="IFullNodeBuilder"/>.
    /// </summary>
    public static class FullNodeBuilderNodeSyncExtension
    {
        public static IFullNodeBuilder UseNodeSync(this IFullNodeBuilder fullNodeBuilder)
        {
            LoggingConfiguration.RegisterFeatureNamespace<NodeSyncFeature>("nodesync");

            //fullNodeBuilder.ConfigureFeature(features =>
            //{
            //    features
            //       .AddFeature<NodeSyncFeature>()
            //       .FeatureServices(services =>
            //       {
            //           services.AddSingleton<NodeSyncBehavior>();
            //       });
            //});

            return fullNodeBuilder;
        }
    }
}
