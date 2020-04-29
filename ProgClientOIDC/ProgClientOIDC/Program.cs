using ProgClientOIDC.GetConf;
using Server;
using System;
using System.Threading.Tasks;

namespace ProgClientOIDC
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //var client = new ProgClient();

            //await client.GetAuthorizationCode();

            ServerProg.StartServer();
        }
    }
}
