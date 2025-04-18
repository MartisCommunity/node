﻿using System;

namespace Martiscoin.Features.Wallet.AddressBook
{
    /// <summary>
    /// An exception thrown by operations on the address book.
    /// </summary>
    public class AddressBookException : Exception
    {
        public AddressBookException(string message) : base(message)
        {
        }

        public AddressBookException()
        {
        }

        public AddressBookException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
