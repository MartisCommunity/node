using Asp.Versioning;
using XOuranos.Connection;
using XOuranos.Consensus;
using XOuranos.Consensus.Chain;
using XOuranos.Features.BlockStore;
using XOuranos.Features.Wallet;
using XOuranos.Features.Wallet.Api.Controllers;
using XOuranos.Features.Wallet.Interfaces;
using XOuranos.Features.Wallet.Types;
using XOuranos.Interfaces;
using XOuranos.Networks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace XOuranos.Features.ColdStaking.Api.Controllers
{
    /// <summary> All functionality is in WalletRPCController, just inherit the functionality in this feature.</summary>
    [ApiVersion("1")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ColdStakingWalletRPCController : WalletRPCController
    {
        public ColdStakingWalletRPCController(
            IBlockStore blockStore,
            IBroadcasterManager broadcasterManager,
            ChainIndexer chainIndexer,
            IConsensusManager consensusManager,
            IFullNode fullNode,
            ILoggerFactory loggerFactory,
            Network network,
            IScriptAddressReader scriptAddressReader,
            StoreSettings storeSettings,
            IWalletManager walletManager,
            WalletSettings walletSettings,
            IConnectionManager connectionManager,
            IWalletTransactionHandler walletTransactionHandler) :
            base(blockStore, broadcasterManager, chainIndexer, consensusManager, fullNode, loggerFactory, network, scriptAddressReader, storeSettings, walletManager, walletSettings, connectionManager, walletTransactionHandler)
        {
        }
    }
}