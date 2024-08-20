using System.Collections.Generic;
using XOuranos.NBitcoin;
using XOuranos.Utilities.JsonConverters;
using Newtonsoft.Json;

namespace XOuranos.Features.Wallet.Api.Models
{
    public class ListSinceBlockModel
    {
        [JsonProperty("transactions")]
        public IList<ListSinceBlockTransactionModel> Transactions { get; } = new List<ListSinceBlockTransactionModel>();

        [JsonProperty("lastblock", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(UInt256JsonConverter))]
        public uint256 LastBlock { get; set; }
    }
}
