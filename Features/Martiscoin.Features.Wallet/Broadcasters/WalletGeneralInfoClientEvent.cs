using System;
using System.Collections.Generic;
using Martiscoin.Broadcasters;
using Martiscoin.EventBus;
using Martiscoin.Features.Wallet.Api.Models;

namespace Martiscoin.Features.Wallet.Broadcasters
{
    public class WalletGeneralInfoClientEvent : EventBase
    {
        public WalletGeneralInfoModel WalletInfo { get; set; }

        public string WalletName { get; set; }

        public IEnumerable<AccountBalanceModel> AccountsBalances { get; set; }
    }
}