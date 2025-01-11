using Martiscoin.NBitcoin;

namespace Martiscoin.Features.RPC
{
    public class RPCAccount
    {
        public Money Amount { get; set; }
        public string AccountName { get; set; }
    }
}