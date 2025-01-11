using System;

namespace Martiscoin.Features.Miner
{
    public class MinerException : Exception
    {
        public MinerException(string message) : base(message)
        {
        }
    }
}
