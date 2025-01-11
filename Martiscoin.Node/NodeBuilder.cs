using Martiscoin.Builder;
using Martiscoin.Configuration;
using Martiscoin.Features.BlockStore;
using Martiscoin.Features.ColdStaking;
using Martiscoin.Features.Consensus;
using Martiscoin.Features.Consensus.Interfaces;
using Martiscoin.Features.Diagnostic;
using Martiscoin.Features.MemoryPool;
using Martiscoin.Features.Miner;
using Martiscoin.Features.RPC;
using Martiscoin.Features.Wallet;
using Martiscoin.Features.NodeHost;
using Martiscoin.Features.Dns;
using Martiscoin.Features.Miner.Interfaces;
using Martiscoin.Persistence;
using Martiscoin.Features.Notifications;
using Martiscoin.Features.WalletWatchOnly;
using Martiscoin.Networks.X1.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Martiscoin.Node
{
    public static class NodeBuilder
    {
        public static PersistenceProviderManager persistenceProviderManager;

        public static IFullNodeBuilder Create(string chain, NodeSettings settings)
        {
            chain = chain.ToUpperInvariant();

            IFullNodeBuilder nodeBuilder = CreateBaseBuilder(chain, settings);

            nodeBuilder.UseX1Consensus().UseColdStakingWallet();

            return nodeBuilder;
        }

        private static IFullNodeBuilder CreateBaseBuilder(string chain, NodeSettings settings)
        {
            IFullNodeBuilder nodeBuilder = new FullNodeBuilder()
            .UseNodeSettings(settings)
            .UseBlockStore()
            .UseStorage()
            .UseMempool()
            .UseBlockNotification()
            .UseTransactionNotification()
            .UseNodeHost()
            .AddRPC()
            .UseDiagnosticFeature();

            UseDnsFullNode(nodeBuilder, settings);

            return nodeBuilder;
        }
        static void UseDnsFullNode(IFullNodeBuilder nodeBuilder, NodeSettings nodeSettings)
        {
            if (nodeSettings.ConfigReader.GetOrDefault("dnsfullnode", false, nodeSettings.Logger))
            {
                var dnsSettings = new DnsSettings(nodeSettings);

                if (string.IsNullOrWhiteSpace(dnsSettings.DnsHostName) || string.IsNullOrWhiteSpace(dnsSettings.DnsNameServer) || string.IsNullOrWhiteSpace(dnsSettings.DnsMailBox))
                    throw new ConfigurationException("When running as a DNS Seed service, the -dnshostname, -dnsnameserver and -dnsmailbox arguments must be specified on the command line.");

                nodeBuilder.UseDns();
            }
        }
    }
}
