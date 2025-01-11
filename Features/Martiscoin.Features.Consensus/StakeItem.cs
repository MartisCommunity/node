using Martiscoin.Consensus.BlockInfo;
using Martiscoin.NBitcoin;

namespace Martiscoin.Features.Consensus
{
    public class StakeItem
    {
        public uint256 BlockId;

        public BlockStake BlockStake;

        public bool InStore;

        public long Height;
    }
}
