using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Martiscoin.Broadcasters;
using Martiscoin.Builder;
using Martiscoin.Builder.Feature;
using Martiscoin.Configuration.Logging;
using Martiscoin.Connection;
using Martiscoin.Connection.Broadcasting;
using Martiscoin.Consensus;
using Martiscoin.Features.BlockStore;
using Martiscoin.Features.MemoryPool;
using Martiscoin.Features.MemoryPool.Broadcasting;
using Martiscoin.Features.RPC;
using Martiscoin.Features.Wallet.AddressBook;
using Martiscoin.Features.Wallet.Broadcasters;
using Martiscoin.Features.Wallet.Interfaces;
using Martiscoin.Features.Wallet.Types;
using Martiscoin.Features.Wallet.UI;
using Martiscoin.Interfaces;
using Martiscoin.Interfaces.UI;
using Martiscoin.NBitcoin.Policy;
using Martiscoin.Networks;
using Martiscoin.Signals;
using Martiscoin.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace Martiscoin.Features.Wallet
{
    /// <summary>
    /// Common base class for any feature replacing the <see cref="WalletFeature" />.
    /// </summary>
    public abstract class BaseWalletFeature : FullNodeFeature
    {
    }

    /// <summary>
    /// Wallet feature for the full node.
    /// </summary>
    /// <seealso cref="FullNodeFeature" />
    public class WalletFeature : BaseWalletFeature
    {
        private readonly IWalletSyncManager walletSyncManager;

        private readonly IWalletManager walletManager;

        private readonly IAddressBookManager addressBookManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="WalletFeature"/> class.
        /// </summary>
        /// <param name="walletSyncManager">The synchronization manager for the wallet, tasked with keeping the wallet synced with the network.</param>
        /// <param name="walletManager">The wallet manager.</param>
        /// <param name="addressBookManager">The address book manager.</param>
        /// <param name="signals">The signals responsible for receiving blocks and transactions from the network.</param>
        /// <param name="connectionManager">The connection manager.</param>
        /// <param name="broadcasterBehavior">The broadcaster behavior.</param>
        public WalletFeature(
            IWalletSyncManager walletSyncManager,
            IWalletManager walletManager,
            IAddressBookManager addressBookManager,
            INodeStats nodeStats)
        {
            this.walletSyncManager = walletSyncManager;
            this.walletManager = walletManager;
            this.addressBookManager = addressBookManager;

            nodeStats.RegisterStats(this.AddComponentStats, StatsType.Component, this.GetType().Name);
            nodeStats.RegisterStats(this.AddInlineStats, StatsType.Inline, this.GetType().Name, 800);
        }

        /// <summary>
        /// Prints command-line help.
        /// </summary>
        /// <param name="network">The network to extract values from.</param>
        public static void PrintHelp(Network network)
        {
            WalletSettings.PrintHelp(network);
        }

        /// <summary>
        /// Get the default configuration.
        /// </summary>
        /// <param name="builder">The string builder to add the settings to.</param>
        /// <param name="network">The network to base the defaults off.</param>
        public static void BuildDefaultConfigurationFile(StringBuilder builder, Network network)
        {
            WalletSettings.BuildDefaultConfigurationFile(builder, network);
        }

        private void AddInlineStats(StringBuilder log)
        {
            var walletManager = this.walletManager as WalletManager;

            if (walletManager != null)
            {
                HashHeightPair hashHeightPair = walletManager.LastReceivedBlockInfo();

                log.AppendLine("Wallet.Height: ".PadRight(LoggingConfiguration.ColumnLength + 1) +
                                        (walletManager.ContainsWallets ? hashHeightPair.Height.ToString().PadRight(8) : "No Wallet".PadRight(8)) +
                                        (walletManager.ContainsWallets ? (" Wallet.Hash: ".PadRight(LoggingConfiguration.ColumnLength - 1) + hashHeightPair.Hash) : string.Empty));
            }
        }

        private void AddComponentStats(StringBuilder log)
        {
            IEnumerable<string> walletNames = this.walletManager.GetWalletsNames();

            if (walletNames.Any())
            {
                log.AppendLine();
                log.AppendLine("======Wallets======");

                foreach (string walletName in walletNames)
                {
                    foreach (HdAccount account in this.walletManager.GetAccounts(walletName))
                    {
                        AccountBalance accountBalance = this.walletManager.GetBalances(walletName, account.Name).Single();
                        log.AppendLine(($"{walletName}/{account.Name}" + ",").PadRight(LoggingConfiguration.ColumnLength + 10)
                                                  + (" Confirmed balance: " + accountBalance.AmountConfirmed.ToString()).PadRight(LoggingConfiguration.ColumnLength + 20)
                                                  + " Unconfirmed balance: " + accountBalance.AmountUnconfirmed.ToString());
                    }
                }
            }
        }

        /// <inheritdoc />
        public override Task InitializeAsync()
        {
            this.walletManager.Start();
            this.walletSyncManager.Start();
            this.addressBookManager.Initialize();

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public override void Dispose()
        {
            this.walletManager.Stop();
            this.walletSyncManager.Stop();
        }
    }

    /// <summary>
    /// A class providing extension methods for <see cref="IFullNodeBuilder"/>.
    /// </summary>
    public static class FullNodeBuilderWalletExtension
    {
        public static IFullNodeBuilder UseWallet(this IFullNodeBuilder fullNodeBuilder)
        {
            LoggingConfiguration.RegisterFeatureNamespace<WalletFeature>("wallet");

            fullNodeBuilder.ConfigureFeature(features =>
            {
                features
                .AddFeature<WalletFeature>()
                .DependOn<MempoolFeature>()
                .DependOn<BlockStoreFeature>()
                .DependOn<RPCFeature>()
                .FeatureServices(services =>
                    {
                        services.AddSingleton<IWalletSyncManager, WalletSyncManager>();
                        services.AddSingleton<IWalletTransactionHandler, WalletTransactionHandler>();
                        services.AddSingleton<IWalletManager, WalletManager>();
                        services.AddSingleton<IWalletFeePolicy, WalletFeePolicy>();
                        services.AddSingleton<WalletSettings>();
                        services.AddSingleton<IScriptAddressReader>(new ScriptAddressReader());
                        services.AddSingleton<StandardTransactionPolicy>();
                        services.AddSingleton<IAddressBookManager, AddressBookManager>();
                        services.AddSingleton<INavigationItem, WalletNavigationItem>();
                        services.AddSingleton<IClientEventBroadcaster, WalletInfoBroadcaster>();
                    });
            });

            return fullNodeBuilder;
        }
    }
}