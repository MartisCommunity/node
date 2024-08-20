using System;

namespace XOuranos.NBitcoin.Protocol
{
    public class ProtocolException : Exception
    {
        public ProtocolException(string message)
            : base(message)
        {
        }
    }
}