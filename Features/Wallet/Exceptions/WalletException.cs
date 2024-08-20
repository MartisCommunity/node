using System;

namespace XOuranos.Features.Wallet.Exceptions
{
    public class WalletException : Exception
    {
        public WalletException(string message) : base(message)
        {
        }
    }
}
