using System.Linq;
using System.Threading.Tasks;
using Martiscoin.Consensus.Rules;
using Martiscoin.Features.BlockStore.AddressIndexing;
using Martiscoin.Features.MemoryPool.Fee;
using Martiscoin.Networks.X1.Consensus;
using Martiscoin.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Martiscoin.Networks.X1.Rules
{
    /// <summary>
    /// Check the miner reward address must have enough balance.
    /// </summary>
    public class X1CheckPeerConnectRule : PartialValidationConsensusRule
    {
        public override Task RunAsync(RuleContext context)
        {
            X1Main x1 = (X1Main)this.Parent.Network;
            var node = x1.Parent as FullNode;
            var block = context.ValidationContext.BlockToValidate;
            if (block != null && block.Transactions.Count > 0)
            {
                if (block.Transactions.Count(a => a.IsCoinStake == true) > 0) return Task.CompletedTask;
                var find = block.Transactions.Find(a => a.IsCoinBase == true);
                if (find != null && find.Outputs.Count > 0)
                {
                    var address = find.Outputs[0].ScriptPubKey.GetDestinationAddress(this.Parent.Network).ToString();
                    if (!x1.DevAddress.ToLower().Equals(address.ToLower()))
                    {
                        if (node.ChainBehaviorState.BestPeerTip == null)
                        {
                            this.Logger.LogTrace($"(-)[FAIL_{nameof(X1CheckPeerConnectRule)}] Mining Address [" + address + "]".ToUpperInvariant());
                            X1ConsensusErrors.NoPeersConnected.Throw();
                        }
                    }
                }
            }

            return Task.CompletedTask;
        }
    }
}