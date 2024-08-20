using System;
using System.Collections.Generic;
using XOuranos.Broadcasters;
using XOuranos.EventBus;
using XOuranos.Features.Wallet.Api.Models;

namespace XOuranos.Features.Wallet.Broadcasters
{
    public class WalletGeneralInfoClientEvent : EventBase
    {
        public WalletGeneralInfoModel WalletInfo { get; set; }

        public string WalletName { get; set; }

        public IEnumerable<AccountBalanceModel> AccountsBalances { get; set; }
    }
}