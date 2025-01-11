using System.Linq;
using System.Threading.Tasks;
using Martiscoin.Base;
using Martiscoin.Consensus.TransactionInfo;
using Martiscoin.Features.Consensus.CoinViews;
using Martiscoin.Interfaces;
using Martiscoin.NBitcoin;
using Martiscoin.Networks;
using Martiscoin.Utilities;
using Microsoft.Extensions.Logging;

namespace Martiscoin.Features.Consensus
{
    /// <summary>
    /// A class that provides the ability to query consensus elements.
    /// </summary>
    public class ConsensusQuery : IGetUnspentTransaction, INetworkDifficulty
    {
        private readonly IChainState chainState;
        private readonly ICoinView coinView;
        private readonly ILogger logger;
        private readonly Network network;

        public ConsensusQuery(
            ICoinView coinView,
            IChainState chainState,
            Network network,
            ILoggerFactory loggerFactory)
        {
            this.coinView = coinView;
            this.chainState = chainState;
            this.network = network;
            this.logger = loggerFactory.CreateLogger(this.GetType().FullName);
        }

        /// <inheritdoc />
        public Task<UnspentOutput> GetUnspentTransactionAsync(OutPoint outPoint)
        {
            FetchCoinsResponse response = this.coinView.FetchCoins(new[] { outPoint });

            return Task.FromResult(response.UnspentOutputs.Values.SingleOrDefault());
        }

        /// <inheritdoc/>
        public Target GetNetworkDifficulty()
        {
            return this.chainState.ConsensusTip?.GetWorkRequired(this.network.Consensus);
        }
    }
}