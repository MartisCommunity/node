using XOuranos.Consensus.BlockInfo;
using XOuranos.NBitcoin;

namespace XOuranos.Features.Consensus
{
    public class StakeItem
    {
        public uint256 BlockId;

        public BlockStake BlockStake;

        public bool InStore;

        public long Height;
    }
}
