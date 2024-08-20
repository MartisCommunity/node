using System.Threading.Tasks;
using XOuranos.Consensus.BlockInfo;
using XOuranos.NBitcoin;

namespace XOuranos.Interfaces
{
    public interface INBitcoinBlockRepository
    {
        Task<Block> GetBlockAsync(uint256 blockId);
    }

    public interface IBlockTransactionMapStore
    {
        uint256 GetBlockHash(uint256 trxHash);
    }
}
