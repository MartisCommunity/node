using System.Collections.Generic;
using Martiscoin.NBitcoin;
using Martiscoin.Utilities.JsonConverters;
using Newtonsoft.Json;

namespace Martiscoin.Features.Wallet.Api.Models
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
