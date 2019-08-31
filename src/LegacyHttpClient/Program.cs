using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace LegacyHttpClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            const int count = 10;
            //Uri targetUri = new Uri("https://www.google.com");
            Uri targetUri = new Uri("http://localhost/HttpConnections/api/values");

            try
            {
                await RunHttpCalls(count, targetUri).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e);
                Console.ResetColor();
            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private static async Task RunHttpCalls(int count, Uri targetUri, bool continueOnCapturedContext = false)
        {
            var timer = new Stopwatch();
            timer.Start();
            ShowActiveTcpConnections();
            timer.Stop();
            Console.WriteLine($"elapsed: {timer.Elapsed}");
            using (var client = new HttpClient())
            {
                for (int i = 0; i < count; i++)
                {
                    timer.Restart();
                    var response = await client.GetAsync(targetUri).ConfigureAwait(continueOnCapturedContext);
                    timer.Stop();
                    Console.WriteLine($"elapsed: {timer.Elapsed}");
                }
            }

            timer.Restart();
            ShowActiveTcpConnections();
            timer.Stop();
            Console.WriteLine($"elapsed: {timer.Elapsed}");
        }

        public static void ShowActiveTcpConnections()
        {
            // https://www.ibm.com/support/knowledgecenter/en/SSLTBW_2.1.0/com.ibm.zos.v2r1.halu101/constatus.htm
            Console.WriteLine("Active TCP Connections");
            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
            TcpConnectionInformation[] connections = properties.GetActiveTcpConnections();
            foreach (var cxByState in connections.GroupBy(c => c.State))
            {
                Console.WriteLine($"[{cxByState.Key}] : {cxByState.Count():0000}");
            }
        }
    }
}
