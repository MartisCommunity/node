﻿using Martiscoin.Controllers.Converters;
using Newtonsoft.Json;

namespace Martiscoin.Features.Wallet.Api.Models
{
    [JsonConverter(typeof(ToStringJsonConverter))]
    public class NewAddressModel
    {
        public string Address { get; set; }

        public NewAddressModel(string address)
        {
            this.Address = address;
        }

        public override string ToString()
        {
            return this.Address;
        }
    }
}
