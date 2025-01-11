using Martiscoin.Consensus;
using Martiscoin.Consensus.BlockInfo;
using Martiscoin.Consensus.Chain;
using Martiscoin.Consensus.Rules;
using Microsoft.Extensions.Logging;

namespace Martiscoin.Features.Consensus.Rules.CommonRules
{
    /// <summary>Checks if <see cref="PosBlock"/> timestamp is greater than previous block timestamp.</summary>
    public class HeaderTimeChecksPosRule : HeaderValidationConsensusRule
    {
        /// <inheritdoc />
        /// <exception cref="ConsensusErrors.BlockTimestampTooEarly">Thrown if block time is equal or behind the previous block.</exception>
        public override void Run(RuleContext context)
        {
            ChainedHeader chainedHeader = context.ValidationContext.ChainedHeaderToValidate;

            // Check timestamp against prev.
            if (chainedHeader.Header.Time <= chainedHeader.Previous.Header.Time)
            {
                this.Logger.LogTrace("(-)[TIME_TOO_EARLY]");
                ConsensusErrors.BlockTimestampTooEarly.Throw();
            }
        }
    }
}