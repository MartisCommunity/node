﻿using Martiscoin.NBitcoin;
using Martiscoin.Utilities.JsonConverters;
using Newtonsoft.Json;

namespace Martiscoin.Features.Wallet.Api.Models
{
    public class WalletBuildTransactionModel
    {
        [JsonProperty(PropertyName = "fee")]
        public Money Fee { get; set; }

        [JsonProperty(PropertyName = "hex")]
        public string Hex { get; set; }

        [JsonProperty(PropertyName = "transactionId")]
        [JsonConverter(typeof(UInt256JsonConverter))]
        public uint256 TransactionId { get; set; }
    }
}
