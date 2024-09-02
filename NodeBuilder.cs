using XOuranos.Builder;
using XOuranos.Configuration;
using XOuranos.Features.BlockStore;
using XOuranos.Features.ColdStaking;
using XOuranos.Features.Consensus;
using XOuranos.Features.Consensus.Interfaces;
using XOuranos.Features.Diagnostic;
using XOuranos.Features.MemoryPool;
using XOuranos.Features.Miner;
using XOuranos.Features.RPC;
using XOuranos.Features.Wallet;
using XOuranos.Features.NodeHost;
using XOuranos.Features.Dns;
using XOuranos.Features.Miner.Interfaces;
using XOuranos.Persistence;
using XOuranos.Features.Notifications;
using XOuranos.Features.WalletWatchOnly;
using XOuranos.X1.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace XOuranos
{
    public static class NodeBuilder
    {
        public static PersistenceProviderManager persistenceProviderManager;

        public static IFullNodeBuilder Create(NodeSettings settings)
        {
            IFullNodeBuilder nodeBuilder = CreateBaseBuilder(settings);
            nodeBuilder.UseX1Consensus().UseColdStakingWallet();
            return nodeBuilder;
        }

        private static IFullNodeBuilder CreateBaseBuilder(NodeSettings settings)
        {
            IFullNodeBuilder nodeBuilder = new FullNodeBuilder()
            .UseNodeSettings(settings)
            .UseBlockStore()
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
