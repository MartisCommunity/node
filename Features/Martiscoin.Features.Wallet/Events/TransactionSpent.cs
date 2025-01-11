using System;
using System.Collections.Generic;
using System.Text;
using Martiscoin.Consensus.TransactionInfo;
using Martiscoin.EventBus;

namespace Martiscoin.Features.Wallet.Events
{
    /// <summary>
    /// Event that is executed when a transactions output is spent in the wallet.
    /// </summary>
    /// <seealso cref="Martiscoin.EventBus.EventBase" />
    public class TransactionSpent : EventBase
    {
        public Transaction SpentTransaction { get; }

        public OutPoint SpentOutPoint { get; }

        public TransactionSpent(Transaction spentTransaction, OutPoint spentOutPoint)
        {
            this.SpentTransaction = spentTransaction;
            this.SpentOutPoint = spentOutPoint;
        }
    }
}