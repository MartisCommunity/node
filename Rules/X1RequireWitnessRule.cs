using System.Threading.Tasks;
using XOuranos.Consensus.Rules;
using Microsoft.Extensions.Logging;

namespace XOuranos.X1.Rules
{
    /// <summary>
    /// Checks if all transaction in the block have witness.
    /// </summary>
    public class X1RequireWitnessRule : PartialValidationConsensusRule
    {
        public override Task RunAsync(RuleContext context)
        {
            var block = context.ValidationContext.BlockToValidate;

            foreach (var tx in block.Transactions)
            {
                if (!tx.HasWitness)
                {
                    this.Logger.LogTrace($"(-)[FAIL_{nameof(X1RequireWitnessRule)}]".ToUpperInvariant());
                    X1ConsensusErrors.MissingWitness.Throw();
                }
            }

            return Task.CompletedTask;
        }
    }
}