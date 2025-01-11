using Martiscoin.Configuration;

namespace Martiscoin.Node
{
    public static class NetworkSelector
    {
        public static NodeSettings Create(string chain, string[] args)
        {
            chain = chain.ToUpperInvariant();

            NodeSettings nodeSettings = null;

            switch (chain)
            {
                case "MSC":
                    nodeSettings = new NodeSettings(networksSelector: Martiscoin.Networks.X1.Networks.X1, args: args);
                    break;
            }

            return nodeSettings;
        }
    }
}