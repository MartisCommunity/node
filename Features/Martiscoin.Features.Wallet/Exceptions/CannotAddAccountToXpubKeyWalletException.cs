using System;

namespace Martiscoin.Features.Wallet.Exceptions
{
    public class CannotAddAccountToXpubKeyWalletException : Exception
    {
        public CannotAddAccountToXpubKeyWalletException(string message) : base(message)
        {
        }
    }
}