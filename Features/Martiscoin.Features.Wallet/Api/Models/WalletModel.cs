﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace Martiscoin.Features.Wallet.Api.Models
{
    public class WalletModel
    {
        [JsonProperty(PropertyName = "network")]
        public string Network { get; set; }

        [JsonProperty(PropertyName = "fileName")]
        public string FileName { get; set; }

        [JsonProperty(PropertyName = "addresses")]
        public IEnumerable<string> Addresses { get; set; }
    }
}
