using System.Threading.Tasks;
using Martiscoin.Consensus;
using Martiscoin.Consensus.BlockInfo;
using Martiscoin.Consensus.Rules;
using Microsoft.Extensions.Logging;

namespace Martiscoin.Features.Consensus.Rules.CommonRules
{
    /// <summary>Ensures a block follows the coinbase rules.</summary>
    public class EnsureCoinbaseRule : PartialValidationConsensusRule
    {
        /// <inheritdoc />
        /// <exception cref="ConsensusErrors.BadCoinbaseMissing">The coinbase transaction is missing in the block.</exception>
        /// <exception cref="ConsensusErrors.BadMultipleCoinbase">The block contains multiple coinbase transactions.</exception>
        public override Task RunAsync(RuleContext context)
        {
            if (context.SkipValidation)
                return Task.CompletedTask;

            Block block = context.ValidationContext.BlockToValidate;

            // First transaction must be coinbase, the rest must not be
            if ((block.Transactions.Count == 0) || !block.Transactions[0].IsCoinBase)
            {
                this.Logger.LogTrace("(-)[NO_COINBASE]");
                ConsensusErrors.BadCoinbaseMissing.Throw();
            }

            for (int i = 1; i < block.Transactions.Count; i++)
            {
                if (block.Transactions[i].IsCoinBase)
                {
                    this.Logger.LogTrace("(-)[MULTIPLE_COINBASE]");
                    ConsensusErrors.BadMultipleCoinbase.Throw();
                }
            }

            return Task.CompletedTask;
        }
    }
}