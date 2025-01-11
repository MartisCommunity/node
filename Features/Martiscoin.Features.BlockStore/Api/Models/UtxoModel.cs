using Martiscoin.Consensus.ScriptInfo;
using Martiscoin.NBitcoin;
using Newtonsoft.Json;

namespace Martiscoin.Features.BlockStore.Models
{
    public class UtxoModel
    {
        [JsonProperty]
        public uint256 TxId { get; set; }

        [JsonProperty]
        public uint Index { get; set; }

        [JsonProperty]
        public Script ScriptPubKey { get; set; }

        [JsonProperty]
        public Money Value { get; set; }
    }
}
