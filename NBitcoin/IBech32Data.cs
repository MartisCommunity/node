using XOuranos.Networks;

namespace XOuranos.NBitcoin
{
    public interface IBech32Data : IBitcoinString
    {
        Bech32Type Type
        {
            get;
        }
    }
}
