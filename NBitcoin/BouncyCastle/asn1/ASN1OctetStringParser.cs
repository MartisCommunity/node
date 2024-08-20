using System.IO;

namespace XOuranos.NBitcoin.BouncyCastle.asn1
{
    internal interface Asn1OctetStringParser
        : IAsn1Convertible
    {
        Stream GetOctetStream();
    }
}
