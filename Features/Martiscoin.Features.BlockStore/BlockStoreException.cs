using System;

namespace Martiscoin.Features.BlockStore
{
    public class BlockStoreException : Exception
    {
        public BlockStoreException(string message) : base(message)
        {
        }
    }
}
