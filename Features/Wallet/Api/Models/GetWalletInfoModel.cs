using XOuranos.NBitcoin;
using XOuranos.Utilities.JsonConverters;
using Newtonsoft.Json;

namespace XOuranos.Features.Wallet.Api.Models
{
    /// <summary>Model for RPC method getwalletinfo.</summary>
    public class GetWalletInfoModel
    {
        [JsonProperty("walletname")]
        public string WalletName { get; set; }

        [JsonProperty("walletversion")]
        public int WalletVersion { get; set; }

        [JsonProperty("balance")]
        [JsonConverter(typeof(MoneyInCoinsJsonConverter))]
        public Money Balance { get; set; }

        [JsonProperty("unconfirmed_balance")]
        [JsonConverter(typeof(MoneyInCoinsJsonConverter))]
        public Money UnConfirmedBalance { get; set; }

        [JsonProperty("immature_balance")]
        [JsonConverter(typeof(MoneyInCoinsJsonConverter))]
        public Money ImmatureBalance { get; set; }
    }
}
