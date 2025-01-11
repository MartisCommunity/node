using System;

namespace Martiscoin.Features.Consensus
{
    public class BlockNotFoundException : Exception
    {
        public BlockNotFoundException(string message) : base(message)
        {
        }
    }
}
