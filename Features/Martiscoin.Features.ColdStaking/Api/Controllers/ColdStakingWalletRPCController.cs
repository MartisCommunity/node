using Asp.Versioning;
using Martiscoin.Connection;
using Martiscoin.Consensus;
using Martiscoin.Consensus.Chain;
using Martiscoin.Features.BlockStore;
using Martiscoin.Features.Wallet;
using Martiscoin.Features.Wallet.Api.Controllers;
using Martiscoin.Features.Wallet.Interfaces;
using Martiscoin.Features.Wallet.Types;
using Martiscoin.Interfaces;
using Martiscoin.Networks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Martiscoin.Features.ColdStaking.Api.Controllers
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