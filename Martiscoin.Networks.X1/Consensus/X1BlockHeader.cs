using System;
using System.IO;
using Martiscoin.Consensus.BlockInfo;
using Martiscoin.NBitcoin;
using Martiscoin.NBitcoin.Crypto;
using DBreeze.Utils;

namespace Martiscoin.Networks.X1.Consensus
{
    public class X1BlockHeader : PosBlockHeader
    {
        public uint256 LotPowLimit = new uint256("00000fffffffffffffffffffffffffffffffffffffffffffffffffffffffffff");

        public override uint256 GetPoWHash()
        {
            byte[] serialized;

            using (var ms = new MemoryStream())
            {
                this.ReadWriteHashingStream(new BitcoinStream(ms, true));
                serialized = ms.ToArray();
            }

            serialized[76] = 0;
            serialized[77] = 0;
            serialized[78] = 0;
            serialized[79] = 0;

            var tempBuffer = X1HashX13.Instance.Hash(serialized);
            serialized = tempBuffer.Concat(new byte[16]);

            var bytes = BitConverter.GetBytes(this.Nonce);
            serialized[76] = bytes[0];
            serialized[77] = bytes[1];
            serialized[78] = bytes[2];
            serialized[79] = bytes[3];

            return Sha512T.GetHash(serialized);
        }

        public bool CheckLotProofOfWork(uint nonce)
        {
            byte[] serialized;

            using (var ms = new MemoryStream())
            {
                this.ReadWriteHashingStream(new BitcoinStream(ms, true));
                serialized = ms.ToArray();
            }

            serialized[76] = 0;
            serialized[77] = 0;
            serialized[78] = 0;
            serialized[79] = 0;

            var tempBuffer = X1HashX13.Instance.Hash(serialized);
            serialized = tempBuffer.Concat(new byte[16]);

            var bytes = BitConverter.GetBytes(this.Nonce);
            serialized[76] = bytes[0];
            serialized[77] = bytes[1];
            serialized[78] = bytes[2];
            serialized[79] = bytes[3];

            uint256 headerHash = Sha512T.GetHash(serialized);
            var res = headerHash <= this.LotPowLimit;
            return res;
        }
    }
}