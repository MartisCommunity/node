using System.IO;
using XOuranos.Consensus.BlockInfo;
using XOuranos.NBitcoin;
using XOuranos.NBitcoin.Crypto;

namespace XOuranos.X1.Consensus
{
    public class X1BlockHeader : PosBlockHeader
    {
        public override uint256 GetPoWHash()
        {
            byte[] serialized;

            using (var ms = new MemoryStream())
            {
                this.ReadWriteHashingStream(new BitcoinStream(ms, true));
                serialized = ms.ToArray();
            }

            return Sha512T.GetHash(serialized);
        }
    }
}