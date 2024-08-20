using Newtonsoft.Json;

namespace XOuranos.Controllers.Models
{
    /// <summary>
    /// A class containing the necessary parameters for a block search request.
    /// </summary>
    public class DecodeRawTransactionModel
    {
        /// <summary>The transaction to be decoded in hex format.</summary>
        [JsonProperty(PropertyName = "rawHex")]
        public string RawHex { get; set; }
    }
}
