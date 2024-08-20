using System.Collections.Generic;
using XOuranos.Consensus.BlockInfo;
using XOuranos.NBitcoin;
using XOuranos.Networks;

namespace XOuranos.Mining
{
    public sealed class BlockTemplate
    {
        public Block Block { get; set; }

        public Money TotalFee { get; set; }

        public Dictionary<uint256, Money> FeeDetails { get; set; }

        public BlockTemplate(Network network)
        {
            this.Block = network.CreateBlock();
        }
    }
}