using Martiscoin.Connection;
using Martiscoin.Interfaces;
using Martiscoin.NBitcoin;
using Martiscoin.Networks;
using Martiscoin.P2P.Peer;
using Martiscoin.P2P.Protocol.Behaviors;
using Martiscoin.P2P.Protocol.Payloads;
using Martiscoin.P2P.Protocol;
using Martiscoin.Utilities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Martiscoin.Features.BlockStore
{
    /// <summary>
    /// Peer behavior for memory pool.
    /// Provides message handling of notifications from attached peer.
    /// </summary>
    public class StorageBehavior : NetworkPeerBehavior
    {
        /// <summary>Connection manager for managing peer connections.</summary>
        private readonly IConnectionManager connectionManager;

        /// <summary>Provider of IBD state.</summary>
        private readonly IInitialBlockDownloadState initialBlockDownloadState;

        /// <summary>Instance logger for the memory pool behavior component.</summary>
        private readonly ILogger logger;

        /// <summary>Factory used to create the logger for this component.</summary>
        private readonly ILoggerFactory loggerFactory;

        /// <summary>The network that this component is running on.</summary>
        private readonly Network network;

        /// <summary>
        /// Locking object for memory pool behaviour.
        /// </summary>
        private readonly object lockObject;

        /// <summary>
        /// The min fee the peer asks to relay transactions.
        /// </summary>
        public Money MinFeeFilter { get; set; }

        /// <summary>
        /// lot miner tx
        /// </summary>
        List<LotTransaction> lotTransactions = new List<LotTransaction>();

        /// <summary>
        /// Get all lot miner in current block hegiht
        /// </summary>
        /// <param name="height"></param>
        /// <returns></returns>
        public List<LotTransaction> GetMinerTx(ulong height)
        {
            lock (lockObject)
            {
                List<LotTransaction> finds = new List<LotTransaction>();
                if (lotTransactions != null)
                {
                    lotTransactions.RemoveAll(tx => tx.LotFoundHeight < height);
                    foreach (var tx in lotTransactions)
                    {
                        if (finds.FindAll(a => a.LotNonce == tx.LotNonce).Count > 0) continue;
                        finds.Add(new LotTransaction()
                        {
                            LotFoundHeight= tx.LotFoundHeight,
                            LotNonce = tx.LotNonce,
                            Address = tx.Address
                        });
                    }
                }
                return finds;
            }
        }

        public StorageBehavior(
            IConnectionManager connectionManager,
            IInitialBlockDownloadState initialBlockDownloadState,
            ILoggerFactory loggerFactory,
            Network network)
        {
            this.connectionManager = connectionManager;
            this.initialBlockDownloadState = initialBlockDownloadState;
            this.logger = loggerFactory.CreateLogger(this.GetType().FullName);
            this.loggerFactory = loggerFactory;
            this.network = network;
            this.lockObject = new object();
        }

        /// <inheritdoc />
        protected override void AttachCore()
        {
            this.AttachedPeer.MessageReceived.Register(this.OnMessageReceivedAsync);
        }

        /// <inheritdoc />
        protected override void DetachCore()
        {
            this.AttachedPeer.MessageReceived.Unregister(this.OnMessageReceivedAsync);
        }

        /// <inheritdoc />
        public override object Clone()
        {
            return new StorageBehavior(this.connectionManager, this.initialBlockDownloadState, this.loggerFactory, this.network);
        }

        private async Task OnMessageReceivedAsync(INetworkPeer peer, IncomingMessage message)
        {
            try
            {
                await this.ProcessMessageAsync(peer, message).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                this.logger.LogTrace("(-)[CANCELED_EXCEPTION]");
                return;
            }
            catch (Exception ex)
            {
                this.logger.LogError("Exception occurred: {0}", ex.ToString());
                throw;
            }
        }

        private async Task ProcessMessageAsync(INetworkPeer peer, IncomingMessage message)
        {
            try
            {
                switch (message.Message.Payload)
                {
                    case StoragePayload storagePayload:
                        lock (lockObject)
                        {
                            if (lotTransactions.FindAll(tx => tx.LotNonce == storagePayload.LotNonce &&
                            (tx.LotFoundHeight == storagePayload.LotFoundHeight)).Count() <= 0)
                            {
                                lotTransactions.Add(new LotTransaction()
                                {
                                    Address = storagePayload.Address,
                                    Header = storagePayload.Header,
                                    LotFoundHeight = storagePayload.LotFoundHeight,
                                    LotNonce = storagePayload.LotNonce,
                                });
                            }
                        }
                        await this.ProcessStorageInPayloadAsync(peer, storagePayload).ConfigureAwait(false);
                        break;
                }
            }
            catch (OperationCanceledException)
            {
                this.logger.LogTrace("(-)[CANCELED_EXCEPTION]");
                return;
            }
        }

        private async Task ProcessStorageInPayloadAsync(INetworkPeer peer, StoragePayload message)
        {
            Guard.NotNull(peer, nameof(peer));

            if (peer != this.AttachedPeer)
            {
                this.logger.LogDebug("Attached peer '{0}' does not match the originating peer '{1}'.", this.AttachedPeer?.RemoteSocketEndpoint, peer.RemoteSocketEndpoint);
                this.logger.LogTrace("(-)[PEER_MISMATCH]");
                return;
            }

            await this.SendStorageAsync(peer, message);
        }

        private async Task SendStorageAsync(INetworkPeer peer, StoragePayload message)
        {
            if (peer.IsConnected)
            {
                this.logger.LogDebug("Sending items to peer '{0}'.", peer.RemoteSocketEndpoint);
                await peer.SendMessageAsync(message).ConfigureAwait(false);
            }
        }

        public async Task SendTrickleAsync()
        {
            INetworkPeer peer = this.AttachedPeer;
            if (peer == null)
            {
                this.logger.LogTrace("(-)[NO_PEER]");
                return;
            }

            this.logger.LogDebug("Sending storage inventory to peer '{0}'.", peer.RemoteSocketEndpoint);
            try
            {
                // Sample data to send.
                await this.SendStorageAsync(peer, new StoragePayload(0, 0, "", null)).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                this.logger.LogTrace("(-)[CANCELED_EXCEPTION]");
                return;
            }
        }
    }
}
