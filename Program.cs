// See https://aka.ms/new-console-template for more information
using System.Globalization;
using XOuranos.Builder;
using XOuranos.Configuration;
using XOuranos.Networks;
using XOuranos;
using DBreeze.TextSearch;
using XOuranos.X1;
using XOuranos.Utilities;

async void start()
{
    try
    {
        NodeSettings nodeSettings = new NodeSettings(networksSelector: Networks.X1);
        IFullNodeBuilder nodeBuilder = NodeBuilder.Create(nodeSettings);

        IFullNode node = nodeBuilder.Build();

        if (node != null)
            await node.RunAsync();

        Console.ReadKey();
    }
    catch (Exception ex)
    {
        Console.WriteLine("There was a problem initializing the node. Details: '{0}'", ex);
    }
}


start();