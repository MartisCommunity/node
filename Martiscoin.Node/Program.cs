using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Martiscoin.Builder;
using Martiscoin.Configuration;
using Martiscoin.Networks.X1;
using Martiscoin.Utilities;

namespace Martiscoin.Node
{
    public class Program
    {
        public static IFullNode node;

        public static async Task Main(string[] args)
        {
            try
            {
                string chain = "MSC";
                NodeSettings nodeSettings = NetworkSelector.Create(chain, args);
                IFullNodeBuilder nodeBuilder = NodeBuilder.Create(chain, nodeSettings);

                node = nodeBuilder.Build();
                ((X1Main)nodeSettings.Network).Parent = node;

                if (node != null)
                {
                    var find = args.Where(a => { return a.Equals("openbrowser=false"); });
                    if (find.Count() <= 0)
                    {
                        new Thread(delegate ()
                        {
                            System.Threading.Thread.Sleep(1000 * 3);
                            Process.Start(new ProcessStartInfo("http://localhost:" + node.Network.DefaultAPIPort) { UseShellExecute = true });
                        }).Start();
                    }
                    await node.RunAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("There was a problem initializing the node. Details: '{0}'", ex);
            }
        }

        public static void Shutdown()
        {
            node?.NodeLifetime.StopApplication();
        }
    }
}
