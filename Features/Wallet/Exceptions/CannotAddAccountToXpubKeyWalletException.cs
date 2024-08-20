using System;

namespace XOuranos.Features.Wallet.Exceptions
{
    public class CannotAddAccountToXpubKeyWalletException : Exception
    {
        public CannotAddAccountToXpubKeyWalletException(string message) : base(message)
        {
        }
    }
}