using XOuranos.Consensus;
using XOuranos.Consensus.BlockInfo;
using XOuranos.Consensus.Rules;
using XOuranos.Consensus.TransactionInfo;
using XOuranos.Features.Consensus;
using XOuranos.Features.Consensus.Rules.CommonRules;
using XOuranos.Features.Consensus.Rules.UtxosetRules;
using XOuranos.NBitcoin;
using XOuranos.Utilities;
using Microsoft.Extensions.Logging;

namespace XOuranos.Features.PoA.BasePoAFeatureConsensusRules
{
    public class PoACoinviewRule : CheckUtxosetRule
    {
        private PoANetwork network;

        /// <inheritdoc />
        public override void Initialize()
        {
            base.Initialize();

            this.network = this.Parent.Network as PoANetwork;
        }

        /// <inheritdoc/>
        public override void CheckBlockReward(RuleContext context, Money fees, int height, Block block)
        {
            Money reward = Money.Zero;

            if (height == this.network.Consensus.PremineHeight)
                reward = this.network.Consensus.PremineReward;

            if (block.Transactions[0].TotalOut > fees + reward)
            {
                this.Logger.LogTrace("(-)[BAD_COINBASE_AMOUNT]");
                ConsensusErrors.BadCoinbaseAmount.Throw();
            }
        }

        /// <inheritdoc/>
        public override Money GetProofOfWorkReward(int height)
        {
            if (height == this.network.Consensus.PremineHeight)
                return this.network.Consensus.PremineReward;

            return 0;
        }

        protected override Money GetTransactionFee(UnspentOutputSet view, Transaction tx)
        {
            return view.GetValueIn(tx) - tx.TotalOut;
        }

        /// <inheritdoc/>
        public override void CheckMaturity(UnspentOutput coins, int spendHeight)
        {
            base.CheckCoinbaseMaturity(coins, spendHeight);
        }

        /// <inheritdoc/>
        public override void UpdateCoinView(RuleContext context, Transaction transaction)
        {
            base.UpdateUTXOSet(context, transaction);
        }
    }
}